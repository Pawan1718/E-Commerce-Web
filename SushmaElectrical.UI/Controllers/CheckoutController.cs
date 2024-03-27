using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using SushmaElectrical.Extensions;


namespace SushmaElectrical.UI.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartRepo _cartRepo;
        private readonly IShippingRepo _shippingRepo;
        private readonly IPaymentModeRepo _paymentModeRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger<CheckoutController> _logger;



        public CheckoutController(ICartRepo cartRepo, IShippingRepo shippingRepo,
            IOrderRepo orderRepo, IPaymentModeRepo paymentModeRepo, ILogger<CheckoutController> logger)
        {
            _cartRepo = cartRepo;
            _shippingRepo = shippingRepo;
            _orderRepo = orderRepo;
            _paymentModeRepo = paymentModeRepo;
            _logger = logger;
        }



        public async Task<IActionResult> GetDeliveryAddress4Order(List<int> selectedItems)
        {
            try
            {
                if (selectedItems == null || !selectedItems.Any())
                {
                    TempData["error"] = "Please select items before proceeding.";
                    return RedirectToAction("Index", "Home");
                }

                // Store the selected items in session
                HttpContext.Session.SetObject("SelectedItems", selectedItems);

                // Retrieve delivery addresses from the repository
                var deliveryAddresses = await _shippingRepo.GetAll();

                var vm = deliveryAddresses.Select(address => new GetDeliveryAddressViewModel
                {
                    Id = address.Id,
                    FullName = address.FullName,
                    Mobile = address.Mobile,
                    AreaStreet = address.AreaStreet,
                    FlatHouseNo = address.FlatHouseNo,
                    Landmark = address.Landmark,
                    TownCity = address.TownCity,
                    Village = address.Village,
                    Country = address.Country,
                    State = address.State,
                    PinCode = address.PinCode
                }).ToList();

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching delivery addresses.");
                TempData["error"] = "An error occurred while fetching delivery addresses.";
                return RedirectToAction("Index", "Home");
            }
        }



        public async Task<IActionResult> GetPaymentMode(int selectedShippingAddress)
        {
            try
            {
                if (selectedShippingAddress <= 0)
                {
                    TempData["error"] = "Please select a shipping address before proceeding.";
                    return RedirectToAction("Index", "Home");
                }

                // Store the selected shipping address in session
                HttpContext.Session.SetObject("SelectedShippingAddress", selectedShippingAddress);

                // Retrieve payment modes from the repository
                var paymentModes = await _paymentModeRepo.GetAllPaymentModes();
                var vm = paymentModes.Select(mode => new PaymentModeViewModel
                {
                    Id = mode.Id,
                    PaymentModeTitle = mode.PaymentModeTitle
                }).ToList();

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching payment modes.");
                TempData["error"] = "An error occurred while fetching payment modes.";
                return RedirectToAction("Index", "Home");
            }
        }




        public async Task<IActionResult> CheckoutSummary(int paymentModeCOD)
        {
            try
            {
                if (paymentModeCOD <= 0)
                {
                    TempData["error"] = "Please select a Payment mode before proceeding.";
                    return RedirectToAction("Index", "Home");
                }

                // Store the selected shipping address in session
                HttpContext.Session.SetObject("SelectedCOD", paymentModeCOD);

                var selectedItemsIds = HttpContext.Session.GetObject<List<int>>("SelectedItems");
                var selectedShippingAddressId = HttpContext.Session.GetObject<int>("SelectedShippingAddress");
                var selectedCOD = HttpContext.Session.GetObject<int>("SelectedCOD");

                if (selectedShippingAddressId == null)
                {
                    TempData["error"] = "No shipping address selected.";
                    return RedirectToAction("Index", "Home");
                }

                var selectedItems = new List<CartDetailsViewModel>();
                int cartId = 0; // Initialize cartId

                foreach (var itemId in selectedItemsIds)
                {
                    var (cartDetail, product) = await _cartRepo.GetProductDetailsByCartDetailId(itemId);

                    if (cartDetail != null && product != null)
                    {
                        var cartDetailsViewModel = new CartDetailsViewModel
                        {
                            CartDetailId = cartDetail.Id,
                            ProductName = product.ProductName,
                            Quantity = cartDetail.Quantity,
                            Price = cartDetail.UnitPrice,
                            ProductImage = product.ImagesUrl,
                            Description = product.Description,
                            CartId = cartDetail.ShoppingCartId
                        };

                        selectedItems.Add(cartDetailsViewModel);

                        // Set cartId if not set already
                        if (cartId == 0)
                        {
                            cartId = cartDetail.ShoppingCartId;
                        }
                    }
                }

                var shippingAddress = await _shippingRepo.GetById(selectedShippingAddressId);

                if (shippingAddress == null)
                {
                    TempData["error"] = "Selected shipping address not found.";
                    return RedirectToAction("Index", "Home");
                }

                var paymentMode = await _paymentModeRepo.GetPaymentModeById(selectedCOD);
                if (paymentMode == null)
                {
                    TempData["error"] = "Selected payment mode not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Create a CheckoutSummaryViewModel instance
                var viewModel = new CheckoutSummaryViewModel
                {
                    SelectedPaymentMode = new PaymentModeViewModel
                    {
                        Id = selectedCOD,
                        PaymentModeTitle = paymentMode.PaymentModeTitle
                    },

                    SelectedItems = new ShoppingCartViewModel
                    {
                        CartId = cartId, // Assign the obtained cartId
                        cartDetails = selectedItems
                    },

                    ShippingAddress = new ShippingViewModel
                    {
                        Id = shippingAddress.Id,
                        DefaultAddress = shippingAddress.DefaultAddress,
                        FullName = shippingAddress.FullName,
                        Mobile = shippingAddress.Mobile,
                        FlatHouseNo = shippingAddress.FlatHouseNo,
                        AreaStreet = shippingAddress.AreaStreet,
                        Landmark = shippingAddress.Landmark,
                        Country = shippingAddress.Country,
                        State = shippingAddress.State,
                        TownCity = shippingAddress.TownCity,
                        Village = shippingAddress.Village,
                        PinCode = shippingAddress.PinCode
                    }
                };

                return View(viewModel); // Return the view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching checkout summary.");
                TempData["error"] = "An error occurred while fetching checkout summary.";
                return RedirectToAction("Index", "Home");
            }
        }





        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int cartId, int deliveryAddressId, int paymentModeId)
        {
            try
            {
                if (cartId <= 0 || deliveryAddressId <= 0 || paymentModeId <= 0)
                {
                    TempData["error"] = "Invalid parameters";
                    return View("GetPaymentMode");
                }

                bool isCheckOut = await _cartRepo.Checkout(deliveryAddressId, paymentModeId, cartId);
                if (!isCheckOut)
                {
                    TempData["error"] = "Failed to place order. Something happened on the server side.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["success"] = "Order placed successfully.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while placing the order.");
                TempData["error"] = "An error occurred while placing the order.";
                return RedirectToAction("Index", "Home");
            }
        }







    }



}
