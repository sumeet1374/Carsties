using Contracts;
using MassTransit;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumers;

public class AuctionCreatedConsumer:NotificationConsumerBase <AuctionCreated>
{
    public AuctionCreatedConsumer(IHubContext<NotificationHub> hub) : base(hub)
    {
    }

    protected override string ConsumerDescription => "Auction Created";
    protected override string ConsumerName => "AuctionCreated";
}
