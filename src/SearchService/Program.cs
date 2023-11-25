using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolocy());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=> {
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search",false));

    x.UsingRabbitMq((context,cfg)=> {

        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(async () =>
{

    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in creating seed data {ex.Message}");
    }
});

app.MapControllers();

// Configure the HTTP request pipeline.

// await DB.InitAsync("SearchDb",MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));
// await DB.Index<Item>().Key(a=>a.Make,KeyType.Text)
//                       .Key(a=>a.Model,KeyType.Text)
//                       .Key(a=>a.Color,KeyType.Text)
//                       .CreateAsync();



app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolocy()
=> HttpPolicyExtensions.HandleTransientHttpError()
   .OrResult(message => message.StatusCode == HttpStatusCode.NotFound)
   .WaitAndRetryForeverAsync(_=> TimeSpan.FromSeconds(3));