using Microsoft.AspNetCore.Mvc.Rendering;
using SushmaElectrical.Repositories.Interfaces;

namespace SushmaElectrical.UI.DDL
{
    public class DDLHelpers
    {
        private readonly ICategoryRepo _categoryRepo;

        public DDLHelpers(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<SelectList> GetCategoryList()
        {
            var categories = await _categoryRepo.GetAllCategory();
            return new SelectList(categories, "Id", "CategoryName");
        }


    }
}
