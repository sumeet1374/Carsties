using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController:ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItem([FromQuery]SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item,Item>();
       // query.Sort(x=>x.Ascending(a=>a.Make));
  
        if(!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full,searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make"=> query.Sort(x=>x.Ascending(a=>a.Make)).Sort(a=>a.Ascending(x=>x.Model)),
             "new"=> query.Sort(x=>x.Descending(a=>a.CreatedAt)),
              _=> query.Sort(x=>x.Ascending(a=>a.AuctionEnd))
        };

        query = searchParams.FilterBy switch
        {
            "finished"=> query.Match(X=>X.AuctionEnd < DateTime.UtcNow),
            "endingSoon"=> query.Match(x=>x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
            _=> query.Match(x=>x.AuctionEnd >DateTime.UtcNow)
        };

        if(!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x=> x.Seller == searchParams.Seller);
        }

        if(!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x=> x.Winner == searchParams.Winner);
        }


        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);
        var result = await query.ExecuteAsync();
        return  Ok(new {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
