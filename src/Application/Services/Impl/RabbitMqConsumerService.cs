using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using PetProject.Application.Models;
using PetProject.Application.Services.Interfaces;

namespace PetProject.Application.Services.Impl;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IServiceProvider _serviceProvider; 

    public RabbitMqConsumerService(IOptions<EmailSettings> emailSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var settings = emailSettings.Value;

        // Настройка подключения к RabbitMQ
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _queueName = "email_queue"; 
        
        _channel.QueueDeclare(queue: _queueName,
                             durable: true,    // Сохранять очередь при перезагрузке RabbitMQ
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);

            bool success = false;

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    
                    // Отправляем письмо!
                    await emailService.SendEmailAsync(
                        emailMessage.ToEmail, 
                        emailMessage.Subject, 
                        emailMessage.Body
                    );
                }

                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при обработке сообщения: {ex.Message}");
            }
            finally
            {
                if (success)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    Console.WriteLine($"✅ Письмо на {emailMessage.ToEmail} отправлено и подтверждено.");
                }
                else
                {
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: !success); 
                    Console.WriteLine($"❌ Сообщение для {emailMessage.ToEmail} отклонено (Nack).");
                }
            }
        };
        
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        Console.WriteLine(" Consumer запущен и ожидает сообщения...");
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}