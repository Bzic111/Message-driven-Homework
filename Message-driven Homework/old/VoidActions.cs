using System.ComponentModel.DataAnnotations;

public class VoidActions
{
    [Required]
    public Dictionary<Position, Action> Actions { get; private set; }
    private VoidActions()
    {
        Actions = new(18);
    }
    public VoidActions(Position pos, Action action) : this()
    {
        Actions.Add(pos, action);
    }
    public VoidActions(Dictionary<Position, Action> acts) : this()
    {
        Actions = acts;
    }
    public void AddAction(Position pos, Action act)
    {
        if (!Actions.TryGetValue(pos, out _))
            Actions.Add(pos, act);
    }

    public void RemoveAction(Position pos)
    {
        Actions.Remove(pos);
    }

    public void ChangeAction(Position pos, Action act)
    {
        RemoveAction(pos);
        AddAction(pos, act);
    }

    public bool Activate(Position pos)
    {
        if (Actions.TryGetValue(pos, out var act))
        {
            act();
            return true;
        }
        return false;
    }
}
