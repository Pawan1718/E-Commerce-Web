using SushmaElectrical.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface IPaymentModeRepo
    {
        Task<List<PaymentMode>> GetAllPaymentModes();
        Task<PaymentMode> GetPaymentModeById(int paymentModeId);
        Task<bool> SetPaymentMode(int id);
    }
}
