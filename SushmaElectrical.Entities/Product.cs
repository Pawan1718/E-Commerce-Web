using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public bool IsFavorite { get; set; }
        public string? ImagesUrl { get; set; }
        public string? Brand { get; set; }
        public string? ModelNumber { get; set; }
        public string? Voltage { get; set; }
        public string? PowerRating { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal GST { get; set; }

        public int CategoryId { get; set;}
        public Category Category { get; set;}

        public List<OrderDetails>orderDetails { get; set; }
        public List<CartDetails>CartDetails { get; set; }

    }
}
