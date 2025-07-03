# MessageProcessorService

The `MessageProcessorService` is designed to process messages from a queue or topic, delegating message handling logic to custom handlers. The `MessageProcessorService` is implemeneted as a HostedService such that it runs for the lifetime of the application and all handlers are also running for the lifetime of the service.

## Implementing a Message Handler

To create a custom message handler, inherit from `MessageHandlerBase` and override the required methods:

The message handler consists of a strongly typed asyn HandleMessageAsync function as well as a queueName for queue per message type implementation 

```csharp
public class MyMessageHandler : MessageHandlerBase<MyMessage>
{
    public override string QueueName => "{queueName}";
    protected override Task HandleMessageAsync(MyMessage message)
    {
        // Implement your message processing logic here
        return Task.CompletedTask;
    }
}
```

## Registering the Handler with Dependency Injection

Register your handler in the DI container, typically in `Startup.cs` or your service configuration:

```csharp
services.AddTransient<IMessageHandler, MyMessageHandler>();
services.AddHostedService<MessageProcessorService>();
```

This ensures your handler is discovered and used by the `MessageProcessorService` at runtime.
