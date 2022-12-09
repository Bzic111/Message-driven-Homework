using RabbitMQ.Client;
using System.Text;

public class Producer
{
    private string _queueName;
    private string _hostName;
    private string _exchange;
    private string _type;
    public Producer(string queueName, string hostName)
    {
        _hostName = hostName;
        _queueName = queueName;
        _exchange = "default";
        _type = "direct";
    }
    public Producer(string queueName, string hostName, string exchangeName, string type) : this(queueName, hostName)
    {
        _exchange = exchangeName;
        _type = type;
    }
    public void SetExchange(string exchangeName, string type)
    {
        _exchange = exchangeName;
        _type = type;
    }
    public void Send(string message, string exchange, string type)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using (var _connection = factory.CreateConnection())
        {
            using (var _channel = _connection.CreateModel())
            {
                _channel.ExchangeDeclare(
                    exchange: exchange,
                    type: type,
                    durable: false,
                    autoDelete: false,
                    arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(
                    exchange: exchange,
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
    public void Send(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using (var _connection = factory.CreateConnection())
        {
            using (var _channel = _connection.CreateModel())
            {
                _channel.ExchangeDeclare(
                    exchange: "direct_exchange",
                    type: "direct",
                    durable: false,
                    autoDelete: false,
                    arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(
                    exchange: "direct_exchange",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}