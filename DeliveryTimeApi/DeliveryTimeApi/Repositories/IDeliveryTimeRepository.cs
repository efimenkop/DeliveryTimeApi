using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;

namespace DeliveryTimeApi.Repositories
{
    public interface IDeliveryTimeRepository
    {
        Task Add(DeliveryTime item);
        Task<List<DeliveryTime>> Get();
    }
}
