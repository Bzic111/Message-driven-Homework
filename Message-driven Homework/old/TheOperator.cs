using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class TheOperator
{
    private static string _welcome = "Welcome to our restaurant\n\tNum\tSeats\tStatus";
    private static int rowForMessages;

    public static void Send(string str)
    {
        Console.ResetColor();
        int tempRow = Console.CursorTop;
        Console.SetCursorPosition(0, rowForMessages);
        Console.WriteLine(str);
        Console.SetCursorPosition(0, tempRow);
        rowForMessages++;
    }
    public static void SetRow(int row)
    {
        rowForMessages = row;
    }
    public static void WelcomeMessage()
    {
        Console.SetCursorPosition((Console.WindowWidth / 2) - (_welcome.Length / 2), 0);
        Console.WriteLine(_welcome + "\n");
    }
    public static void ChangeWelcomeMessage(string str) => _welcome = str;

    public static void Send(string str, ConsoleColor bgColor, ConsoleColor fgColor)
    {
        int tempRow = Console.CursorTop;
        Console.BackgroundColor = bgColor;
        Console.ForegroundColor = fgColor;
        Console.SetCursorPosition(0, rowForMessages);
        Console.WriteLine(str);
        Console.SetCursorPosition(0, tempRow);
        Console.ResetColor();
        rowForMessages++;
    }
}
