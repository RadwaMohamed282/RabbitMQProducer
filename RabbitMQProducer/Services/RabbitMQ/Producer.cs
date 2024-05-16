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
        public Producer(IOptions<RabbitMQConfig> options)
        {
            _rabbitMQConfig = options.Value;
            var factory = new ConnectionFactory() { HostName = _rabbitMQConfig.HostName, UserName = _rabbitMQConfig.UserName, Password = _rabbitMQConfig.Password };
            var connection = factory.CreateConnection();
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

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up resources before the service stops
            await base.StopAsync(cancellationToken);
        }
    }
}