using SushmaElectrical.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Repositories.Interfaces
{
    public interface  IShippingRepo
    {
        Task SetDefaultShippingAddress(ShippingDetails shippingDetails);
        Task<IEnumerable<ShippingDetails>> GetAll();
        Task<ShippingDetails> GetById(int id);
        Task Save(ShippingDetails shipping);
        Task Edit(ShippingDetails shipping);
        Task Delete(int id);
    }
}
