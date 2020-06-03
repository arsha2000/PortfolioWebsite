using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Services
{
    public class JSONFileManager<T>
    {

        private IWebHostEnvironment _env { get; set; }
        private string DirPath { get
            {
                return Path.Combine(_env.ContentRootPath, "Data");
            }
        }


        public JSONFileManager(IWebHostEnvironment env)
        {
            _env = env;
        }



        public void WriteToFile(string fileName, IEnumerable<T> items)
        {

            string filePath = Path.Combine(DirPath, fileName);

            var currentItems = ReadAll(fileName).ToList();
            foreach (var item in items)
                currentItems.Add(item);

            using var outputStream = File.OpenWrite(filePath);

            JsonSerializer.Serialize(new Utf8JsonWriter(outputStream), currentItems);
        }

        //public async Task WriteToFileAsync(string fileName, IEnumerable<T> items)
        //{
        //    string filePath = Path.Combine(DirPath, fileName);
        //    using var inputStream = File.OpenWrite(filePath);




        //} 

        public IEnumerable<T> ReadAll(string fileName)
        {
            string filePath = Path.Combine(DirPath, fileName);
            using var stream = File.OpenText(filePath);

            return JsonSerializer.Deserialize<T[]>(stream.ReadToEnd());
        }

        public async Task<IEnumerable<T>> ReadAllAsync(string fileName)
        {
            string filePath = Path.Combine(DirPath, fileName);
            using var stream = File.Open(filePath, FileMode.Open);

            return await JsonSerializer.DeserializeAsync<T[]>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
