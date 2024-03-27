using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class CartDetails
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
