using RabbitMQ.Client;
using System.Text;

public class Producer
{
    private string _queueName;
    private string _hostName;
    public Producer(string queueName, string hostName)
    {
        _hostName = hostName; 
        _queueName = queueName;
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
                    durable:false,
                    autoDelete:false,
                    arguments:null);
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