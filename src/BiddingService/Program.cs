using BiddingService.Consumers;
using BiddingService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddHostedService<CheckAuctionFinished>();

builder.Services.AddMassTransit(x=>
{
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    x.UsingRabbitMq((context,cfg)=> {
        cfg.Host(builder.Configuration["RabbitMq:Host"],"/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username","guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password","guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids",false));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });
builder.Services.AddScoped<GrpcAuctionClient>();
var app = builder.Build();




app.MapControllers();

await DB.InitAsync("BidDb"
    , MongoClientSettings.FromConnectionString(
        builder.Configuration.GetConnectionString("BidDbConnection")));






app.Run();

