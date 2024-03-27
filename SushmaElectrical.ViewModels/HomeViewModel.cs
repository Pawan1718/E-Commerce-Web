using SushmaElectrical.ViewModels.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class HomeViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductImages { get; set; } 
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
    public class PagedHomeViewModel
    {
        public List<HomeViewModel> Home { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
