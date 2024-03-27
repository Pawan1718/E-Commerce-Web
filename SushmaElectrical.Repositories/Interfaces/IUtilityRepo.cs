using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface IUtilityRepo
    {
       // Task<string> SaveImage(string containerName, IFormFile file);
        Task<List<string>> SaveImages(string containerName, IFormFileCollection files);
        Task<List<string>> EditImage(string containerName, IFormFileCollection files, string dbPath);
        Task<string> DeleteImage(string containerName, string dbPath);

    }
}
