using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class ProductsOrderedByUserViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string? ModelNumber { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
