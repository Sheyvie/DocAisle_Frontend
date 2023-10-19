using DocAisle_Email.Extensions;
using DocAisle_Email.Service;
using Microsoft.EntityFrameworkCore;
using DocAisle_Email.RabbitMQMessaging;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DocAisle_Email.Db.EmailDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});


var dbContextBuilder = new DbContextOptionsBuilder<DocAisle_Email.Db.EmailDbContext>();
dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
builder.Services.AddSingleton(new EmailService(dbContextBuilder.Options));

builder.Services.AddHostedService<Registration>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMigration();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
