using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;

namespace DeliveryTimeApi.Repositories
{
    public class DeliveryTimeInMemoryRepository : IDeliveryTimeRepository
    {
        private readonly List<DeliveryTime> _deliveryTimes = new List<DeliveryTime>();

        public Task Add(DeliveryTime item)
        {
            _deliveryTimes.Add(item);

            return Task.CompletedTask;
        }

        public Task<List<DeliveryTime>> Get()
        {
            return Task.FromResult(_deliveryTimes);
        }
    }
}
