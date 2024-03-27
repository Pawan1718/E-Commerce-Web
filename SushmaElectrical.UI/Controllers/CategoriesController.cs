
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Implementations;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels;
using SushmaElectrical.ViewModels.Utility;
using System.Threading.Tasks;
using static SushmaElectrical.ViewModels.CategoryViewModel;

namespace SushmaElectrical.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;


        private readonly IOrderRepo _orderRepo;


        private readonly ILogger<CategoriesController> _logger;




        public CategoriesController(ICategoryRepo categoryRepo, IOrderRepo orderRepo, ILogger<CategoriesController> logger)
        {
            _categoryRepo = categoryRepo;
            _orderRepo = orderRepo;
            _logger = logger;
        }






        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10)
        {
            try
            {
                // Validate page number and page size
                if (pageNo < 1 || pageSize < 1)
                {
                    ModelState.AddModelError("", "Invalid page number or page size.");
                    return View();
                }

                var categories = await _categoryRepo.GetAllCategory();
                int totalItems = categories.Count();
                var ViewModels = categories.Skip((pageNo - 1) * pageSize)
                                           .Take(pageSize)
                                           .Select(Category => new CategoryViewModel
                                           {
                                               Id = Category.Id,
                                               Description = Category.Description,
                                               CategoryName = Category.CategoryName,
                                           })
                                           .ToList();

                var pagedViewModel = new PagedCategoryViewModel
                {
                    Categories = ViewModels,
                    PageInfo = new PageInfo
                    {
                        PageNo = pageNo,
                        PageSize = pageSize,
                        TotalItems = totalItems
                    }
                };

                return View(pagedViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");

                _logger.LogError(ex, "An error occurred in Index action.");
                return View();
            }
        }




        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isCategoryExist = await _categoryRepo.IsCategoryExist(categoryViewModel.CategoryName);
                    if (isCategoryExist)
                    {
                        ModelState.AddModelError("", "Category with the same name already exists.");
                        return View(categoryViewModel);
                    }
                    var categoryEntity = new Category
                    {
                        CategoryName = categoryViewModel.CategoryName,
                        Description = categoryViewModel.Description
                    };

                    await _categoryRepo.Save(categoryEntity);

                    TempData["success"] = "Category is Saved Successfully";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error occurred while saving category: {ex.Message}");
                }
            }

            return View(categoryViewModel);
        }





        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var categoryEntity = await _categoryRepo.GetById(id);
                if (categoryEntity == null)
                {
                    return NotFound();
                }

                var categoryViewModel = new CategoryViewModel
                {
                    Id = categoryEntity.Id,
                    CategoryName = categoryEntity.CategoryName,
                    Description = categoryEntity.Description
                    // Map other properties as needed
                };
                return View(categoryViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while fetching category for editing.");
                // Optionally, add an alert message
                TempData["error"] = "An error occurred while fetching category for editing.";
                return RedirectToAction(nameof(Index));
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel categoryViewModel)
        {
            try
            {
                if (id != categoryViewModel.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var categoryEntity = new Category
                    {
                        Id = categoryViewModel.Id,
                        CategoryName = categoryViewModel.CategoryName,
                        Description = categoryViewModel.Description
                        // Map other properties as needed
                    };

                    await _categoryRepo.Edit(categoryEntity);

                    TempData["success"] = "Category updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                return View(categoryViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while updating category.");
                // Optionally, add an alert message
                TempData["error"] = "An error occurred while updating category.";
                return View(categoryViewModel);
            }
        }






        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryRepo.GetById(id);
                if (category == null)
                {
                    return NotFound();
                }

                var categoryViewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Description = category.Description
                };

                return View(categoryViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while fetching category for deletion.");
                // Optionally, add an alert message
                TempData["error"] = "An error occurred while fetching category for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }





        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _categoryRepo.GetById(id);
                if (category == null)
                {
                    return NotFound();
                }

                await _categoryRepo.Delete(category);

                TempData["success"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while deleting category.");
                // Optionally, add an alert message
                TempData["error"] = "An error occurred while deleting category.";
                return RedirectToAction(nameof(Index));
            }
        }



    }
}
