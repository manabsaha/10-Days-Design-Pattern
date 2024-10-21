// Proxy Design Pattern - Structural Design Pattern

/*

The Proxy design pattern allows us to introduce an extra layer of control over access to an 
object. This access control comes with the provision of a wrapper object (Proxy) that acts 
on behalf of the original object (RealSubject). Since the proxy mimics the real subject, it 
essentially needs to implement the same set of contracts (ISubject) as the real subject does.

The Proxy operates and serves the result to the client using the same interface (or abstract 
class) as the RealSubject does. Under the hood, it holds a reference to the realSubject instance.
During the operation(), the proxy delegates the call to realSubject.operation() before/after 
doing some other actions by itself.

It helps in incorporating additional logic without changing the functionality and intent of 
the original object.

*/

/* 
Scenario:
---------
Let’s imagine we are using a data service that fetches exchange rates from a remote API service.
And, we call this data service throughout the application whenever we need exchange-rates data.
Every time we call this data service, it sends a request to the remote service.

Now the problem is, that exchange rates usually do not change so frequently, and initiating a 
network communication for every such request is bad for performance. A wiser approach is to 
load data from the remote service once, cache it for a certain duration, and serve the cached 
data on subsequent requests.

However, client code shouldn’t be bothered about how data is provided and the data service 
shouldn’t be coupled to the caching layer. That means neither client nor the data service 
should be altered for this purpose.

A proxy service comes to the rescue! It can encapsulate all the logic that ties the caching 
layer to the data service layer while offering the same interface to the client code.
*/

using System;

// Model - 'Exchange Rate'
public class ExchangeRate
{
    public string CurrencyCode { get; }
    public decimal Rate { get; }

    public ExchangeRate(string currencyCode, decimal rate)
    {
        CurrencyCode = currencyCode;
        Rate = rate;
    }
}

// The contract - interface
public interface IExchangeRateService
{
    ExchangeRate[] GetExchangeRates();
}

// Real Subject 
public class ExchangeRateService : IExchangeRateService
{
    public ExchangeRate[] GetExchangeRates() {
        // In real-world application, this data comes from a remote API service
        ExchangeRate[] data = [
            new("CAD", 0.73m),
            new("EUR", 1.07m),
            new("GBP", 1.27m),
        ];

        Console.WriteLine("Fetched data from remote service");
        return data;   
    }
}

// Proxy Subject to the Real Subject - using same contract
public class CachedExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateService _exchangeRateService;
    private ExchangeRate[]? _exchangeRates;

    public CachedExchangeRateService()
    {
        _exchangeRateService = new ExchangeRateService();
    }

    public ExchangeRate[] GetExchangeRates()
    {
        if (_exchangeRates is null) {
            _exchangeRates = _exchangeRateService.GetExchangeRates();
            return _exchangeRates;
        }

        Console.WriteLine("Read data from cache");
        return _exchangeRates;
    }
}

// Main
public class Run 
{
    public static void Main (string[] args) {
        var service = new CachedExchangeRateService();
        for (int i = 1; i <= 3; i++) {
            Console.WriteLine($"Request {i}");
            _ = service.GetExchangeRates();
        }
    }
}

/*
Other Use Cases of Proxy Pattern:
---------------------------------
Logging proxy - around 3rd party classes
Protection proxy - around incoming data requests
Remote proxy - fault tolerance, error handling for remote services
Virtual Proxy - resource intrinsic services -> instantiation, caching and disposal 

Caveats of Proxy Pattern:
-------------------------
1. Since a proxy brings in an extra layer of abstraction, it might lead to additional complexity 
and maintenance overhead. The overhead is even more when there are more dependencies involved. 
For example, when the constructor dependencies in the actual subject get changed, we need relevant 
changes in the proxy class as well

2. Although the performance cost of an additional proxy layer might be negligible in many cases, 
it might be a significant concern for performance-critical applications. Virtual proxies are 
more prone to performance degradation due to improper lifecycle management.

3. Maintaining state synchronization between the real object and the proxy might be challenging 
too. For example, in the case of a caching proxy, it might get tricky to invalidate the cache 
and ensure that no stale data is served to the client.

4. Another potential drawback is leaky abstraction. This might happen for virtual and caching 
proxies if the client needs knowledge of lifecycle events and cache invalidation events.

5. Last but not least, while a protection proxy offers a convenient way of access control, relying 
solely on it for security is dangerous.
*/
