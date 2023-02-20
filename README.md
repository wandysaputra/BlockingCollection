# Blocking Collection

## [IProducerConsumerCollection](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.iproducerconsumercollection-1?view=net-7.0)

|| ConcurrentQueue | ConcurrentStack | ConcurrentBag |
|:--|:--|:--|:--|
|Order|First-in-First-out| Last-in-First-out | Order is unspecified <br/>Based on performance. <br/> [Effecient if same threads are adding and removing.](https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool#:~:text=The%20ConcurrentBag%3CT%3E%20is%20used%20to%20store%20the%20objects%20because%20it%20supports%20fast%20insertion%20and%20removal%2C%20especially%20when%20the%20same%20thread%20is%20both%20adding%20and%20removing%20items.)|
|Add Item|Enqueue() | Push() | Add()|
|Remove Item | TryDequeue()|TryPop()|TryTake()|


## [BlockingCollection](https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview)
To avoing polling we can use `BlockingCollection` 
> Multiple threads or tasks can add items to the collection concurrently, and if the collection reaches its specified maximum capacity, the producing threads will block until an item is removed. Multiple consumers can remove items concurrently, and if the collection becomes empty, the consuming threads will block until a producer adds an item. A producing thread can call `CompleteAdding` to indicate that no more items will be added. Consumers monitor the `IsCompleted` property to know when the collection is empty and no more items will be added.

### GetConsumingEnumerable 

> `BlockingCollection<T>` provides a GetConsumingEnumerable method that enables consumers to use `foreach` (`For Each` in Visual Basic) to remove items until the collection is completed, which means it is empty and no more items will be added.
