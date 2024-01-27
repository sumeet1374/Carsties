using Contracts;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumers;


public class AuctionFinishedConsumer:NotificationConsumerBase<AuctionFinished>
{
    public AuctionFinishedConsumer(IHubContext<NotificationHub> hub) : base(hub)
    {
    }

    protected override string ConsumerDescription => "Auction Finished";
    protected override string ConsumerName => "AuctionFinished";
}