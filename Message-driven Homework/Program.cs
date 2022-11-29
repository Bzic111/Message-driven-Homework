
using System.Diagnostics;

var rest = new Restaurant();
var stopWatch = new Stopwatch();
while (true)
{

    Console.WriteLine("Booking table 1 now? or 2 later:");
    if (!int.TryParse(Console.ReadLine(), out var choise) || choise is not (1 or 2))
    {
        Console.WriteLine("Wrong input");
        continue;
    }
    else
    {
        Console.WriteLine("How many persons? 1 - 5");
        if (!int.TryParse(Console.ReadLine(), out var persons) || persons < 1 || persons > 5)
        {
            Console.WriteLine("Wrong persons count");
            continue;
        }
        stopWatch.Start();
        if (choise == 1)
        {
            rest.BookFreeTable(persons);
        }
        else
        {
            rest.BookFreeTableAsync(persons);
        }
        Console.WriteLine("End program...");
        var ts = stopWatch.Elapsed;
        stopWatch.Stop();
        stopWatch.Reset();
        Console.WriteLine($"{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds:00}");
    }
}
