using Contracts;
using MassTransit.SqlTransport;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumers;

public class BidPlacedConsumer:NotificationConsumerBase<BidPlaced>
{
    public BidPlacedConsumer(IHubContext<NotificationHub> hub) : base(hub)
    {
    }

    protected override string ConsumerDescription => "Bid Placed";
}