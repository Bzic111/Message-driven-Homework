


using Microsoft.Extensions.Hosting;
using System.Text;

Producer producer = new Producer("BookingNotification", "localhost");
Consumer consumer = new Consumer("Messages", "localhost");

class Listner :  BackgroundService
{
    private readonly Consumer _consumer;
    public Listner(Consumer consumer)
    {
        _consumer = _consumer = new(
                queueName: "Messages",
                hostName: "localhost");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Receive((sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"{DateTime.Now.Date} Received {message}");
        });
    }
}