using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using AuctionService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt=> { 
opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=> {

    x.AddEntityFrameworkOutbox<AuctionDbContext>(o=> {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.UsingRabbitMq((context,cfg)=> {
        cfg.ConfigureEndpoints(context);
    });
   
    x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction",false));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.

// Seed Data
try
{
    DbInitializer.InitDb(app);
}
catch(Exception ex)
{
    Console.WriteLine($"Exception is {ex.Message}");
}
app.MapControllers();


app.Run();

