using DocAisle_Auth.Services.IService;
using DocAisle_Auth.Services;
using System;
using DocAisle_Auth.Db;
using Microsoft.EntityFrameworkCore;
using DocAisle_Auth.Extensions;
using Microsoft.AspNetCore.Identity;
using DocAisle_Auth.Model;
using DocAisle_Auth.Utility;
using MassTransit;
using DocAisle_Auth.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);






builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//connecting to database
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});
//identityFramework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<UsersDbContext>();



builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IJwtInterface, JwtService>();
builder.Services.AddScoped<IMessagePublisherInterface, MessagePublisher>();


//Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//JWT options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
//builder.Services.AddHostedService<Registration>();
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
