using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int CartId { get; set; }
        public string UserId { get; set; }

        public List<CartDetailsViewModel> cartDetails{ get; set; }



    }
}
