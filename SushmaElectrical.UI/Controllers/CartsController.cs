using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SushmaElectrical.Extensions;
using SushmaElectrical.Repositories.Implementations;

namespace SushmaElectrical.UI.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ICartRepo _cartRepo;



        private readonly IProductRepo _productRepo;

        private readonly IHttpContextAccessor _httpContextAccessor;



        public CartsController(ICartRepo cartRepo, IProductRepo productRepo, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _httpContextAccessor = httpContextAccessor;
        }





        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            try
            {
                var cartCount = await _cartRepo.AddItem(productId, qty);
                if (redirect != 0)
                {
                    return RedirectToAction("GetUserCart");
                }
                return Ok(new { cartCount });

            }
            catch (Exception)
            {

                throw;
            }
        }





        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    throw new Exception("Invalid id");
                }
                var cartCount = await _cartRepo.RemoveItem(productId);

                // TempData["success"] = "Item removed from cart.";

                return RedirectToAction("GetUserCart");
            }
            catch (Exception)
            {

                throw;
            }

        }






        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                // Retrieve the cart from the repository
                var cartData = await _cartRepo.GetUserCart();


                // Pass the populated cartViewModel to the view
                return View(cartData);
            }
            catch (Exception ex)
            {
                // Handle exceptions more gracefully
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to redirect to an error page or show an error message
                return RedirectToAction("Index", "Home");
            }
        }



      



        public async Task<IActionResult> GetTotalItemInCart()
        {
            try
            {
                int cartItem = await _cartRepo.GetCartItemCount();
                return Ok(cartItem);
            }
            catch (Exception)
            {

                throw;
            }

        }






    }
}
