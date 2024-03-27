using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class PaymentMode
    {
        public int Id { get; set; }
        public string PaymentModeTitle { get; set; }
        public string? Description { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>(); 
    }
}
