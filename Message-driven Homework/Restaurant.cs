using System.ComponentModel.DataAnnotations.Schema;

public class Restaurant
{
    private readonly Producer _producer;// = new Producer("BookingNotification", "localhost");
    private readonly object _lock = new object();
    public List<Table> _tables { get; private set; }
    public Restaurant()
    {
        _tables = new();
        for (ushort i = 0; i < 10; i++)
            _tables.Add(new Table(i));
        _producer = new Producer("BookingNotification", "localhost", "", "fanout");
        _producer.Send("Restaurant is working");
    }
    public void BookTable(int id)
    {
        var temp = GetTable(id);
        lock (_lock)
        {
            temp!.SetState(State.Booked);
        }
    }
    public void BookFreeTable(int personsCount)
    {
        _producer.Send("Cheking tables", "BookingNotification", "fanout");
        var table = _tables.FirstOrDefault(t => t.SeatsCount >= personsCount && t.CurrentState is State.Free);
        Thread.Sleep(1000 * new Random().Next(1, 5));
        string result;
        if (table is null)
        {
            result = $"No tables";
        }
        else
        {
            table.SetState(State.Booked);
            result = $"Table {table.Id} is booked";
        }
        _producer.Send(result, "BookingNotification", "fanout");
    }
    public async void BookFreeTableAsync(int personsCount)
    {
        _producer.Send("Cheking tables", "BookingNotification", "fanout");
        //_producer.Send("Cheking tables");
        await Task.Run(() =>
        {
            lock (_lock)
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount >= personsCount & t.CurrentState == State.Free);
                Thread.Sleep(1000 * new Random().Next(1, 5));
                string result;
                if (table is null)
                {
                    result = $"No tables";
                }
                else
                {
                    table.SetState(State.Booked);
                    result = $"Table {table.Id} is booked";
                }
                _producer.Send(result, "BookingNotification", "fanout");
            }
        });
    }
    private Table? GetTable(int id) => _tables.FirstOrDefault(t => t.Id == id);
    private async Task<Table?> GetTableAsync(int id) => await new Task<Table?>(() => { return _tables.FirstOrDefault(t => t.Id == id); });
    public void UnbookTable(int tableNum)
    {
        var temp = GetTable(tableNum);
        if (temp is not null && temp.CurrentState == State.Booked)
            temp.SetState(State.Free);
    }
    public async void UnbookTableAsync(int tableNum)
    {
        await Task.Run(async () =>
        {
            var temp = await GetTableAsync(tableNum);
            if (temp is not null && temp.CurrentState == State.Booked)
            {
                lock (_lock)
                {
                    temp.SetState(State.Free);
                }
            }
        });
    }
}
