using ServiceBus.Api.Messaging;
using ServiceBus.Contracts;

namespace ServiceBus.Api;

public class AccountMessageHandler(ILogger<AccountMessageHandler> logger) : MessageHandlerBase<AccountMessage>(logger)
{
    public override string QueueName => "accounts";

    public override Task HandleMessageAsync(AccountMessage message)
    {
        if (message.AccountNumber % 2 == 0)
        {
            throw new InvalidOperationException("Account number is even, throwing exception for testing purposes.");
        }
        logger.LogInformation($"Processing account: {message.AccountId} with number: {message.AccountNumber}");
        return Task.CompletedTask;
    }
}
