using System;
using DeliveryTimeApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using DeliveryTimeApi.Services;

namespace DeliveryTimeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryTimesController : ControllerBase
    {
        private readonly IDeliveryTimeService _service;

        public DeliveryTimesController(IDeliveryTimeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<DeliveryTimeDto>> Get(DateTime currentDate, int horizon)
        {
            return await _service.Get(currentDate, horizon);
        }

        [HttpPost]
        public async Task Post(DeliveryTime deliveryTime)
        {
            await _service.Add(deliveryTime);
        }
    }
}
