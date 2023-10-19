using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DocAisle_Auth.RabbitMQ
{
    public class MessagePublisher : IMessagePublisherInterface
    {
        private IConnection _connection;
        public void PublishMessage(object message, string exchangeTypeName)
        {
            if (verifyConnection())
            {
                var channel = _connection.CreateModel();
                channel.QueueDeclare(exchangeTypeName, false, false, false, null);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: exchangeTypeName, null, body: body);
            }
        }
            private void createConnection()
            {
                try
                {
                    var factory = new ConnectionFactory();
                    factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
                    _connection = factory.CreateConnection();
                    var channel = _connection.CreateModel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            public bool verifyConnection()
            {
                if (_connection != null)
                {
                    return true;
                }
                createConnection();
                return true;

            }
        
    }
}
