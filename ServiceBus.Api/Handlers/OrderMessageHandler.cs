using ServiceBus.Api.Messaging;
using ServiceBus.Contracts;

namespace ServiceBus.Api.Handlers;

public class OrderMessageHandler(ILogger<OrderMessageHandler> logger) : MessageHandlerBase<OrderMessage>(logger)
{
    public override string QueueName => "orders";

    public override Task HandleMessageAsync(OrderMessage message)
    {
        // Handle the order message
        logger.LogInformation($"Processing order: {message.OrderId} with amount: {message.Amount}");
        return Task.CompletedTask;
    }
}
