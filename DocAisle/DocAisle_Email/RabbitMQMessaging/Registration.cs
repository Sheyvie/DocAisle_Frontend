using DocAisle_Email.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MimeKit;
using DocAisle_Email.Service;

namespace DocAisle_Email.RabbitMQMessaging
{
    public class Registration:BackgroundService
    {


        private readonly EmailService _toDb;
        private readonly EmailSendService _sendEmail;
        private IConfiguration _configuration;
        private IModel _channel;
        private readonly IConnection _connection;


        public Registration(EmailService emailService,IConfiguration configuration)
        {
            _toDb = emailService;
            _configuration = configuration;
            _sendEmail = new EmailSendService(_configuration);
        

        
        
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp:guest:guest@locahhost:5672");

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            //Declare queue

            _channel.QueueDeclare(_configuration.GetSection("ExchangeType:RegisterUser").Get<string>(), false, false, false, null);
        }

         protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();


           
            var consumer = new EventingBasicConsumer(_channel);
            //fires a receive message

            consumer.Received += (sender, eventArgs) =>
            {
                //decode
                var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var userDetails = JsonConvert.DeserializeObject<UserDetails>(content);

                sendEmail(userDetails).GetAwaiter().GetResult();


                //deleting from queue
                _channel.BasicAck(eventArgs.DeliveryTag, false);

                Console.WriteLine(userDetails);
            };

            _channel.BasicConsume(_configuration.GetSection("ExchangeType:RegisterUser").Get<string>(), false, consumer);
           return Task.CompletedTask;

        }

        private async Task sendEmail(UserDetails userDetails)
        {


            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://api.logo.com/api/v2/images?logo=logo_9fafefd9-1c3d-46e0-8459-0186396f93cb&format=webp&margins=100&quality=60\" width=\"1000\" height=\"600\">");
                stringBuilder.Append("<h1> Hello " + userDetails.UserName + "</h1>");
                stringBuilder.AppendLine("<br/>Welcome to DocAisle,Best Doctors Lined for You ");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');

                var emailDto = new EmailDto()
                {
                    Email = userDetails.Email,
                    Body = stringBuilder.ToString()

                };
                await _toDb.SaveToDb(emailDto);
                await _sendEmail.SendEmail(userDetails, stringBuilder.ToString());


            }

            catch (Exception ex) { }
        }




    }


}


