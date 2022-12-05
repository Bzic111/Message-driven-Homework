using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Consumer:IDisposable
{
	private string _queueName;
	private string _hostName;
	private IConnection _connection;
	private IModel _channel;
	public Consumer(string queueName,string hostName)
	{
        _hostName = hostName;
        _queueName = queueName;
        var factory = new ConnectionFactory() { HostName = _hostName };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
	}
	public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
	{
		_channel.ExchangeDeclare(exchange:"direct_exchange",type: "direct");
		_channel.QueueDeclare(
			queue: _queueName,
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);
		_channel.QueueBind(
			queue: _queueName,
			exchange: "direct_exchange",
			routingKey: _queueName);
		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += receiveCallback;
		_channel.BasicConsume(
			queue: _queueName,
			autoAck: true,
			consumer: consumer);
	}
    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
