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
