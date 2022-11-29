
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


public enum State
{
    Free = 0,
    Booked = 1
}
public class Table
{
    public State CurrentState { get; private set; }
    public int SeatsCount { get; }
    public int Id { get; }
    public Table(int id)
    {
        Id = id;
        CurrentState = State.Free;
        SeatsCount = new Random().Next(2, 5);
    }
    public bool SetState(State st)
    {
        if (st == CurrentState)
            return false;
        CurrentState = st;
        return true;
    }
}
public class Restaurant
{
    private readonly List<Table> _tables = new List<Table>();
    public Restaurant()
    {
        for (ushort i = 0; i < 10; i++)
        {
            _tables.Add(new Table(i));
        }
    }
    public void BookFreeTable(int personsCount)
    {
        Console.WriteLine("Cheking tables");
        var table = _tables.FirstOrDefault(t => t.SeatsCount >= personsCount && t.CurrentState is State.Free);
        Thread.Sleep(1000 * 2);
        string result;
        if (table is null)
        {
            result = $"No tables";
        }
        else
        {
            table.SetState(State.Booked);
            result = $"Yours table {table.Id}";
        }
        Console.WriteLine(result);
    }
    public void BookFreeTableAsync(int personsCount)
    {
        Console.WriteLine("Cheking tables");
        Task.Run(async () =>
        {
            var table = _tables.FirstOrDefault(t => t.SeatsCount >= personsCount && t.CurrentState is State.Free);
            await Task.Delay(1000 * 2);
            string result;
            if (table is null)
            {
                result = $"No tables";
            }
            else
            {
                table.SetState(State.Booked);
                result = $"Yours table {table.Id}";
            }
            Console.WriteLine(result);
        });
    }
}
