namespace TaskManagementSystem.Application;

public interface IServiceBusHandler
{
    Task SendMessage(object messageObject, string action);
    Task ReceiveMessages();
}
