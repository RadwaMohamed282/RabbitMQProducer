using DeliveryIntegration.Configrations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProducer.Services.RabbitMQ
{
    public class Producer : BackgroundService
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly IModel _channel;
        private readonly IModel _couponzChannel;
        private readonly IModel _loyaltyChannel;


        public Producer(IOptions<RabbitMQConfig> options)
        {
            _rabbitMQConfig = options.Value;
            var factory = new ConnectionFactory() 
            { 
                HostName = _rabbitMQConfig.HostName, 
                UserName = _rabbitMQConfig.UserName, 
                Password = _rabbitMQConfig.Password 
            };
            var connection = factory.CreateConnection();

            // Initialize channels for both queues
            _couponzChannel = connection.CreateModel();
            _loyaltyChannel = connection.CreateModel();

            _channel = connection.CreateModel();

        }

        protected override Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.FromResult(Task.CompletedTask);
        }

        public void PublishMessage(string message)
        {
            _channel.QueueDeclare(queue: _rabbitMQConfig.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: _rabbitMQConfig.QueueName, body: body);
        }

        public void PublishCouponzMessage(string message)
        {
            _couponzChannel.QueueDeclare(queue: _rabbitMQConfig.CouponzQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            _couponzChannel.BasicPublish(exchange: "", routingKey: _rabbitMQConfig.CouponzQueue, body: body);
        }

        public void PublishLoyaltyMessage(string message)
        {
            _loyaltyChannel.QueueDeclare(queue: _rabbitMQConfig.LoyaltyQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _loyaltyChannel.BasicPublish(exchange: "", routingKey: _rabbitMQConfig.LoyaltyQueue, body: body);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up resources before the service stops
            _couponzChannel.Close();
            _loyaltyChannel.Close();
            _couponzChannel.Dispose();
            _loyaltyChannel.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}