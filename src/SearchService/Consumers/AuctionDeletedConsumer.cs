using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine("Deletd consumer");
        var message = context.Message;
        var result = await DB.DeleteAsync<Item>(message.Id);
        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
    }
}
