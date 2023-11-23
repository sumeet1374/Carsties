using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>();
var app = builder.Build();
try
{
    await DbInitializer.InitDb(app);
}
catch(Exception ex)
{
    Console.WriteLine($"Error in creating seed data {ex.Message}");
}
app.MapControllers();

// Configure the HTTP request pipeline.

// await DB.InitAsync("SearchDb",MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));
// await DB.Index<Item>().Key(a=>a.Make,KeyType.Text)
//                       .Key(a=>a.Model,KeyType.Text)
//                       .Key(a=>a.Color,KeyType.Text)
//                       .CreateAsync();



app.Run();


