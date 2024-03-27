using System.ComponentModel.DataAnnotations;

namespace SushmaElectrical.Entities
{
    public class ShippingDetails
    {
        public int Id { get; set; }

        public bool DefaultAddress { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Mobile { get; set; }

        [Display(Name = "Flat, House No., Building, Company, Apartment")]
        public string FlatHouseNo { get; set; }

        [Display(Name = "Area, Street, Sector, Village")]
        public string AreaStreet { get; set; }

        public string Landmark { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        [Display(Name = "Town, City")]
        public string TownCity { get; set; }

        public string Village { get; set; }

        [Display(Name = "Pin Code")]
        public int PinCode { get; set; }

        public string CustomerId { get; set; }

        public ApplicationUser Customer { get; set; }
    }
}
