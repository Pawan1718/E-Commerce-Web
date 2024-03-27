using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class UtilityRepo : IUtilityRepo
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ShippingRepo> _logger;

        public UtilityRepo(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor,
            ILogger<ShippingRepo> logger)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }



        public async Task<string> DeleteImage(string ContainerName, string dbPath)
        {
            try
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    return null;
                }

                var fileName = Path.GetFileName(dbPath);
                var completePath = Path.Combine(_environment.WebRootPath, ContainerName, fileName);
                if (File.Exists(completePath))
                {
                    File.Delete(completePath);
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while deleting the image file.");

                // Optionally handle or rethrow the exception
                throw;
            }
        }






        public async Task<List<string>> EditImage(string ContainerName, IFormFileCollection files, string dbPath)
        {
            try
            {
                await DeleteImage(ContainerName, dbPath);

                List<string> newImagePaths = await SaveImages(ContainerName, files);

                return newImagePaths;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the image file.");
                throw;
            }
           
        }






        public async Task<List<string>> SaveImages(string ContainerName, IFormFileCollection files)
        {
            try
            {
                List<string> imagePaths = new List<string>();

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    string folder = Path.Combine(_environment.WebRootPath, ContainerName);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, fileName);

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            await memoryStream.CopyToAsync(fileStream);
                        }
                    }

                    var basePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                    var completePath = Path.Combine(basePath, ContainerName, fileName).Replace("\\", "/");

                    imagePaths.Add(completePath); // Add the image path to the list
                }

                return imagePaths;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the image file.");
                throw;
            }
          
        }
    }
}