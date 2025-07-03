using Azure.Messaging.ServiceBus;

namespace ServiceBus.Api.Messaging;

public class MessageProcessorService(ServiceBusClient serviceBusClient, IEnumerable<IMessageHandler> handlers) : IHostedService
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;

    private Dictionary<string, ServiceBusProcessor> _processors = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return StartProcessorsAsync();
    }

    public Task StartProcessorsAsync()
    {
        foreach (var handler in handlers)
        {
            var processor = _serviceBusClient.CreateProcessor(handler.QueueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 5,
                AutoCompleteMessages = false
            });

            processor.ProcessMessageAsync += handler.ProcessMessageAsync;
            processor.ProcessErrorAsync += handler.ProcessErrorAsync;
            _ = processor.StartProcessingAsync();
            _processors[handler.QueueName] = processor;
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return StopProcessorsAsync();
    }

    public async Task StopProcessorsAsync()
    {
        var tasks = new List<Task>();
        foreach (var processor in _processors.Values)
        {
            if (processor.IsProcessing)
            {
                tasks.Add(processor.StopProcessingAsync());
            }
        }
        await Task.WhenAll(tasks);
        _processors.Clear();
    }
}
