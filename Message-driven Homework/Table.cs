using System.Timers;

public class Table
{
    private System.Timers.Timer? _timer;
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
        {
            return false;
        }
        else if (st == State.Booked)
        {
            CurrentState = st;
            SetTimer();
            TheOperator.Send($"Table {Id} Booked for 20sec");
            return true;
        }
        else
        {
            CurrentState = st;
            return true;
        }
    }
    private void SetTimer()
    {
        _timer = new System.Timers.Timer(20000);
        _timer.Elapsed += OnTimerEvent!;
        _timer.AutoReset = false;
        _timer.Enabled = true;
    }

    private void OnTimerEvent(Object source, ElapsedEventArgs e)
    {
        SetState(State.Free);
        TheOperator.Send($"Table {Id} is free for booking");
    }
}
