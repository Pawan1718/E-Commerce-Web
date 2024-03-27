using SushmaElectrical.Entities;
using SushmaElectrical.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize);
        Task<Product> GetById(int Id);
        Task Save(Product product);
        Task Edit(Product product);
        Task Delete(Product product);
        Task<bool> ProductExists(string productName, string powerRating, string brand, string voltage);
        Task<int> GetTotalProductCount();
    }
}
