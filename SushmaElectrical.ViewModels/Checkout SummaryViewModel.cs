using SushmaElectrical.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{

    public class CheckoutSummaryViewModel
    {
        public ShoppingCartViewModel SelectedItems { get; set; }
        public PaymentModeViewModel SelectedPaymentMode { get; set; }
        public ShippingViewModel ShippingAddress { get; set; }


    }


}
