using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumers;


public class AuctionFinishedConsumer:NotificationConsumerBase<AuctionFinishedConsumer>
{
    public AuctionFinishedConsumer(IHubContext<NotificationHub> hub) : base(hub)
    {
    }

    protected override string ConsumerDescription => "Auction Finished";
}