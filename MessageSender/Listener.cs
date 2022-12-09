using Microsoft.Extensions.Hosting;
using System.Text;

class Listener : BackgroundService
{
    private readonly Consumer _consumer;
    public Listener()
    {
        _consumer = _consumer = new(
                queueName: "BookingNotification",
                hostName: "localhost",
                exchange: "ForAll",
                type: "fanout");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            _consumer.Receive(
                receiveCallback:(sender, args) =>
                    {
                        var body = args.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"{DateTime.Now.Date} Received {message}");
                    }, 
                exchange: "ForAll",
                type: "fanout",
                durable:false,
                exclusive:false,
                autoDelete:false,
                autoAck:true
            );
        });
    }
}