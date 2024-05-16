using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQProducer.Services.RabbitMQ;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly Producer _producer;

        public ProductController(IOptions<RabbitMQConfig> options, Producer producer)
        {
            _rabbitMQConfig = options.Value;
            _producer = producer;
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(product);

            // Publish the product message to RabbitMQ
            _producer.PublishMessage(jsonProduct);

            return Ok(product);
        }
    }
}
