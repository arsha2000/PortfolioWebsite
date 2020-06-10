using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioWebsite.Services
{
    public class FileManager
    {

        private readonly IWebHostEnvironment _env;
        private string DataDirPath {
            get
            {
                return Path.Combine(_env.ContentRootPath, "Data");
            }
        }

        public FileManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public FileStream GetFile(string fileName, FileMode mode = FileMode.OpenOrCreate)
        {
            string filePath = Path.Combine(DataDirPath, fileName);
            return File.Open(filePath, mode);
        }

        public FileStream ReadFile(string fileName)
        {
            string filePath = Path.Combine(DataDirPath, fileName);
            return File.OpenRead(filePath);
        }

        public string ReadFileAsText(string fileName)
        {
            string filePath = Path.Combine(DataDirPath, fileName);
            return File.OpenText(filePath).ReadToEnd();
        }

        public async Task<string> ReadFileAsTextAsync(string fileName)
        {
            string filePath = Path.Combine(DataDirPath, fileName);
            return await File.OpenText(filePath).ReadToEndAsync();
        }
    }
}
