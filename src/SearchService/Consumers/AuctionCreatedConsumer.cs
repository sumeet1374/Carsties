using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionCreatedConsumer : ConsumerBase<AuctionCreated>
{
    public AuctionCreatedConsumer(IMapper mapper) : base(mapper)
    {
    }

    public override async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"----> Consuming Auction Created : {context.Message.Id}");
        if(context.Message.Model == "Foo")
            throw new ArgumentException("Model Cannot be Foo");
        var item = _mapper.Map<Item>(context.Message);
        await item.SaveAsync();
    }
}
