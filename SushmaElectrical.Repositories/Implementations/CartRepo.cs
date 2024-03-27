using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class CartRepo : ICartRepo
    {



        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;




        public CartRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(CartDetails CartDetail, Product Product)> GetProductDetailsByCartDetailId(int cartDetailId)
        {
            
            var cartDetailAndProduct = await _context.CartDetails
        .Include(cd => cd.Product)
        .FirstOrDefaultAsync(cd => cd.Id == cartDetailId);

            return (cartDetailAndProduct, cartDetailAndProduct?.Product);

        }



        public async Task<int> AddItem(int productId, int Quantity)
        {
            string userId = await GetUserId();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not  logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.Carts.Add(cart);
                }
                await _context.SaveChangesAsync();


                var cartItem = await _context.CartDetails
                                 .Include(p => p.Product)
                                 .FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);

                if (cartItem is not null)
                {
                    cartItem.Quantity += Quantity;
                }
                else
                {
                    var product=await _context.Products.FindAsync(productId);
                    cartItem = new CartDetails
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = Quantity,
                        UnitPrice = product.NetPrice
                    };
                    await _context.CartDetails.AddAsync(cartItem);
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var totalCartItemCount = await GetCartItemCount(userId);
            return totalCartItemCount;
        }






        public async Task<int> RemoveItem(int productId)
        {
            string userId = await GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user not logged-in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Cart is Empty");
                }
                var cartItem = await _context.CartDetails
                    .FirstOrDefaultAsync(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                {
                    throw new Exception("No item in cart");
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {


            }
            var totalCartItemCount = await GetCartItemCount(userId);
            return totalCartItemCount;
        }






        public async Task<ShoppingCart> GetCart(string userId)
        {
          
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User not logged-in");
                }
                return await _context.Carts
                    .FirstOrDefaultAsync(s => s.UserId == userId);

            }
            catch (Exception ex)
            {

                throw;
            }
        }






        private async Task<string> GetUserId()
        {
            try
            {
                var principal = _httpContextAccessor.HttpContext.User;

                if (principal == null || !principal.Identity.IsAuthenticated)
                {
                    return null; 
                }

                var user = await _userManager.GetUserAsync(principal);

                if (user == null)
                {
                    return null; 
                }

                var userId = user.Id;

                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user ID: {ex.Message}");
                return null; 
            }
        }







        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = await GetUserId();
            if (userId == null)
            {
                throw new Exception("Invalid userId");
            }
            var shoppingCart = await _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(p => p.Product)
                .ThenInclude(cat => cat.Category)
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync();
            return shoppingCart;
        }







        public async Task<int> GetCartItemCount(string userId="")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = await GetUserId();
            }
            var data = await (from cart in _context.Carts
                              join cartDetails in _context.CartDetails
                              on cart.Id equals cartDetails.ShoppingCartId
                              where cart.UserId == userId // Filter by userId
                              select new { cartDetails.Id }).ToListAsync();

            return data.Count();
        }









        public async Task<bool> Checkout(int shippingAddressId, int paymentModeId, int cartId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get the user ID
                var userId = await GetUserId();
                if (userId == null)
                {
                    throw new Exception("User is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Invalid cart");
                }

                var cartDetail = await _context.CartDetails.Where(a => a.ShoppingCartId == cart.Id).ToListAsync();
                if (cartDetail.Count == 0)
                {
                    throw new Exception("Cart is empty");
                }

                if (shippingAddressId == 0)
                    throw new Exception("Shipping details are missing. Please provide shipping details.");

                if (paymentModeId == 0)
                    throw new Exception("Payment mode is missing. Please provide payment mode.");

                // Create an order
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatus = (int)OrderStatus.Pending,
                    ShippingAddressId = shippingAddressId,
                    PaymentModeId = paymentModeId
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Reduce product quantities
                foreach (var item in cartDetail)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        // Check if the available quantity is sufficient
                        if (product.Quantity < item.Quantity)
                        {
                            throw new Exception($"Insufficient quantity for product {product.ProductName}");
                        }

                        // Update the product quantity
                        product.Quantity -= item.Quantity;
                    }
                    else
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found");
                    }

                    // Create order detail
                    var orderDetail = new OrderDetails
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _context.OrderDetails.AddAsync(orderDetail);
                }

                // Remove cart items
                _context.CartDetails.RemoveRange(cartDetail);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Rollback transaction if there's an exception
                await transaction.RollbackAsync();
                // Log the exception
                Console.WriteLine($"An error occurred during checkout: {ex.Message}");
                return false;
            }
        }











        public async Task<ShoppingCart> GetCartItemsByCartId(int cartId)
        {
            try
            {
                var userId = GetUserId();
                if (cartId == null)
                {
                    throw new Exception("Invalid Cart");
                }
                return await _context.Carts
                    .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Product)
                    .FirstOrDefaultAsync(c => c.Id == cartId);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }




        public async Task<List<CartDetails>> GetCartItems(List<int> itemIds)
        {
            // Retrieve cart items based on selected item IDs
            return await _context.CartDetails.Where(item => itemIds.Contains(item.Id)).ToListAsync();
        }




    }

}
