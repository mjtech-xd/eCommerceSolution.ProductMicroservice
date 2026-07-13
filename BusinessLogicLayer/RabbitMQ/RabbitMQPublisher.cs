using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    
    private RabbitMQPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }


    public static async Task<RabbitMQPublisher> CreateAsync(IConfiguration configuration)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ_HostName"] ?? "",
            UserName = configuration["RabbitMQ_UserName"] ?? "",
            Password = configuration["RabbitMQ_Password"] ?? "",
            Port = int.Parse(configuration["RabbitMQ_Port"] ?? "5672")
        };
        
        var connection =await  connectionFactory.CreateConnectionAsync();

        var channel = await connection.CreateChannelAsync();
        return new RabbitMQPublisher(connection, channel);
    }


    
    public async Task PublishAsync<T>(string routingKey, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

        string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;

        await _channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Direct,
            durable: true);

        await _channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: messageBodyInBytes);
    }


    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}