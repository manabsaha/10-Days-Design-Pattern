// Abstract Factory Design Pattern - Creational DP

/*

The Abstract Factory design pattern is a creational pattern that handles object instantiations. It offers a 
framework to manage complex object creation scenarios and helps us achieve a more maintainable code structure.

The Abstract Factory design pattern enables clients to use an abstract interface to generate a family of 
related products without knowing about the concrete products this interface produces. This is one of the 
core creational design patterns.

By encapsulating this creation of related objects, we decouple the client code from the actual implementations 
of the objects.

Inheritance: Abstract Factory --> Concrete Factory --> Concrete Product --> Abstract Product

The Abstract Factory is an interface that declares a set of abstract creation methods. It does not specify 
details about concrete products but rather focuses on structure. A Concrete Factory is an implementation of 
the Abstract Factory interface, which we use to create concrete product objects.

The Abstract Product is an interface that defines the structure that the concrete product should have. In 
turn, a Concrete Product represents an implementation of the Abstract Product.

Finally, the Client is the class that uses the Abstract Factory and Abstract Product interfaces. However, it 
is decoupled from any specific variant of client products it gets from the factory.

---
Let’s consider a scenario where we want to implement a theme park. The theme park has different sections – 
for instance, Adventure and Fantasy. Each section has its own type of ride and show.

*/

using System;

// Let’s try to implement this without using the Abstract Factory pattern. 

public class AdventureRide
{
    public void Start() {
        Console.WriteLine("Starting the Adventure Ride.");
    }
}
public class AdventureShow
{
    public void Begin() {
        Console.WriteLine("Beginning the Adventure Show.");
    }
}
public class FantasyRide
{
    public void Start() {
        Console.WriteLine("Starting the Fantasy Ride.");
    }
}
public class FantasyShow
{
    public void Begin() {
        Console.WriteLine("Beginning the Fantasy Show.");
    }
}

public class ThemeParkClient
{
    public void EnjoyThemePark(string section) {
        if (section == "Adventure") {
            var ride = new AdventureRide();
            var show = new AdventureShow();
            ride.Start();
            show.Begin();
        }
        else if (section == "Fantasy") {
            var ride = new FantasyRide();
            var show = new FantasyShow();
            ride.Start();
            show.Begin();
        }
    }
}

/*
Here, the EnjoyThemePark() method calls the appropriate ride and show methods depending on the section of 
the theme park. With this, we have completed our theme park.

However, while this setup works fine for our current requirement, we start seeing problems as soon as 
there is talk of park expansion.

Our code violates the open/closed principle. Let’s assume we want to add a new “Sci-Fi” section to our 
theme park. We’ll need to modify the ThemeParkClient class to handle the new rides and shows, but the 
open/closed principle states that we should only be extending the class, not modifying it.  

The ThemeParkClient class is tightly coupled to the concrete ride and show classes, e.g. FantasyRide 
and AdventureShow. This makes it difficult for us to update these classes without modifying the client code.
*/

// Transition to the Abstract Factory Pattern

// using System;

// Abstract product
public interface IRide
{
    void Start();
}

// Abstract product
public interface IShow
{
    void Begin();
}

/// <summary>
/// Concrete Product - 'Adventure' starts
/// </summary>
public class AdventureRide : IRide 
{
    public void Start() {
        Console.WriteLine("Starting the Adventure Ride.");
    }
}

public class AdventureShow : IShow 
{
    public void Begin() {
        Console.WriteLine("Starting the Adventure Show.");
    }
}
/// <summary>
/// Concrete Product - 'Adventure' ends
/// </summary>

/// <summary>
/// Concrete Product - 'Fantasy' starts
/// </summary>
public class FantasyRide : IRide 
{
    public void Start() {
        Console.WriteLine("Starting the Fantasy Ride.");
    }
}

public class FantasyShow : IShow 
{
    public void Begin() {
        Console.WriteLine("Starting the Fantasy Show.");
    }
}
/// <summary>
/// Concrete Product - 'Fantasy' ends
/// </summary>

// Factory Interface
public interface IThemeParkFactory
{
    IRide CreateRide();
    IShow CreateShow();
}

// Factory class = 'Adventure'
public class AdventureThemeParkFactory : IThemeParkFactory 
{
    public IRide CreateRide() {
        return new AdventureRide();
    }

    public IShow CreateShow() {
        return new AdventureShow();
    }
}

// Factory class - 'Fantasy'
public class FantasyThemeParkFactory : IThemeParkFactory 
{
    public IRide CreateRide() {
        return new FantasyRide();
    }

    public IShow CreateShow() {
        return new FantasyShow();
    }
}

// Abstract Factory for both
public class ThemeParkClientNew
{
    private readonly IRide _ride;
    private readonly IShow _show;

    public ThemeParkClientNew(IThemeParkFactory factory) {
        _ride = factory.CreateRide();
        _show = factory.CreateShow();
    }

    public void EnjoyThemePark() {
        _ride.Start();
        _show.Begin();
    }
}

// Main
public class Run 
{
    public static void Main() {
        ThemeParkClientNew themeParkClientNew1 = new ThemeParkClientNew(new FantasyThemeParkFactory());
        themeParkClientNew1.EnjoyThemePark();
        
        ThemeParkClientNew themeParkClientNew2 = new ThemeParkClientNew(new AdventureThemeParkFactory());
        themeParkClientNew2.EnjoyThemePark();
    }
}

/*
---------
Benefits =>
First, the Abstract Factory encapsulates the creation of related objects, allowing clients to use families of objects 
without knowing about or being tied to the specific implementations. This in turn promotes modular and reusable code.

In addition, we define a common interface for the creation of related objects. Thus, we ensure that the objects produced 
by a factory are compatible and work seamlessly together. This consistency simplifies maintenance.

Finally, this pattern adheres to the open/closed design principle. Thus, we can introduce new variants of products through 
the creation of new concrete factories, without modifying existing client code.
---------
Drawbacks =>
Implementing the Abstract Factory pattern involves defining multiple interfaces and their concrete implementations. 
This can make the code complex and difficult to understand, especially for someone new to the codebase. The complexity 
keeps growing as the number of product variants grows.

Also, as we are adding layers of abstraction, it might introduce some runtime overhead. While minimal, this overhead 
might be something to consider in a performance-sensitive application.

Lastly, while it’s easy to add a new family of products using the Abstract Factory pattern, extending existing families 
can be comparatively challenging.
---------

We need to ensure that our abstract factory and product interfaces are well-defined. As these interfaces are the base 
of our design, they should encapsulate the necessary methods to create the product objects. This also includes grouping 
related products into families and defining interfaces for each product in the family. This makes sure that the client 
can use products from different families interchangeably.

*/