using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionFinishedConsumer:IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("=> ---- Consuming Auction Finished");
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            if (context.Message.Amount != null) auction.SoldAmount = context.Message.Amount.Value;
            auction.Status = "Finished";
            await auction.SaveAsync();
        }
    }
}