using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;

namespace SushmaElectrical.UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger<HomeController> _logger;

        public OrderController(IOrderRepo orderRepo, ILogger<HomeController> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }



        public async Task<IActionResult> Orders()
        {
            try
            {
                var orders = await _orderRepo.GetUserOrder();
                if (orders == null)
                {
                    TempData["error"] = "No orders found.";
                    return View(new List<OrderViewModel>());
                }

                var orderViewModels = orders.Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    ProductName = order.orderDetails.Select(od => od.Product.ProductName).FirstOrDefault(),
                    ProductImage = order.orderDetails.Select(od => od.Product.ImagesUrl).FirstOrDefault(),
                    GrossPrice = order.orderDetails.Select(od => od.UnitPrice * od.Quantity).Sum(),
                    Quantity = order.orderDetails.Sum(od => od.Quantity),
                    OrderStatus = order.OrderStatus,
                    OrderDate=order.CreatedDate,
                    
                    OrderDetails = order.orderDetails.ToList() // Assigning the list of OrderDetails directly
                }).ToList();

                return View(orderViewModels);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while fetching user orders.");

                // Set an error message to display an alert in the view
                TempData["error"] = "An error occurred while fetching user orders.";

                // Redirect to a different action or return a view with a message
                return RedirectToAction("Error", "Home");
            }
        }





        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    TempData["error"] = "Invalid orderId.";
                    return RedirectToAction("Index", "Home");
                }

                var success = await _orderRepo.CancelOrder(orderId);

                if (!success)
                {
                    TempData["error"] = "Failed to cancel the order.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["success"] = "Order cancelled successfully.";
                return RedirectToAction("Index", "Home"); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cancelling the order.");

                TempData["error"] = "An error occurred while cancelling the order.";

                return RedirectToAction("Error", "Home");
            }
        }






        public async Task<IActionResult> OrderIssue(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    TempData["error"] = "Invalid orderId.";
                    return RedirectToAction("Index", "Home");
                }

                var success = await _orderRepo.OrderIssue(orderId);

                if (!success)
                {
                    TempData["error"] = "Failed to report issue for the order.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["success"] = "Order issue reported successfully.";
                return RedirectToAction("Index", "Home"); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reporting issue for the order.");

                TempData["error"] = "An error occurred while reporting issue for the order.";

                return RedirectToAction("Error", "Home");
            }
        }


    }
}
