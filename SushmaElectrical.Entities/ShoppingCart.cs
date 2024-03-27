using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CartDetails> CartDetails { get; set; }=new List<CartDetails>();
    }

  
}
