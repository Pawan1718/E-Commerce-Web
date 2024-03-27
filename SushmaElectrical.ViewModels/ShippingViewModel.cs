using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class ShippingViewModel
    {
        public int Id { get; set; }

        public bool DefaultAddress { get; set; }

        [Display(Name = "Full Name")]
        public required string FullName { get; set; }

        public required string Mobile { get; set; }

        [Display(Name = "Flat, House No., Building, Company, Apartment")]
        public required string FlatHouseNo { get; set; }

        [Display(Name = "Area, Street, Sector, Village")]
        public required string AreaStreet { get; set; }

        public required string Landmark { get; set; }

        public required string Country { get; set; }

        public required string State { get; set; }

        [Display(Name = "Town, City")]
        public required string TownCity { get; set; }

        public required string Village { get; set; }

        [Display(Name = "Pin Code")]
        public required int PinCode { get; set; }
    }
}
