using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

// 1. SINGLETON (Центробанк)
public class CentralBank
{
    public static CentralBank Instance { get; } = new CentralBank();
    public decimal Rate { get; set; } = 0.05m;
    private CentralBank() { }
}

// 2. STRATEGY (Відсотки)
public interface IStrategy { decimal Calc(decimal bal); }
public class RegularStrategy : IStrategy { public decimal Calc(decimal bal) => bal * CentralBank.Instance.Rate; }
public class VipStrategy : IStrategy { public decimal Calc(decimal bal) => bal * (CentralBank.Instance.Rate + 0.05m); }

// 3. OBSERVER (Біржа)
public interface IObserver { void Update(string stock, decimal price); }
public class StockMarket
{
    private List<IObserver> observers = new List<IObserver>();
    public void Attach(IObserver o) => observers.Add(o);
    public void ChangePrice(string stock, decimal price)
    {
        Console.WriteLine($"\n[БІРЖА] {stock} коштує {price}");
        foreach (var o in observers) o.Update(stock, price);
    }
}

public class Investor : IObserver
{
    public string Name;
    public void Update(string stock, decimal price) => Console.WriteLine($" -> Інвестор {Name} бачить: {stock} = {price}");
}

// Рахунок з фабрикою
public class Account 
{ 
    public decimal Balance; 
    public IStrategy Strategy;
    public void AddInterest() => Balance += Strategy.Calc(Balance);
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== БАНК ПАТЕРНИ (Лабораторна 3) ===");

        // Тест Стратегії
        var acc = new Account { Balance = 1000, Strategy = new RegularStrategy() };
        acc.AddInterest();
        Console.WriteLine($"Баланс з відсотками (Regular): {acc.Balance}");

        // Тест Обсервера
        StockMarket market = new StockMarket();
        market.Attach(new Investor { Name = "Воррен" });
        market.ChangePrice("Apple", 150);

        Console.ReadLine();
    }
}
