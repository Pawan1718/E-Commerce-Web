// PlaceOrderViewModel.cs
using SushmaElectrical.Entities;
using SushmaElectrical.ViewModels;
using System.ComponentModel.DataAnnotations;



public class OrderViewModel
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal GrossPrice { get; set; }
    public int Quantity { get; set; }
    public int OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderDetails>OrderDetails { get; set; }



    public string FullName { get; set; }
    public string Mobile { get; set; }
    public string FlatHouseNo { get; set; }
    public string AreaStreet { get; set; }
    public string Landmark { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string TownCity { get; set; }
    public string Village { get; set; }
    public int PinCode { get; set; }

}
