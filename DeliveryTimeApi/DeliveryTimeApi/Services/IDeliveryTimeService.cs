using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;

namespace DeliveryTimeApi.Services
{
    public interface IDeliveryTimeService
    {
        Task Add(DeliveryTime item);
        Task<IEnumerable<DeliveryTimeDto>> Get(DateTime currentDate, int horizon);
    }
}
