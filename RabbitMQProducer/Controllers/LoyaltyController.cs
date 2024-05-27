using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQProducer.DTO;
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
        public ActionResult<IEnumerable<RewardLoyalty>> AddNewPoint(List<RewardLoyaltyDto> rewardLoyalty)
        {
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(rewardLoyalty);

            if (rewardLoyalty == null || rewardLoyalty.Count == 0)
            {
                return BadRequest("The subscription list cannot be empty.");
            }   
            // Publish the product message to RabbitMQ
            _producer.PublishLoyaltyMessage(jsonProduct);

            return Ok(rewardLoyalty);
        }
    }
}
