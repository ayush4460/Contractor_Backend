using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor_Backend.Infrastructure.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var fullPath = Path.Combine(_basePath, fileName);
            using var file = File.Create(fullPath);
            await fileStream.CopyToAsync(file);
            return fullPath;
        }

        public Task DeleteAsync(string filePath)
        {
            File.Delete(filePath);
            return Task.CompletedTask;
        }
    }
}
