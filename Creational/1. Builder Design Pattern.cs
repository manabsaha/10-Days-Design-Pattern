// Builder Design Pattern and Fluent Builder

/*

The Builder design pattern is a 'creational design pattern' that lets us create an object one step at a time. 
It is quite common to use this pattern when creating a complex object. By using this pattern, we can create 
different parts of an object, step by step, and then connect all the parts together.

Without this pattern, we can end up with a large constructor to provide all the required parameters for 
constructing our object. That could lead to quite unreadable and hardly maintainable code. Furthermore, a 
constructor with lots of parameters has a downside to it. We won’t need to use all the parameters, all the 
time. Also, if we add new paramters, it will break the exisiting constructor implementation.

*/

using System;

// Object Model
internal class House {
    public int stories {get; set;}
    public string roofType {get; set;}
    public string doorType {get; set;}

    public override string ToString() => 
        $"Stories: {stories}, RoofType: {roofType}, DoorType: {doorType}"
}

// Builder class
internal class HouseBuilder {
    private House _house = new House();
	
    public HouseBuilder SetStories(int stories) {
        _house.stories = stories;
        return this;
    }

    public HouseBuilder SetRoofType(string roofType) {
        _house.roofType = roofType;
        return this;
    }

    public HouseBuilder SetDoorType(string doorType) {
        _house.doorType = doorType;
        return this;
    }

    public House Build() {
        return _house;
    }
}

// Director class
/* It encapsulates the building process from the client class inside a Director class. */
internal class HouseDirector {
    private HouseBuilder _houseBuilder = new HouseBuilder();

    public House BuildOneStoreyHouse() {
        return _houseBuilder.SetStories(1).SetDoorType("single").SetRoofType("pointy").Build();
    }

    public House BuildTwoStoreyHouse() {
        return _houseBuilder.SetStories(2).SetDoorType("double").SetRoofType("flat").Build();
    }
}

// Main
public class Run {
	public static void Main() {
        // using Builder
		House house = new HouseBuilder().SetStories(1)
			.SetDoorType("single").SetRoofType("plain")
			.Build();
		Console.WriteLine("House => " + house.ToString());

        // using Director
        HouseDirector houseDirector = new HouseDirector();
        House oneStorey = houseDirector.BuildOneStoreyHouse();
        House twoStorey = houseDirector.BuildTwoStoreyHouse();
        Console.WriteLine("House 1 => " + oneStorey.ToString());
        Console.WriteLine("House 2 => " + twoStorey.ToString());
	}
}