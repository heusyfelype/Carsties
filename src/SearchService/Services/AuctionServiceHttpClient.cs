using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _config;

        public AuctionServiceHttpClient(HttpClient httpclient, IConfiguration config)
        {
            _httpclient = httpclient;
            _config = config;
        }

        public async Task<List<Item>> GetItensForSearchDb()
        {
            var lastUpdate = await DB.Find<Item, string>()
                .Sort(x => x.Descending(x => x.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            return await _httpclient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdate);
        }
    }
}
