using AutoMapper;
using MassTransit;

namespace SearchService;

public abstract class ConsumerBase<T> : IConsumer<T> where T:class
{
    protected readonly IMapper _mapper;

    public ConsumerBase(IMapper mapper)
    {
        _mapper = mapper;
    }

    public abstract  Task Consume(ConsumeContext<T> context);
   
}
