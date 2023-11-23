using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public static class DbInitializer
{
    public static async Task InitDb( WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("MongoDbConnection");
        Console.WriteLine($"Connection string is {connectionString}");
        await DB.InitAsync("SearchDb",MongoClientSettings.FromConnectionString(connectionString));
        await DB.Index<Item>().Key(a=>a.Make,KeyType.Text)
                      .Key(a=>a.Model,KeyType.Text)
                      .Key(a=>a.Color,KeyType.Text)
                      .CreateAsync();

        var count = await DB.CountAsync<Item>();
        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine($"{items.Count} returned from the auction  service");

        if(items.Count > 0)
        { 
            await DB.SaveAsync(items);
        }
        // if(count == 0)
        // {
        //     Console.WriteLine("There is no data.Will attempt to seed");
        //     var itemsData = await File.ReadAllTextAsync("Data/Auctions.json");

        //     var options =new JsonSerializerOptions() { PropertyNameCaseInsensitive = true};
        //     var items = JsonSerializer.Deserialize<List<Item>>(itemsData,options);
        //     await DB.SaveAsync(items);

        // }

    }
}
