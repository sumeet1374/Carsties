using MongoDB.Entities;

namespace SearchService;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AuctionServiceHttpClient(HttpClient httpClient,IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item,string>()
                         .Sort(x=>x.Descending(y=>y.UpdatedAt))
                         .Project(x=>x.UpdatedAt.ToString())
                         .ExecuteFirstAsync();
        var url = _config["AuctionServiceUrl"];
        return await _httpClient.GetFromJsonAsync<List<Item>>($"{url}/api/auctions?date={lastUpdated}");
    }
}
