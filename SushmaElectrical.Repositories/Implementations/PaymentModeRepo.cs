using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SushmaElectrical.Entities;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class PaymentModeRepo:IPaymentModeRepo
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly ILogger<PaymentModeRepo> _logger;





        public PaymentModeRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, ILogger<PaymentModeRepo> logger)
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
                var principal = _httpContextAccessor.HttpContext.User;
                var userId = _userManager.GetUserId(principal);
                return userId;
            }
            catch (Exception)
            {

                throw;
            }
           
        }




        public async Task<List<PaymentMode>> GetAllPaymentModes()
        {
            try
            {
                var paymentModes = await _context.PaymentModes.ToListAsync();
                return paymentModes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all payment modes.");
                throw;
            }
        }






        public async Task<bool> SetPaymentMode(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid payment mode ID", nameof(id));
                }

                var customerId = GetUserId();
                if (customerId != null)
                {
                    var paymentMode = await _context.PaymentModes.FindAsync(id);
                    if (paymentMode != null)
                    {
                        await _context.SaveChangesAsync();
                        return true; 
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid payment mode ID");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting payment mode.");
                throw;
            }
        }





        public async Task<PaymentMode> GetPaymentModeById(int paymentModeId)
        {
            try
            {
                return await _context.PaymentModes.FindAsync(paymentModeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching payment mode by ID.");
                throw;
            }
        }

    }
}
