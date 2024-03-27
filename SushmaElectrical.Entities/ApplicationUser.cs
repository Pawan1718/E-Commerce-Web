using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Entities
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Phone]
        [Required]
        public string Mobile { get; set; }
        public ICollection<ShippingDetails> ShippingDetails { get; set; } = new List<ShippingDetails>();
    }
}
