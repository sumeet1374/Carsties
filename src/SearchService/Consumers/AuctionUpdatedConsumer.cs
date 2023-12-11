using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : ConsumerBase<AuctionUpdated>
{
    public AuctionUpdatedConsumer(IMapper mapper) : base(mapper)
    {
    }

    public override async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("Updating the entity");
        var model = context.Message;
        var item = _mapper.Map<Item>(model);
        var result = await DB.Update<Item>()
           .Match(x=>x.ID == context.Message.Id)
           .ModifyOnly(x=>new { x.Make,x.Mileage,x.Year,x.Color,x.Model},item)
           .ExecuteAsync();
        
        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
    }
}
