// Facated Builder Pattern - Creational DP

/*

Sometimes we may have a complex object, and the creational process requires more than one builder class. 
So, what we need to do is to introduce multiple builder classes in such a way, that we can jump from one 
builder to another while creating our object.

The faceted Builder approach helps us a lot in that process because we create a facade over our builders 
and it allows us to use all the builders to create a single object.

*/

using System;

// Object Model
internal class Car
{
    public string Type { get; set; }
    public string Color { get; set; }
    public int NumberOfDoors { get; set; }

    public string City { get; set; }
    public string Address { get; set; }

    public override string ToString()
    {
        return $"CarType: {Type}, Color: {Color}, Number of doors: {NumberOfDoors}, " + 
            $"Manufactured in {City}, at address: {Address}";
    }
} 

// Facade Builder
internal class CarBuilderFacade
{
    protected Car Car { get; set; }

    public CarBuilderFacade() {
        Car = new Car();
    }

    public Car Build() => Car;

    // Exposed our builders inside the facade class.
    public CarInfoBuilder Info => new CarInfoBuilder(Car);
    public CarAddressBuilder Built => new CarAddressBuilder(Car);
}

// 1st Builder class - 'info'
internal class CarInfoBuilder : CarBuilderFacade {
    public CarInfoBuilder(Car car) {
        Car = car;
    }
	
    public CarInfoBuilder WithType(string type) {
        Car.Type = type;
        return this;
    }
    public CarInfoBuilder WithColor(string color) {
        Car.Color = color;
        return this;
    }
    public CarInfoBuilder WithNumberOfDoors(int number) {
        Car.NumberOfDoors = number;
        return this;
    }
}

// 2nd Builder class - 'address'
internal class CarAddressBuilder : CarBuilderFacade {
    public CarAddressBuilder (Car car) {
        Car = car;
    }

    public CarAddressBuilder InCity(string city) {
        Car.City = city;
        return this;
    }
    public CarAddressBuilder AtAddress(string address) {
        Car.Address = address;
        return this;
    }
}

// Main
public class Run {
	public static void Main() {
        // using Facade Builder
		var car = new CarBuilderFacade()
            .Info
              .WithType("BMW")
              .WithColor("Black")
              .WithNumberOfDoors(5)
            .Built
              .InCity("Leipzig")
              .AtAddress("Some address 254")
            .Build();
        
        Console.WriteLine(car);
	}
}