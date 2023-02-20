﻿using System.Collections.Concurrent;

namespace BlockingCollection;

public class StockController
{
    private ConcurrentDictionary<string, int> _stock = 
        new ConcurrentDictionary<string, int>();
    int _totalQuantityBought;
    int _totalQuantitySold;
    public void BuyShirts(string code, int quantityToBuy)
    {
        _stock.AddOrUpdate(code, quantityToBuy,
            (key, oldValue) => oldValue + quantityToBuy);
        Interlocked.Add(ref _totalQuantityBought, quantityToBuy);
    }

    public bool TrySellShirt(string code)
    {
        bool success = false;
        int newStockLevel = _stock.AddOrUpdate(code,
            (itemName) => { success = false; return 0; },
            (itemName, oldValue) =>
            {
                if (oldValue == 0)
                {
                    success = false;
                    return 0;
                }
                else
                {
                    success = true;
                    return oldValue - 1;
                }
            });
        if (success)
            Interlocked.Increment(ref _totalQuantitySold);
        return success;
    }

    public void DisplayStock()
    {
        Console.WriteLine("Stock levels by item:");
        foreach (TShirt shirt in TShirtProvider.AllShirts)
        {
            //_stock.TryGetValue(shirt.Code, out int stockLevel);
            int stockLevel = _stock.GetOrAdd(shirt.Code, 0);
            Console.WriteLine($"{shirt.Name,-30}: {stockLevel}");
        }

        int totalStock = _stock.Values.Sum();
        Console.WriteLine($"\r\nBought = {_totalQuantityBought}");
        Console.WriteLine($"Sold   = {_totalQuantitySold}");
        Console.WriteLine($"Stock  = {totalStock}");
        int error = totalStock + _totalQuantitySold - _totalQuantityBought;
        if (error == 0)
            Console.WriteLine("Stock levels match");
        else
            Console.WriteLine($"Error in stock level: {error}");
    }
}