using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class ShippingRepo : IShippingRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ShippingRepo> _logger;


        public ShippingRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor, ILogger<ShippingRepo> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }





        private string GetUserId()
        {
            try
            {
                if (_httpContextAccessor == null || _userManager == null)
                {
                    return null;
                }

                var principal = _httpContextAccessor.HttpContext.User;
                if (principal == null)
                {
                    return null;
                }

                return _userManager.GetUserId(principal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user ID.");

                return null;
            }
        }






        public async Task Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return;
                }

                var shipping = await _context.ShippingDetails.FindAsync(id);
                if (shipping != null)
                {
                    _context.ShippingDetails.Remove(shipping);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception( "Shipping details not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting shipping details.");

                throw new Exception("An error occurred while deleting shipping details.");
            }
        }






        public async Task Edit(ShippingDetails shipping)
        {
            try
            {
                if (shipping == null)
                {
                    throw new ArgumentNullException(nameof(shipping), "Shipping details cannot be null.");
                }

                _context.ShippingDetails.Update(shipping);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the shipping details in the database.");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the shipping details.");

                throw;
            }
        }






        public async Task<IEnumerable<ShippingDetails>> GetAll()
        {
            try
            {
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User not logged-in");
                }

                return await _context.ShippingDetails.Where(sd => sd.CustomerId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving shipping details for the user.");

                throw;
            }
        }






        public async Task<ShippingDetails> GetById(int id)
        {
            try
            {
                if (id<=0)
                {
                    throw new Exception("Invalid id");
                }
                return await _context.ShippingDetails.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving shipping details with ID {id}.");

                throw;
            }
        }






        public async Task Save(ShippingDetails shipping)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User not logged in.");
                }

                shipping.CustomerId = userId;
                _context.ShippingDetails.Add(shipping);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving shipping details.");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving shipping details.");

                throw;
            }
        }







        public async Task SetDefaultShippingAddress(ShippingDetails shippingDetails)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User not logged in.");
                }

                var userShippingDetails = await _context.ShippingDetails
                    .Where(sd => sd.CustomerId == userId)
                    .ToListAsync();

                foreach (var userShippingDetail in userShippingDetails)
                {
                    userShippingDetail.DefaultAddress = (userShippingDetail.Id == shippingDetails.Id);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting default shipping address.");

                throw;
            }
        }



    }
}
