using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SushmaElectrical.UI.Controllers
{
    [Authorize]
    public class ShippingController : Controller
    {
        private readonly IShippingRepo _shippingRepo;
        private readonly ILogger<ShippingController> _logger;

        public ShippingController(IShippingRepo shippingRepo, ILogger<ShippingController> logger)
        {
            _shippingRepo = shippingRepo;
            _logger = logger;
        }






        public async Task<IActionResult> GetAllShippingDetails()
        {
            try
            {
                var shippingDetails = await _shippingRepo.GetAll();

                if (shippingDetails != null && shippingDetails.Any())
                {
                    var viewModels = shippingDetails.Select(sd => new ShippingViewModel
                    {
                        Id = sd.Id,
                        FullName = sd.FullName,
                        Mobile = sd.Mobile,
                        FlatHouseNo = sd.FlatHouseNo,
                        Country = sd.Country,
                        State = sd.State,
                        TownCity = sd.TownCity,
                        Village = sd.Village,
                        PinCode = sd.PinCode,
                        AreaStreet = sd.AreaStreet,
                        Landmark = sd.Landmark,
                        DefaultAddress = false
                    });

                    return View(viewModels);
                }
                else
                {
                    return View(new List<ShippingViewModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving shipping details");
                return View("Error");
            }
        }






        [HttpPost]
        public async Task<IActionResult> SetDefault(int id)
        {
            try
            {
                var shippingDetails = await _shippingRepo.GetById(id);

                if (shippingDetails == null)
                {
                    return RedirectToAction("Error");
                }
                await _shippingRepo.SetDefaultShippingAddress(shippingDetails);

                return RedirectToAction("GetAllShippingDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting default shipping address");
                return RedirectToAction("Error");
            }
        }





        public async Task<IActionResult> AddShippingDetails()
        {

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShippingDetails(AddShippingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var shippingDetails = new ShippingDetails
                    {
                        FullName = model.FullName,
                        Mobile = model.Mobile,
                        FlatHouseNo = model.FlatHouseNo,
                        Country = model.Country,
                        State = model.State,
                        TownCity = model.TownCity,
                        Village = model.Village,
                        PinCode = model.PinCode,
                        AreaStreet = model.AreaStreet,
                        Landmark = model.Landmark,
                    };
                    await _shippingRepo.Save(shippingDetails);
                    TempData["success"] = "Shipping address saved successfully !";

                    return RedirectToAction("GetAllShippingDetails");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding shipping details");
                    ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                    return View(model);
                }
            }
            return View(model);
        }







        public async Task<IActionResult> EditShippingDetails(int id)
        {
            try
            {
                var shippingDetails = await _shippingRepo.GetById(id);

                if (shippingDetails == null)
                {
                    return RedirectToAction("Error");
                }
                var vm = new ShippingViewModel
                {
                    FullName = shippingDetails.FullName,
                    Mobile = shippingDetails.Mobile,
                    FlatHouseNo = shippingDetails.FlatHouseNo,
                    Country = shippingDetails.Country,
                    State = shippingDetails.State,
                    TownCity = shippingDetails.TownCity,
                    Village = shippingDetails.Village,
                    PinCode = shippingDetails.PinCode,
                    Id = shippingDetails.Id,
                    AreaStreet = shippingDetails.AreaStreet,
                    Landmark = shippingDetails.Landmark
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing shipping details");
                return RedirectToAction("Error");
            }
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShippingDetails(int cartId, ShippingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var shippingDetails = await _shippingRepo.GetById(model.Id);

                    if (shippingDetails == null)
                    {
                        return RedirectToAction("Error");
                    }

                    shippingDetails.FullName = model.FullName;
                    shippingDetails.Mobile = model.Mobile;
                    shippingDetails.FlatHouseNo = model.FlatHouseNo;
                    shippingDetails.Country = model.Country;
                    shippingDetails.State = model.State;
                    shippingDetails.TownCity = model.TownCity;
                    shippingDetails.Village = model.Village;
                    shippingDetails.PinCode = model.PinCode;
                    shippingDetails.AreaStreet = model.AreaStreet;
                    shippingDetails.Landmark = model.Landmark;
                    shippingDetails.Id = model.Id;

                    await _shippingRepo.Edit(shippingDetails);


                    return RedirectToAction("GetAllShippingDetails");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing shipping details");
                return RedirectToAction("Error");
            }

        }






        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var shipping = await _shippingRepo.GetById(id);
                if (shipping == null)
                {
                    return NotFound();
                }

                var shippingViewModel = new ShippingViewModel
                {
                    Id = shipping.Id,
                    FullName = shipping.FullName,
                    Mobile = shipping.Mobile,
                    FlatHouseNo = shipping.FlatHouseNo,
                    Country = shipping.Country,
                    State = shipping.State,
                    TownCity = shipping.TownCity,
                    Village = shipping.Village,
                    PinCode = shipping.PinCode,
                    AreaStreet = shipping.AreaStreet,
                    Landmark = shipping.Landmark
                };

                return View(shippingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the shipping details for deletion");
                return RedirectToAction("Error");
            }
        }






        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var shipping = await _shippingRepo.GetById(id);
                if (shipping == null)
                {
                    return NotFound(); 
                }

                await _shippingRepo.Delete(id);
                return RedirectToAction("GetAllShippingDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the shipping details");
                return RedirectToAction("Error");
            }
        }




       

    }
}
