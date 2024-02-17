using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
        {
            var query = DB.PagedSearch<Item, Item>();

            if (!String.IsNullOrWhiteSpace(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(y => y.Make)),
                "new" => query.Sort(x => x.Descending(y => y.CreatedAt)),
                _ => query.Sort(x => x.Ascending(y => y.AuctionEnd)),
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
            };

            if (!string.IsNullOrWhiteSpace(searchParams.Seller))
            {
                query.Match(x => x.Seller == searchParams.Seller);
            }

            if (!string.IsNullOrWhiteSpace(searchParams.Winner))
            {
                query.Match(x => x.Winner == searchParams.Winner);
            }


            var result = await query.ExecuteAsync();

            return Ok(new
            {
                result = result.Results,
                pageCount = result.PageCount,
                totalcount = result.TotalCount
            });
        }

    }
}
