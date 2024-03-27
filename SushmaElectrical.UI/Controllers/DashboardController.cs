using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushmaElectrical.Entities;
using SushmaElectrical.Infrastructure.GlobalConfiguration;
using SushmaElectrical.Repositories.Implementations;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels;
using SushmaElectrical.ViewModels.DashboardViewModel;
using SushmaElectrical.ViewModels.Utility;
using System.Drawing.Printing;
using static NuGet.Packaging.PackagingConstants;
using static SushmaElectrical.ViewModels.DashboardViewModel.CustomerOrdersViewModel;

namespace SushmaElectrical.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IShippingRepo _shippingRepo;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IOrderRepo orderRepo, ILogger<DashboardController> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }





        public async Task<IActionResult> GetUserOrders(int pageNo = 1, int pageSize = 9)
        {
            try
            {
                var orders = await _orderRepo.GetAllOrders(pageNo, pageSize);
                var totalItems = await _orderRepo.GetTotalCustomerOrderCount();

                var vm = new List<CustomerOrdersViewModel>();

                foreach (var order in orders)
                {
                    var orderViewModel = new CustomerOrdersViewModel
                    {
                        Id = order.Id,
                        ProductName = order.orderDetails
                                        .Where(od => od.Product != null)
                                        .Select(od => od.Product.ProductName)
                                        .FirstOrDefault() ?? "Unknown",
                      
                        Quantity = order.orderDetails.Sum(od => od.Quantity),
                        OrderStatus = order.OrderStatus,
                        OrderDate=order.CreatedDate,
                        FullName = order.ShippingAddress?.FullName,
                       
                    };

                    vm.Add(orderViewModel);
                }

                var pageInfo = new PageInfo
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                var pagedViewModel = new PagedCustomerViewModel
                {
                    CustomerOrders = vm,
                    PageInfo = pageInfo
                };

                return View(pagedViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user orders.");
                TempData["error"] = "An error occurred while retrieving user orders.";
                return RedirectToAction("Index");
            }
        }






        public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                var order = await _orderRepo.GetOrderById(orderId);

                if (order == null)
                {
                    TempData["warning"] = "No orders found.";

                    return RedirectToAction("Index");
                }

                var orderViewModel = new OrderViewModel
                {
                    Id = order.Id,
                    ProductName = order.orderDetails.FirstOrDefault()?.Product.ProductName,
                    ProductImage = order.orderDetails.FirstOrDefault()?.Product.ImagesUrl,
                    GrossPrice = order.orderDetails.Sum(od => od.UnitPrice * od.Quantity),
                    Quantity = order.orderDetails.Sum(od => od.Quantity),
                    OrderStatus = order.OrderStatus,
                    OrderDetails = order.orderDetails.ToList(),
                };

                if (order.ShippingAddress != null)
                {
                    orderViewModel.FullName = order.ShippingAddress.FullName;
                    orderViewModel.Mobile = order.ShippingAddress.Mobile;
                    orderViewModel.FlatHouseNo = order.ShippingAddress.FlatHouseNo;
                    orderViewModel.AreaStreet = order.ShippingAddress.AreaStreet;
                    orderViewModel.Landmark = order.ShippingAddress.Landmark;
                    orderViewModel.Country = order.ShippingAddress.Country;
                    orderViewModel.State = order.ShippingAddress.State;
                    orderViewModel.TownCity = order.ShippingAddress.TownCity;
                    orderViewModel.Village = order.ShippingAddress.Village;
                    orderViewModel.PinCode = order.ShippingAddress.PinCode;
                }

                var orderViewModels = new List<OrderViewModel> { orderViewModel };

                return View(orderViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user orders.");

                TempData["error"] = "An error occurred while retrieving user orders.";

                return RedirectToAction("Index");
            }
        }





    }
}
