using PetProject.Application.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace PetProject.Application.Services.Impl;

public class RabbitMqService
{
    private readonly string _connectionString;
    private readonly string _queueName;

    public RabbitMqService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("RabbitMq");
        _queueName = configuration["RabbitMq:QueueName"];
    }

    public void SendMessage<T>(T message)
    {
        try
        {
            var factory = new ConnectionFactory { Uri = new Uri(_connectionString) };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка в SendMessage");
            throw ex;
        }
    }
}