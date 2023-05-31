using condogenius_api_reservation.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using condogenius_api_reservation.Controllers;
using condogenius_api_reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>
(
    options => options.UseMySQL("server=localhost;port=3306;database=condogenius;user=root")
);

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton<IModel>(serviceProvider =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();
    return channel;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseSwagger();

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Consumer
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<DataContext>();
    var channel = serviceProvider.GetRequiredService<IModel>();

    string queueName = "reservations";
    channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Mensagem recebida: {0}", message);

        // Converter a mensagem para o objeto Reservation
        var reservation = JsonConvert.DeserializeObject<Reservation>(message);

        // Chamar o método Create do ReservationsController para criar a reserva
        var reservationsController = new ReservationsController(context);
        var result = reservationsController.Create(reservation);

        if (result is CreatedResult createdResult)
        {
            Console.WriteLine("Reserva cadastrada!");
            string projectId = "handy-courage-388421";
            string topicId = "send-notifications";
            string mgm = "Reserva realizada com sucesso para o dia " + reservation.reserve_date;

            PubSubUtils pubSubUtils = new PubSubUtils();
            pubSubUtils.SendMessageToPubSub(projectId, topicId, mgm);
        } 
        else 
        {
            Console.WriteLine("Erro ao cadastrar reserva: ", reservation);
        }

    };

    channel.BasicConsume(queueName, autoAck: true, consumer: consumer);

    Console.ReadLine();
}


app.Run();
