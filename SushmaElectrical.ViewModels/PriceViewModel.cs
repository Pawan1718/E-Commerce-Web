using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class PriceViewModel
    {
        public int Id { get; set; }
        public decimal Gross { get; set; }
        public decimal Tax { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Surcharge { get; set; }

    }
}
