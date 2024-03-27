using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Implementations;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.UI.Models;
using SushmaElectrical.ViewModels;
using SushmaElectrical.ViewModels.Utility;
using System.Diagnostics;
using System.Drawing;

namespace SushmaElectrical.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IProductRepo _productRepo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICategoryRepo categoryRepo, IProductRepo productRepo, ILogger<HomeController> logger)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _logger = logger;
        }




        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                var products = await _productRepo.GetProducts(pageNumber, pageSize);
                var totalItems = await _productRepo.GetTotalProductCount();
             

                var productViewModels = products.Select(product => new HomeViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    CategoryName = product.Category?.CategoryName ?? "Uncategorized",
                    ProductImages = product.ImagesUrl,
                    Price = product.NetPrice,
                    Discount = product.Discount
                }).ToList();

                var pagedViewModel = new PagedHomeViewModel
                {
                    Home = productViewModels,
                    PageInfo = new PageInfo
                    {
                        PageNo = pageNumber,
                        PageSize = pageSize,
                        TotalItems = totalItems
                    }
                };

                return View(pagedViewModel);
            }
            catch (Exception ex)
            {
                // Log the error
                return BadRequest("An error occurred while fetching products.");
            }
        }


        public IActionResult About()
        {
            return View();
        }






        public async Task<IActionResult> ProductDetails(int id)
        {
            try
            {
                var product = await _productRepo.GetById(id);
                if (product == null)
                {
                    // Set a message to display an alert in the view
                    TempData["error"] = "Product not found.";
                    return NotFound();
                }
                var productDetails = new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductImages = product.ImagesUrl,

                    CategoryName = product.Category.CategoryName,
                    Description = product.Description,
                    Brand = product.Brand,
                    ModelNumber = product.ModelNumber,
                    PowerRating = product.PowerRating,
                    Voltage = product.Voltage,
                    NetPrice = product.NetPrice,
                    Discount = product.Discount,
                };
                if (product == null)
                {
                    return NotFound();
                }
                return View(productDetails);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while loading the product details page.");

                // Set an error message to display an alert in the view
                TempData["error"] = "An error occurred while loading the product details page.";

                // Optionally, handle or rethrow the exception
                throw;
            }

        }





        public IActionResult Privacy()
        {
            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
