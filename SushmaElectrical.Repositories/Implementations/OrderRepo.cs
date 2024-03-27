using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SushmaElectrical.Entities;
using SushmaElectrical.Infrastructure.GlobalConfiguration;
using SushmaElectrical.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Implementations
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProductRepo> _logger;


        public OrderRepo(ApplicationDbContext context, UserManager<ApplicationUser>
                           userManager, IHttpContextAccessor httpContextAccessor, ILogger<ProductRepo> logger)
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






        public async Task<Order> GetOrderByCartId(int cartId)
        {
            try
            {
                if (cartId <= 0)
                {
                    throw new ArgumentException("Invalid cartId");
                }

                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == cartId);

                if (order == null)
                {
                    throw new InvalidOperationException($"Order with cartId {cartId} not found");
                }

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }






        public async Task UpdateOrder(Order order)
        {
            try
            {
                if (order == null)
                {
                    throw new ArgumentNullException(nameof(order), "Order object cannot be null");
                }

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating order", ex);
            }
        }






        public async Task<IEnumerable<Order>> GetUserOrder()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }

                var orders = await _context.Orders
                    .Include(x => x.ShippingAddress)
                    .Include(o => o.orderDetails)
                    .ThenInclude(os => os.Product)
                    .ThenInclude(p => p.Category)
                    .Where(u => u.UserId == userId)
                    .ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user orders", ex);
            }
        }








        public async Task<Order> GetOrderById(int orderId)
        {
            try
            {

                if (orderId <= 0)
                {
                    return null; // Invalid orderId
                }

                var getOrder = await _context.Orders
                    .Include(x => x.orderDetails)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Category)
                    .Include(x => x.PaymentMode)
                    .Include(x => x.ShippingAddress)
                    .Include(x => x.Users)
                        .ThenInclude(x => x.ShippingDetails)
                    .FirstOrDefaultAsync(x => x.Id == orderId);

                return getOrder;
            }
            catch (Exception)
            {

                throw;
            }
        }







        public async Task<IEnumerable<Order>> GetAllOrders(int pageNo, int pageSize)
        {
            try
            {
                // Calculate the number of items to skip based on the page size and page number
                return await _context.Orders
                    .Include(x => x.ShippingAddress)
                    .Include(o => o.orderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Category)
                     .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)  // Take only the items for the current page
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders by page.");
                throw; // Rethrow the exception to propagate it to the caller
            }
        }








        public async Task<bool> CancelOrder(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Find the order
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                // Find order details
                var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

                // Restore product quantities
                foreach (var orderDetail in orderDetails)
                {
                    var product = await _context.Products.FindAsync(orderDetail.ProductId);
                    if (product != null)
                    {
                        product.Quantity += orderDetail.Quantity;
                    }
                    else
                    {
                        throw new Exception($"Product with ID {orderDetail.ProductId} not found");
                    }
                }

                order.OrderStatus = (int)Infrastructure.GlobalConfiguration.OrderStatus.Cancelled;

              //  _context.OrderDetails.RemoveRange(orderDetails);

                // Remove the order itself
             //   _context.Orders.Remove(order);

                // Commit transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                // Rollback transaction if there's an exception
                await transaction.RollbackAsync();
                return false;
            }
        }







        public async Task<bool> OrderIssue(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Find the order
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                // Find order details
                var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

               

                order.OrderStatus = (int)Infrastructure.GlobalConfiguration.OrderStatus.Shipped;

                //  _context.OrderDetails.RemoveRange(orderDetails);

                // Remove the order itself
                //   _context.Orders.Remove(order);

                // Commit transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                // Rollback transaction if there's an exception
                await transaction.RollbackAsync();
                return false;
            }
        }





        public async Task<int> GetTotalCustomerOrderCount()
        {
            try
            {
                var totalCount = await _context.Orders.CountAsync();
                return totalCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the total count of products.");
                throw;
            }
        }
    }
}
