using Microsoft.AspNetCore.Http;
using SushmaElectrical.ViewModels.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string?  ProductImages{ get; set; }
        public string? Brand { get; set; }
        public string? ModelNumber { get; set; }
        public string? Voltage { get; set; }
        public string? PowerRating { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }    
    }

    public class PagedProductViewModel
    {
        public List<ProductViewModel>Products { get; set; }
        public PageInfo PageInfo {  get; set; }
    }
}
