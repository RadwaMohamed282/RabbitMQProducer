using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQProducer.Services.RabbitMQ;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly Producer _producer;

        public LoyaltyController(IOptions<RabbitMQConfig> options, Producer producer)
        {
            _rabbitMQConfig = options.Value;
            _producer = producer;
        }

        [HttpPost]
        public IActionResult AddNewPoint(RewardLoyalty rewardLoyalty)
        {
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(rewardLoyalty);

            // Publish the product message to RabbitMQ
            _producer.PublishLoyaltyMessage(jsonProduct);

            return Ok(rewardLoyalty);
        }
    }
}
