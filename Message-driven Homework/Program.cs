
using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;


int row, col, top, left;
var rest = new Restaurant();
var stopWatch = new Stopwatch();
var cntr = (Controls)0b1011;
row = col = top = left = 0;
bool cycle = true;
System.Timers.Timer timer = new(1000);

List<string> menu = new List<string>();


foreach (var item in rest._tables)
{
    menu.Add($"Table\t{item.Id}\t{item.SeatsCount}\t{item.CurrentState}");
}

menu.Add($"Booking random table");
menu.Add($"Exit");

TheOperator.SetRow(menu.Count + 5);

var vActs = new VoidActions(Position.EscapeCase, () =>
{
    cycle = false;
    EndProgram();
    //Thread.CurrentThread.Abort();
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
    if (row < rest._tables.Count)
    {
        rest.BookTable(row);
    }
    else if (row == menu.Count - 1)
    {
        cycle = false;
    }
    else
    {
        rest.BookFreeTableAsync(new Random().Next(1, 5));
    }
});

vActs.AddAction(Position.BackspaceCase, () =>
{
    rest.UnbookTable(row);
});

vActs.AddAction(Position.StartCycle, () =>
{
    TheOperator.WelcomeMessage();
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


void OntTimerAction(Object source, ElapsedEventArgs e)
{

}

void EndProgram()
{
    timer.Stop();
    timer.Dispose();
}

void SetTimer()
{
    //timer = new(1000);
    timer.Elapsed += OntTimerAction;
    timer.AutoReset = true;
    timer.Enabled = true;
}


#region Comments


//System.Timers.Timer aTimer;

//SetTimer();

//Console.WriteLine("\nPress the Enter key to exit the application...\n");
//Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
//Console.ReadLine();
//aTimer.Stop();
//aTimer.Dispose();

//Console.WriteLine("Terminating the application...");
//void SetTimer()
//{
//    // Create a timer with a two second interval.
//    aTimer = new System.Timers.Timer(2000);
//    // Hook up the Elapsed event for the timer. 
//    aTimer.Elapsed += OnTimedEvent;
//    aTimer.AutoReset = true;
//    aTimer.Enabled = true;
//}
//void OnTimedEvent(Object source, ElapsedEventArgs e)
//{
//    Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
//                      e.SignalTime);
//}

//Console.ReadLine();


//while (true)
//{
//    Console.WriteLine("Booking table 1 now? or 2 later:");
//    if (!int.TryParse(Console.ReadLine(), out var choise) || choise is not (1 or 2))
//    {
//        Console.WriteLine("Wrong input");
//        continue;
//    }
//    else
//    {
//        Console.WriteLine("How many persons? 1 - 5");
//        if (!int.TryParse(Console.ReadLine(), out var persons) || persons < 1 || persons > 5)
//        {
//            Console.WriteLine("Wrong persons count");
//            continue;
//        }
//        stopWatch.Start();
//        if (choise == 1)
//        {
//            rest.BookFreeTable(persons);
//        }
//        else
//        {
//            rest.BookFreeTableAsync(persons);
//        }
//        Console.WriteLine("End program...");
//        var ts = stopWatch.Elapsed;
//        stopWatch.Stop();
//        stopWatch.Reset();
//        Console.WriteLine($"{ts.Minutes:00}:{ts.Seconds:00}:{ts.Milliseconds:00}");
//    }
//}
#endregion