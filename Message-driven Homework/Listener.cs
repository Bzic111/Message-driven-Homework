using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message_driven_Homework
{
    public class Listener : BackgroundService
    {
        private readonly Consumer _consumer;
        private static Action? _rndAction;
        private static Action<int>? _booking;
        public Listener()
        {
            _consumer = new(
                queueName: "Restaurant",
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
                    if (message == "Random" & _rndAction != null)
                        _rndAction!();
                    else if (int.TryParse(message, out int seats) & _booking != null)
                        _booking!(seats);
                    else
                        Console.WriteLine($"{DateTime.Now.Date} Received {message}");
                });
            });
        }
        public static void SetAction(Action act)
        {
            _rndAction = act;
        }
        public static void SetBookingAction(Action<int> act)
        {
            _booking = act;
        }
    }
}
