using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CreateHostBuilder(args).Build().Run();
Producer prod = new Producer("", "localhost");
var cycle = true;
while (cycle)
{
    var str = Console.ReadLine();
    prod.Send(str!);
    cycle = Console.ReadKey(false).Key != ConsoleKey.Escape;

}
IHostBuilder CreateHostBuilder(string[] args)
{
    var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
    {
        //services.AddHostedService<Worker>();
        services.AddHostedService<FanoutWorker>();
    });

    return host;
}



public class Worker : BackgroundService
{
    private readonly Consumer _consumer;
    public Worker()
    {
        _consumer = new(
            queueName: "",
            hostName: "localhost");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{DateTime.Now.Date} Received {message}");
            });
        });
    }
}

public class FanoutWorker : BackgroundService
{
    private readonly Consumer _consumer;
    public FanoutWorker()
    {
        _consumer = new(
            queueName: "",
            hostName: "localhost");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() => 
        { 
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{DateTime.Now.Date} Received: {message}");
            });
        });
    }
}

public class Consumer : IDisposable
{
    private string _queueName;
    private string _hostName;
    private IConnection _connection;
    private IModel _channel;
    public Consumer(string queueName, string hostName)
    {
        _hostName = hostName;
        _queueName = queueName;
        var factory = new ConnectionFactory() { HostName = _hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
    {
        _channel.ExchangeDeclare(exchange: "logs", type: "fanout");

        _channel.QueueDeclare(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.QueueBind(
            queue: _queueName,
            exchange: "logs",
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
                    exchange: "logs",
                    type: "fanout",
                    durable: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(
                    exchange: "logs",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}