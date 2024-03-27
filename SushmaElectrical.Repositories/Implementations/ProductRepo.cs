using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels.Utility;
using SushmaElectrical.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepo> _logger;


        public ProductRepo(ApplicationDbContext context, ILogger<ProductRepo> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .OrderBy(p => p.Id) // Or any other sorting criteria you prefer
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products by page.");
                throw new Exception("Error occurred while fetching products. Please try again later.");
            }
        }

        




        public async Task<Product> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid product ID", nameof(id));
                }

                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid product ID");
         
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product by ID.");
                throw;
            }
        }






        public async Task Save(Product product)
        {
            try
            {
                // Validate input
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product cannot be null.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "A product with the same name, power rating, or voltage already exists.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the product.");
                throw;
            }
        }





        public async Task Edit(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product in the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the product.");
                throw;
            }
        }





        public async Task Delete(Product product)
        {
            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the product.");
                throw;
            }
        }

        public async Task<bool> ProductExists(string productName, string powerRating, string brand, string voltage)
        {
            try
            {
                // Assuming _context is your DbContext instance
                var existingProduct = await _context.Products
                    .AnyAsync(p =>
                        p.ProductName == productName &&
                        p.PowerRating == powerRating &&
                        p.Brand == brand &&
                        p.Voltage == voltage);

                return existingProduct;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while checking if the product exists by attributes.");
                // You might want to throw the exception or handle it differently based on your application's requirements
                throw;
            }
        }

        public async Task<int> GetTotalProductCount()
        {
            try
            {
                var totalCount = await _context.Products.CountAsync();
                return totalCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the total count of products.");
                throw; 
            }
        }

    }
}
