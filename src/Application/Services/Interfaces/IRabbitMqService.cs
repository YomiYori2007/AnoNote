namespace PetProject.Application.Services.Interfaces;

public interface IRabbitMqService
{
    public void SendMessage<T>(T message);
}