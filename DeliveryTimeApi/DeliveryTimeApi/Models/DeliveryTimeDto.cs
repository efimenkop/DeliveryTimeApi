using System;

namespace DeliveryTimeApi.Models
{
    public class DeliveryTimeDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public decimal Price { get; set; }
        public DeliveryType Type { get; set; }
        public bool Available { get; set; }
    }
}
