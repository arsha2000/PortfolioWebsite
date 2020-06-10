using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace PortfolioWebsite.Services
{
    public class JSONFileManager<T>
    {

        public readonly FileManager FileManager;

        public JSONFileManager(FileManager fileManager)
        {
            FileManager = fileManager;
        }

        public void WriteToFile(string fileName, IEnumerable<T> items)
        {
            var currentItems = ReadAll(fileName).ToList();
            currentItems.AddRange(items);

            var outputStream = FileManager.GetFile(fileName, mode: System.IO.FileMode.OpenOrCreate);
            JsonSerializer.Serialize(new Utf8JsonWriter(outputStream), currentItems);
        }


        public IEnumerable<T> ReadAll(string fileName)
        {
            string json = FileManager.ReadFileAsText(fileName);

            return JsonSerializer.Deserialize<T[]>(json);
        }

        public async Task<IEnumerable<T>> ReadAllAsync(string fileName)
        {
            var stream = FileManager.ReadFile(fileName);

            return await JsonSerializer.DeserializeAsync<T[]>(stream,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
