using Azure.Messaging.ServiceBus;

namespace ServiceBus.Api.Messaging;

public interface IMessageHandler
{
    string QueueName { get; }
    Task ProcessMessageAsync(ProcessMessageEventArgs messageArguments);
    Task ProcessErrorAsync(ProcessErrorEventArgs errorArguments);
}
