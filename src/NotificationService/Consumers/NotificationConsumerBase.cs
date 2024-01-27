using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumers;

public abstract class NotificationConsumerBase<T> :IConsumer<T> where T:class
{
    private readonly IHubContext<NotificationHub> _hub;
    
    protected abstract string ConsumerDescription { get; }
    protected abstract string ConsumerName { get; }

    public NotificationConsumerBase(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }
    public async Task Consume(ConsumeContext<T> context)
    {
        Console.WriteLine($"----> {ConsumerDescription} Message Received");
        Console.WriteLine($"----- Message Name {ConsumerName}");
        await _hub.Clients.All.SendAsync( ConsumerName, context.Message);
    }
}