namespace DeliveryIntegration.Configrations
{
    public class RabbitMQConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }

        public string CouponzQueue { get; set; }
        public string LoyaltyQueue { get; set; }

    }
}
