using SushmaElectrical.Entities;
using SushmaElectrical.ViewModels.DashboardViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        Task<Order> GetOrderByCartId(int cartId);
        Task UpdateOrder(Order order);
        Task<IEnumerable<Order>> GetUserOrder();
        Task<IEnumerable<Order>> GetAllOrders(int pageSize, int page);
        Task<Order> GetOrderById(int orderId);
        Task<bool> CancelOrder(int orderId);
        Task<bool> OrderIssue(int orderId);
        Task<int> GetTotalCustomerOrderCount();
    }
}
