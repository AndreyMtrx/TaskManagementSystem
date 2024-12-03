using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace TaskManagementSystem.Application;

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public ServiceBusHandler(string connectionString, string queueName)
    {
        var clientOptions = new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Mode = ServiceBusRetryMode.Exponential,
                MaxRetries = 5,
                Delay = TimeSpan.FromSeconds(1),
                MaxDelay = TimeSpan.FromSeconds(30),
            }
        };

        _client = new ServiceBusClient(connectionString, clientOptions);
        _sender = _client.CreateSender(queueName);
    }

    public async Task SendMessage(object messageObject, string action)
    {
        var messageBody = JsonSerializer.Serialize(new { Action = action, Data = messageObject });
        var message = new ServiceBusMessage(messageBody);

        try
        {
            await _sender.SendMessageAsync(message);
        }
        catch (ServiceBusException ex) when (ex.IsTransient)
        {
            Console.WriteLine($"Transient error sending message: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
            throw;
        }
    }

    public async Task ReceiveMessages()
    {
        var processorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = false
        };

        var processor = _client.CreateProcessor(_sender.EntityPath, processorOptions);

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            var jsonString = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<ServiceBusMessageModel>(jsonString);

            Console.WriteLine($"Received message: {message.Action}");

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error in message processing: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}

public class ServiceBusMessageModel
{
    public string Action { get; set; }
    public object Data { get; set; }
}
