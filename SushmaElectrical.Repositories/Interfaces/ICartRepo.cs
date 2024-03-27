using SushmaElectrical.Entities;
using SushmaElectrical.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface ICartRepo
    {
        Task<(CartDetails CartDetail, Product Product)> GetProductDetailsByCartDetailId(int cartDetailId);
        Task<int> AddItem(int productId, int Quantity);
        Task<int> RemoveItem(int productId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId="");
        Task<ShoppingCart> GetCart(string userId);

        Task<bool> Checkout(int shippingAddressId, int paymentMode,int cartId);
        Task<ShoppingCart> GetCartItemsByCartId(int cartId);
        Task<List<CartDetails>> GetCartItems(List<int> itemIds);
    }
}
