using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Implementations;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.UI.DDL;
using SushmaElectrical.ViewModels;
using SushmaElectrical.ViewModels.Utility;
using static SushmaElectrical.ViewModels.CategoryViewModel;

namespace SushmaElectrical.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IProductRepo productRepo;
        private readonly IOrderRepo orderRepo;
        private readonly IUtilityRepo utilityRepo;
        private readonly ILogger<ProductsController> _logger;

        private readonly DDLHelpers ddl;
        private string ImageFolder = "ProductImages";




        public ProductsController(ICategoryRepo categoryRepo, IProductRepo productRepo,
            IUtilityRepo utilityRepo, DDLHelpers ddl, IOrderRepo orderRepo, ILogger<ProductsController> logger)
        {
            this.categoryRepo = categoryRepo;
            this.productRepo = productRepo;
            this.utilityRepo = utilityRepo;
            this.ddl = ddl;
            this.orderRepo = orderRepo;
            _logger = logger;
        }









        public async Task<IActionResult> Index(int PageNo = 1, int pageSize = 10)

        {
            try
            {
                var products = await productRepo.GetProducts(PageNo, pageSize);

                var productViewModels = products
                    .Select(product => new ProductViewModel
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        CategoryName = product.Category.CategoryName,
                        Brand = product.Brand,
                        PowerRating = product.PowerRating,
                        ModelNumber = product.ModelNumber,
                        Voltage = product.Voltage,
                        NetPrice = product.NetPrice,
                        Quantity = product.Quantity
                    })
                    .ToList();

                var totalItems = await productRepo.GetTotalProductCount();

                var pagedViewModel = new PagedProductViewModel
                {
                    Products = productViewModels,
                    PageInfo = new PageInfo
                    {
                        PageNo = PageNo,
                        PageSize = pageSize,
                        TotalItems = totalItems
                    }
                };

                return View(pagedViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                return RedirectToAction("Error", "Home");
            }
        }








        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await productRepo.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }

                var productDetails = new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductImages = product.ImagesUrl,
                    CategoryName = product.Category?.CategoryName,
                    Description = product.Description,
                    Brand = product.Brand,
                    ModelNumber = product.ModelNumber,
                    PowerRating = product.PowerRating,
                    Voltage = product.Voltage,
                    NetPrice = product.NetPrice,
                    Discount = product.Discount,
                    Tax = product.GST,
                    GrossPrice = product.GrossPrice,
                    Quantity = product.Quantity
                };

                return View(productDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product details.");
                TempData["ErrorMessage"] = "An error occurred while retrieving product details. Please try again later.";
                return RedirectToAction("Error", "Home");
            }
        }





        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var categoryList = await ddl.GetCategoryList();
                ViewBag.CategoryList = categoryList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving category list for product creation.");
                TempData["ErrorMessage"] = "An error occurred while retrieving category list for product creation. Please try again later.";
                return RedirectToAction("Error", "Home");
            }
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsExistingProduct = await productRepo.ProductExists(vm.ProductName, vm.Brand, vm.PowerRating, vm.Voltage);
                    if (IsExistingProduct)
                    {
                        ModelState.AddModelError(string.Empty, "Product with this name already exists.");
                        TempData["ErrorMessage"] = "Product with this name already exists.";
                        var existingCategoryList = await ddl.GetCategoryList();
                        ViewBag.CategoryList = existingCategoryList;
                        return View(vm);
                    }

                    var product = new Product
                    {
                        ProductName = vm.ProductName,
                        Description = vm.Description,
                        CategoryId = vm.CategoryId,
                        Brand = vm.Brand,
                        PowerRating = vm.PowerRating,
                        ModelNumber = vm.ModelNumber,
                        Voltage = vm.Voltage,
                        NetPrice = vm.NetPrice,
                        Discount = vm.Discount,
                        Quantity = vm.Quantity,
                        GST = vm.Tax,
                        GrossPrice = vm.GrossPrice
                    };

                    if (vm.ProductImages != null && vm.ProductImages.Count > 0)
                    {
                        var filesCollection = new FormFileCollection();
                        foreach (var file in vm.ProductImages)
                        {
                            var formFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, file.FileName)
                            {
                                Headers = file.Headers,
                                ContentType = file.ContentType
                            };
                            filesCollection.Add(formFile);
                        }

                        var savedImageUrls = await utilityRepo.SaveImages(ImageFolder, filesCollection);
                        product.ImagesUrl = string.Join(",", savedImageUrls);
                    }

                    await productRepo.Save(product);
                    TempData["success"] = "Product saved successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Please fix the errors below.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product.");
                TempData["error"] = "An error occurred while creating the product. Please try again later.";
            }

            var categoryList = await ddl.GetCategoryList();
            ViewBag.CategoryList = categoryList;
            return View(vm);
        }







        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await productRepo.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }

                var editproductViewModel = new EditProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    ImageUrl = product.ImagesUrl,
                    PowerRating = product.PowerRating,
                    Brand = product.Brand,
                    ModelNumber = product.ModelNumber,
                    Voltage = product.Voltage,
                    Tax = product.GST,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    NetPrice = product.NetPrice,
                    GrossPrice = product.GrossPrice
                };
                var categoryList = await ddl.GetCategoryList();
                ViewBag.CategoryList = categoryList;

                return View(editproductViewModel);
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while editing the product. Please try again later.";
                _logger.LogError(ex, "An error occurred while editing the product.");
                throw;
            }

        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel vm)
        {
            try
            {
                var product = await productRepo.GetById(vm.Id);
                if (ModelState.IsValid)
                {
                    product.ProductName = vm.ProductName;
                    product.ModelNumber = vm.ModelNumber;
                    product.Brand = vm.Brand;
                    product.PowerRating = vm.PowerRating;
                    product.Description = vm.Description;
                    product.CategoryId = vm.CategoryId;
                    product.NetPrice = vm.NetPrice;
                    product.GST = vm.Tax;
                    product.Quantity = vm.Quantity;
                    product.Discount = vm.Discount;
                    product.GrossPrice = vm.GrossPrice;

                    var filesCollection = new FormFileCollection();
                    if (vm.ChooseImage != null && vm.ChooseImage.Count > 0)
                    {
                        foreach (var file in vm.ChooseImage)
                        {
                            filesCollection.Add(file);
                        }
                        var newImageUrls = await utilityRepo.EditImage(ImageFolder, filesCollection, product.ImagesUrl);
                        product.ImagesUrl = string.Join(",", newImageUrls);
                    }

                    await productRepo.Edit(product);
                    TempData["success"] = "Product updated successfully !";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If model validation fails, reload the view with validation errors and display an alert
                    TempData["error"] = "Please fix the errors below.";
                    var categoryList = await ddl.GetCategoryList();
                    ViewBag.CategoryList = categoryList;
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while editing the product.");
                TempData["error"] = "An error occurred while editing the product. Please try again later.";
                // You may want to throw the exception or handle it differently based on your application's requirements
                throw;
            }
        }






        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = product.Category.CategoryName,
                Brand = product.Brand,
                PowerRating = product.PowerRating,
                ModelNumber = product.ModelNumber,
                Voltage = product.Voltage,
                NetPrice = product.NetPrice,
                Quantity = product.Quantity,
                ProductImages = product.ImagesUrl,
                GrossPrice = product.GrossPrice,
                Description = product.Description,
                Discount = product.Discount,

            };

            return View(productViewModel);
        }







        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ProductViewModel viewModel)
        {
            try
            {
                var product = await productRepo.GetById(viewModel.Id);
                if (product == null)
                {
                    return NotFound();
                }

                await productRepo.Delete(product);
                TempData["success"] = "Product deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                TempData["error"] = "An error occurred while deleting the product. Please try again later.";
                throw;
            }
        }









    }
}
