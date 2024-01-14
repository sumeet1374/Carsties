using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
  
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        
        var message = context.Message;
        Console.WriteLine($"Delete Message Received {context.Message.ToString()}");
        var result = await DB.DeleteAsync<Item>(message.Id);
        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
    }
}
