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
    private readonly IServiceProvider _serviceProvider; // Для получения сервисов через DI

    public RabbitMqConsumerService(IOptions<EmailSettings> emailSettings, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var settings = emailSettings.Value;

        // Настройка подключения к RabbitMQ (используем те же настройки, что и для отправки)
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // Хост RabbitMQ. По умолчанию - localhost
            Port = 5672,            // Порт RabbitMQ. По умолчанию - 5672
            UserName = "guest",     // Логин RabbitMQ. По умолчанию - guest
            Password = "guest",     // Пароль RabbitMQ. По умолчанию - guest
            VirtualHost = "/",
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        // Имя очереди должно совпадать с тем, что используется в Producer
        _queueName = "email_queue"; 

        // Объявляем очередь. ВАЖНО: параметры должны совпадать с Producer!
        _channel.QueueDeclare(queue: _queueName,
                             durable: true,    // Сохранять очередь при перезагрузке RabbitMQ
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        // Настраиваем QoS (качество обслуживания). 
        // Не обрабатывать более 1 сообщения за раз для каждого Consumer'a
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
            var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message); // Десериализуем

            bool success = false;

            try
            {
                // Создаем scope для получения сервисов с коротким временем жизни (Scoped)
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Получаем EmailService из контейнера зависимостей
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
                // Логируем ошибку (в реальном проекте используйте ILogger)
                Console.WriteLine($"❌ Ошибка при обработке сообщения: {ex.Message}");
                // Сообщение НЕ подтверждено и будет повторно доставлено другому Consumer'у или позже
            }
            finally
            {
                if (success)
                {
                    // Вручную подтверждаем (ack) успешную обработку сообщения
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    Console.WriteLine($"✅ Письмо на {emailMessage.ToEmail} отправлено и подтверждено.");
                }
                else
                {
                    // Отклоняем сообщение (nack) без повторной постановки в очередь
                    // Если requeue = true, сообщение вернется в очередь. 
                    // Если false - сообщение будет удалено или отправлено в Dead Letter Exchange (если настроен)
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: !success); 
                    Console.WriteLine($"❌ Сообщение для {emailMessage.ToEmail} отклонено (Nack).");
                }
            }
        };

        // Начинаем потребление сообщений. autoAck: FALSE - это ВАЖНО для ручного подтверждения!
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        Console.WriteLine(" Consumer запущен и ожидает сообщения...");

        // Ждем сигнала отмены, чтобы корректно завершить работу
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}