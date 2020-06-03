using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PortfolioWebsite.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace PortfolioWebsite.Services
{
    public class PortfolioJSONManager
    {

        private static readonly string fileName = "Models/portfolioItems.json";
        private static string FilePath()
        {
            return Path.GetFullPath(fileName);
        }

        public PortfolioJSONManager()
        {
            
        }

        public IEnumerable<PortfolioItem> AllItems()
        {
            using var streamReader = File.OpenText(FilePath());

            return JsonSerializer.Deserialize<PortfolioItem[]>(streamReader.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
        }

        public async Task<IEnumerable<PortfolioItem>> AllItemsAsync()
        {
            
            using FileStream fileStream = File.Open(FilePath(), FileMode.Open);
            return await JsonSerializer.DeserializeAsync<PortfolioItem[]>(fileStream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

    }
}
