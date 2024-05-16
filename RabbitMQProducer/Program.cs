using DeliveryIntegration.Configrations;
using RabbitMQProducer.Services.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

InjectService();
SetConfigurations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
void InjectService()
{
    builder.Services.AddSingleton<Producer>();
}
void SetConfigurations()
{
    builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection(nameof(RabbitMQConfig)));

}