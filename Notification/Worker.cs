using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification
{
    public class Worker : BackgroundService
    {
        private readonly Consumer _consumer;
        public Worker()
        {
            _consumer = new(
                queueName: "BookingNotification",
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
}
