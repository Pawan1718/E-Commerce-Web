﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class CreateProductViewModel
    {
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public decimal Tax { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal GrossPrice { get; set; }
        public string? Brand { get; set; }
        public string? ModelNumber { get; set; }
        public string? Voltage { get; set; }
        public string? PowerRating { get; set; }
        public List<IFormFile> ProductImages { get; set; } // Changed to List<IFormFile>

    }
}