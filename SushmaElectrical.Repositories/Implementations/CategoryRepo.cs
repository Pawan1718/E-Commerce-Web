using Microsoft.EntityFrameworkCore;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class CategoryRepo:ICategoryRepo
    {



        private readonly ApplicationDbContext _context;



        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            try
            {
                return await _context.Categories.ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<Category> GetById(int id)
        {
            try
            {
                if (id ==null)
                {
                    throw new Exception("Invalid Id");
                }
                return await _context.Categories.FindAsync(id);

            }
            catch (Exception)
            {

                throw;
            }
        }




        public async Task Save(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
           
        }




        public async Task Edit(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
          
        }




        public async Task Delete(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
           
        }




        public async Task<bool> IsCategoryExist(string categoryName)
        {
            try
            {
                var category= await _context.Categories.AnyAsync(x=>x.CategoryName==categoryName);
                return category;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
