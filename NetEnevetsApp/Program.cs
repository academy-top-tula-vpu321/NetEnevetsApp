using System.Threading.Channels;

BankAccount account = new(1000);
account.Notify += ConsoleColorMessage;

account.Take(400);
account.Take(400);
account.Take(400);




void ConsoleColorMessage(BankAccount sender, BankAccountEventArgs e)
{
    var colorOld = Console.ForegroundColor;

    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine($"Red message from Bank: {e.Message}");

    Console.ForegroundColor = colorOld;
}

class BankAccount
{
    int amount;
    event BankAccountHandler notify;

    public BankAccount(int amount) => this.amount = amount;

    public void Add(int amount)
    {
        this.amount += amount;
        notify?.Invoke(this, new($"Add sum {amount}. Balace: {this.amount}", this.amount, amount));
    }

    public void Take(int amount)
    {
        if (this.amount >= amount)
        {
            this.amount -= amount;
            notify?.Invoke(this, new($"Take sum {amount}. Balace: {this.amount}", this.amount, amount));
        }
        else
            notify?.Invoke(this, new($"Not take sum {amount}. Balace: {this.amount}", this.amount, amount));
    }

    public event BankAccountHandler Notify
    {
        add
        {
            notify += value;
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Handler {value.Method.Name} add to event notify");
            Console.ForegroundColor = colorOld;
            
        }
        remove
        {
            notify -= value;
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Handler {value.Method.Name} remove from event notify");
            Console.ForegroundColor = colorOld;
            
        }
    }

    public delegate void BankAccountHandler(BankAccount sender, BankAccountEventArgs e);

}


class BankAccountEventArgs
{
    public string Message { get; }
    public int Amount { get; }
    public int Balance { get; }
    public BankAccountEventArgs(string message, int balance, int amount)
    {
        Message = message;
        Amount = amount;
        Balance = balance;
    }
}