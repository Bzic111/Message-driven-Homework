public static class ConsoleSelector
{

    public static void Switcher(VoidActions vAct, ConsoleKey? key)
    {
        if (vAct is not null)
            switch (key)
            {
                case ConsoleKey.UpArrow: vAct.Activate(Position.UpCase); break;
                case ConsoleKey.DownArrow: vAct.Activate(Position.DownCase); break;
                case ConsoleKey.LeftArrow: vAct.Activate(Position.LeftCase); break;
                case ConsoleKey.RightArrow: vAct.Activate(Position.RightCase); break;
                case ConsoleKey.Enter: vAct.Activate(Position.EnterCase); break;
                case ConsoleKey.Escape: vAct.Activate(Position.EscapeCase); break;
                case ConsoleKey.Backspace: vAct.Activate(Position.BackspaceCase); break;
                case ConsoleKey.PageUp: vAct.Activate(Position.PgUpCase); break;
                case ConsoleKey.PageDown: vAct.Activate(Position.PgDownCase); break;
                case ConsoleKey.Home: vAct.Activate(Position.HomeCase); break;
                case ConsoleKey.End: vAct.Activate(Position.EndCase); break;
                case null: vAct.Activate(Position.NullCase); break;
                default: vAct.Activate(Position.DefaultCase); break;
            }

    }

    public static void MultiLine(
        Controls flag,
        VoidActions vAct,
        ref bool cycle,
        bool outerKeyGet = false)
    {
        ConsoleKey? key = null;
        vAct.Activate(Position.StartCycle);
        do
        {
            vAct.Activate(Position.BeforeKeyPosition); //select line

            if (outerKeyGet)
            {
                key = Console.ReadKey(true).Key;
                vAct.Activate(Position.AfterKeyPosition);
            }

            Switcher(vAct, GetControl(flag, key));
            vAct.Activate(Position.AfterSwitchPosition); // change position
        } while (cycle);
        vAct.Activate(Position.AfterCycle);
    }

    public static ConsoleKey? GetControl(Controls flag, ConsoleKey? key = null)
    {
        if (key is null)
            key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow or ConsoleKey.DownArrow
            when flag.HasFlag(Controls.UD):
                return key;

            case ConsoleKey.LeftArrow or ConsoleKey.RightArrow
            when flag.HasFlag(Controls.LR):
                return key;

            case ConsoleKey.PageDown or ConsoleKey.PageUp or ConsoleKey.Home or ConsoleKey.End
            when flag.HasFlag(Controls.PGHE):
                return key;

            case ConsoleKey.Escape or ConsoleKey.Enter or ConsoleKey.Backspace
            when flag.HasFlag(Controls.EEB):
                return key;

            default:
                return null;
        }
    }
}
