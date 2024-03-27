using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser Users { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;

        public int PaymentModeId { get; set; }
        public PaymentMode PaymentMode { get; set; }

        public int ShippingAddressId { get; set; }
        public ShippingDetails ShippingAddress { get; set; }
        public int OrderStatus { get; set; }
        public List<OrderDetails>orderDetails { get; set; }
    }
}
