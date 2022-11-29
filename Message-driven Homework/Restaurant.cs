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
