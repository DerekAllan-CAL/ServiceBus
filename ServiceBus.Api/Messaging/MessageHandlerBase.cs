using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace ServiceBus.Api.Messaging;

public abstract class MessageHandlerBase<T>(ILogger logger) : IMessageHandler
{
    private readonly ILogger _logger = logger;

    public abstract string QueueName { get; }
    public async Task ProcessMessageAsync(ProcessMessageEventArgs messageArguments)
    {
        try
        {
            var body = messageArguments.Message.Body.ToString();
            var deserializedMessage = JsonSerializer.Deserialize<T>(body) ?? throw new InvalidOperationException("Deserialized message is null.");
            await HandleMessageAsync(deserializedMessage);
            await messageArguments.CompleteMessageAsync(messageArguments.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to complete message in queue {QueueName}. Message: {Message}", QueueName, messageArguments.Message.Body.ToString());
            throw;
        }
    }

    public Task ProcessErrorAsync(ProcessErrorEventArgs errorArguments)
    {
        _logger.LogError(errorArguments.Exception, "Error processing message from queue {QueueName}", QueueName);
        return Task.CompletedTask;
    }

    public abstract Task HandleMessageAsync(T message);
}