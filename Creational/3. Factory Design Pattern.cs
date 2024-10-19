// Factory Design Pattern - Creational DP

/*

The Factory method is a creational design pattern that provides an interface for creating objects without 
specifying their concrete classes. It defines a method that we can use to create an object instead of using 
its constructor. The important thing is that the subclasses can override this method and create objects of 
different types.

We use the Factory method refactoring technique to hide the constructor and use our own method to expose it.

To implement a Factory method pattern, we are going to create a simple Air conditioner application. Our app 
will receive an input from a user and based on that input will trigger a required action (cooling or 
warming the room). 

*/

using System;
using System.Collections.Generic;

// interface for AC
public interface IAirConditioner
{
    void Operate();
}

// Cooling Manager - 'Feature'
public class CoolingManager : IAirConditioner
{
    private readonly double _temperature;

    public CoolingManager(double temperature) {
        _temperature = temperature;
    }

    public void Operate() {
        Console.WriteLine($"Cooling the room to the required temperature of {_temperature} degrees");
    }
}

// Warming Manager - 'Feature'
public class WarmingManager : IAirConditioner
{
    private readonly double _temperature;

    public WarmingManager(double temperature) {
        _temperature = temperature;
    }

    public void Operate() {
        Console.WriteLine($"Warming the room to the required temperature of {_temperature} degrees.");
    }
}

// Base functionality preparation done till here.
// We will now create factory creator for this objects.

public abstract class AirConditionerFactory
{
    public abstract IAirConditioner Create(double temperature);
}

public class CoolingFactory : AirConditionerFactory
{
    public override IAirConditioner Create(double temperature) => new CoolingManager(temperature);
}

public class WarmingFactory : AirConditionerFactory
{
    public override IAirConditioner Create(double temperature) => new WarmingManager(temperature);
}

// Now we are ready to start using our Factory methods.
// Factory execution starts here.

// Actions
public enum Actions
{
    Cooling,
    Warming
}

// Adding all factories
public class AirConditioner
{
    private readonly Dictionary<Actions, AirConditionerFactory> _factories;

    public AirConditioner() {
        _factories = new Dictionary<Actions, AirConditionerFactory> {
            { Actions.Cooling, new CoolingFactory() },
            { Actions.Warming, new WarmingFactory() }
        };
    }

    public static AirConditioner InitializeFactories() => new AirConditioner();
    public IAirConditioner ExecuteCreation(Actions action, double temperature) =>_factories[action].Create(temperature);
}
// instead of adding one by one, we can use reflection extension as well.

// Main
public class Run
{
    public static void Main(string[] args) {
        AirConditioner
            .InitializeFactories()
            .ExecuteCreation(Actions.Cooling, 22.5)
            .Operate();
    }
}