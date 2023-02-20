using System.Collections.Concurrent;

namespace BlockingCollection;

public class LogTradesQueue
{
    private BlockingCollection<Trade> _tradesToLog 
        = new BlockingCollection<Trade>();
    private readonly StaffRecords _staffLogs;
    // private bool _workingDayComplete;
    public LogTradesQueue(StaffRecords staffLogs)
    {
        _staffLogs = staffLogs;
    }
    // public void SetNoMoreTrades() => _workingDayComplete = true;
    public void SetNoMoreTrades() => _tradesToLog.CompleteAdding();
    public void QueueTradeForLogging(Trade trade) => _tradesToLog.TryAdd(trade);

    public void MonitorAndLogTrades()
    {
        //// Before BlockingCollection or implementation with IProducerConsumerCollection
        //while (true)
        //{
        //    Trade nextTrade;
        //    bool done = _tradesToLog.TryTake(out nextTrade);
        //    if (done)
        //    {
        //        _staffLogs.LogTrade(nextTrade);
        //        Console.WriteLine(
        //            $"Processing transaction from {nextTrade.Person.Name}");
        //    }
        //    else if (_workingDayComplete)
        //    {
        //        Console.WriteLine("No more sales to log - exiting");
        //        return;
        //    }
        //    else
        //    {
        //        Console.WriteLine("No transactions available");
        //        Thread.Sleep(500);
        //    }
        //}

        //// Alternative 1 of BlockingCollection Implementation
        //while (true)
        //{
        //    try
        //    {
        //        Trade nextTrade = _tradesToLog.Take();
        //        _staffLogs.LogTrade(nextTrade);
        //        Console.WriteLine(
        //            $"Processing transaction from {nextTrade.Person.Name}");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        // may throw exception `The collection argument is empty and has been marked as complete with regards to additions.` when `CompleteAdding()` being called
        //        // `BlockingCollection<T>.Take()` throws an exception if/when `CompleteAdding()` has been called.
        //        Console.WriteLine(ex.Message);
        //        return;
        //    }
        //}

        // Alternative 2 of BlockingCollection Implementation
        foreach (Trade nextTrade in _tradesToLog.GetConsumingEnumerable())
        {
            _staffLogs.LogTrade(nextTrade);
            Console.WriteLine("Processing transaction from " + nextTrade.Person.Name);
        }
    }
}