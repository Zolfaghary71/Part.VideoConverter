using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Part.VideoUploader.Service.Contracts.Infrastructure;

namespace Part.VideoUploader.Infrastructure.RabbitMq;

public class RabbitMqQueue<T> : IQueue<T>, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMqQueue(string hostname, string queueName)
    {
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = queueName;

        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public Task EnqueueAsync(T item)
    {
        var message = JsonSerializer.Serialize(item);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
            routingKey: _queueName,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }

    public Task<T?> DequeueAsync()
    {
        var result = _channel.BasicGet(_queueName, true);
        if (result == null)
        {
            return Task.FromResult(default(T)); 
        }

        var message = Encoding.UTF8.GetString(result.Body.ToArray());
        var deserializedMessage = JsonSerializer.Deserialize<T>(message);

        return Task.FromResult(deserializedMessage);
    }


    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
