using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor_Backend.Infrastructure.Storage
{
    public interface IStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);
        Task DeleteAsync(string filePath);
    }
}
