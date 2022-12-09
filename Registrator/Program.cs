
Producer producer = new Producer("Restaurant", "localhost");
//Consumer consumer = new Consumer("Messages", "localhost");

int row, col, top, left;
var cntr = (Controls)0b1011;
row = col = top = left = 0;
bool cycle = true;
List<string> menu = new List<string>();

menu.Add($"Book table with seats count");
menu.Add($"Booking random table");
menu.Add($"Exit");


var vActs = new VoidActions(Position.EscapeCase, () =>
{
    cycle = false;
});

vActs.AddAction(Position.UpCase, () =>
{
    if (row > 0)
    {
        row--;
        top--;
    }
});

vActs.AddAction(Position.DownCase, () =>
{
    if (row < menu.Count - 1)
    {
        row++; top++;
    }
});

vActs.AddAction(Position.EnterCase, () =>
{
    switch (row)
    {
        case 0:
            producer.Send(AskSeats());
            break;
        case 1:
            producer.Send("Random");
            break;
        case 2:
            cycle = false;
            break;
        default:
            break;
    }
});

vActs.AddAction(Position.StartCycle, () =>
{
    Console.ResetColor();
    WelcomeMessage(null);
    top = Console.CursorTop;
    left = 0;
    foreach (var item in menu)
        Console.WriteLine(item);
    Console.SetCursorPosition(left, top);

});

vActs.AddAction(Position.AfterKeyPosition, () =>
{
    Console.ResetColor();
    Console.Write(menu[row]);
    Console.CursorLeft = left;
});

vActs.AddAction(Position.BeforeKeyPosition, () =>
{
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write(menu[row]);
    Console.CursorLeft = left;
    Console.ResetColor();
});

vActs.AddAction(Position.AfterSwitchPosition, () =>
{
    Console.SetCursorPosition(left, top);
});

ConsoleSelector.MultiLine(cntr, vActs, ref cycle, true);

void WelcomeMessage(string? welcome)
{
    welcome ??= "Welcome to our restaurant";
    Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2), 0);
    Console.WriteLine(welcome + "\n");
}
string AskSeats()
{
    int top = Console.CursorTop;
    int left = Console.CursorLeft;
    Console.ResetColor();
    Console.SetCursorPosition(0, 10);
    Console.Write("Input number of seats: ");
    var seats = Console.ReadLine();
    Console.SetCursorPosition(left, top);
    return seats!;
}