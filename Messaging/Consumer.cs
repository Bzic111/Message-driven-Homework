using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Consumer : IDisposable
{
	private string _exchange;
	private string _type;
	private string _queueName;
	private string _hostName;
	private IConnection _connection;
	private IModel _channel;
	//private bool _durable = false;
	//private bool _exclusive = false;
	//private bool _autoDelete = false;
	//private bool _autoAck = true;

	public Consumer(string queueName, string hostName)
	{
		_hostName = hostName;
		_queueName = queueName;
		var factory = new ConnectionFactory() { HostName = _hostName };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();
		_exchange = "default";
		_type = "direct";
	}
	public Consumer(string queueName, string hostName, string exchange, string type) : this(queueName, hostName)
	{
		_exchange = exchange;
		_type = type;
	}
	public void SetExchange(string exchange, string type)
	{
		_exchange = exchange;
		_type = type;
	}

	public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback, string? exchange = "", string? type = "", bool durable = false, bool exclusive = false, bool autoDelete = false, bool autoAck = true)
	{
		exchange ??= _exchange;
		type ??= _type;
		_channel.ExchangeDeclare(exchange, type);
		_channel.QueueDeclare(_queueName, durable, exclusive, autoDelete);
		_channel.QueueBind(_queueName, exchange, _queueName);
		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += receiveCallback;
		_channel.BasicConsume(_queueName, autoAck, consumer);
	}
	public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
	{
		_channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");
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
