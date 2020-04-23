using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;
using DeliveryTimeApi.Repositories;

namespace DeliveryTimeApi.Services
{
    public class DeliveryTimeService : IDeliveryTimeService
    {
        private readonly IDeliveryTimeRepository _repository;

        public DeliveryTimeService(IDeliveryTimeRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(DeliveryTime item)
        {
            await _repository.Add(item);
        }

        public async Task<IEnumerable<DeliveryTimeDto>> Get(DateTime currentDate, int horizon)
        {
            var result = new List<DeliveryTimeDto>();

            var deliveryTimes = await _repository.Get();

            for (var i = 0; i <= horizon; i++)
            {
                var nextDate = currentDate.AddDays(i);
                var validDeliveryTimes = deliveryTimes
                    .Where(d => d.ExistAt(currentDate, nextDate))
                    .Select(x => new DeliveryTimeDto
                    {
                        Name = x.Name,
                        Description = x.Description,
                        Start = x.CalculateStart(nextDate),
                        Finish = x.CalculateFinish(nextDate),
                        Type = x.Type,
                        Price = x.Price,
                        Available = x.IsAvailable(currentDate, nextDate)
                    });

                result.AddRange(validDeliveryTimes);
            }

            return result;
        }
    }
}
