﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.Infrastructure.GlobalConfiguration
{
    public enum OrderStatus
    {
        Pending = 1,
        Completed = 2,
        Processing = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6,
        Refunded = 7
    }
}
