using SushmaElectrical.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<Category> GetById(int Id);
        Task Save(Category category);
        Task Edit(Category category);
        Task Delete(Category category);
        Task<bool> IsCategoryExist(string categoryName);
    }
}
