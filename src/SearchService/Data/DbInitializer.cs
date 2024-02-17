using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;
using System.Text.Json;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            await DB.InitAsync("SearchDb", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            #region Codigo Para ler arquivo json
            /* PARA LER OS DADOS A PARTIR DE UMARQUIVO JSON - APENAS TESTE*/
            /* if(count == 0)
             {
                 Console.WriteLine("No data - will attempt to seed");
                 var itemData = await File.ReadAllTextAsync("Data/auctions.json");

                 var optins = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                 var items = JsonSerializer.Deserialize<List<Item>>(itemData, optins);

                 await DB.SaveAsync(items);
             }*/
            #endregion

            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

            var items = await httpClient.GetItensForSearchDb();

            Console.WriteLine(items.Count + " returned from the auction service");

            if (items.Count > 0) await DB.SaveAsync(items);

        }
    }
}
