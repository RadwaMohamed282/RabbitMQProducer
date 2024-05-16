using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQProducer.Services.RabbitMQ;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponzController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly Producer _producer;

        public CouponzController(IOptions<RabbitMQConfig> options, Producer producer)
        {
            _rabbitMQConfig = options.Value;
            _producer = producer;
        }

        [HttpPost]
        public IActionResult AddNewOffer(SubscribeToOffer subscribeToOffer)
        {
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(subscribeToOffer);

            // Publish the product message to RabbitMQ
            _producer.PublishMessage(jsonProduct);

            return Ok(subscribeToOffer);
        }
    }
}
