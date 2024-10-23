// Flyweight Design Pattern - Structural Design Pattern

/*

Flyweight is a structural design pattern that allows programs to support vast quantities of objects 
by keeping their memory consumption low.

The pattern achieves it by sharing parts of object state between multiple objects. In other words, 
the Flyweight saves RAM by caching the same data used by different objects.
Flyweight can be recognized by a creation method that returns cached objects instead of creating new.

In Flyweight Design Pattern, there are two states, i.e., Intrinsic and Extrinsic.
Intrinsic states are things that are Constants and Stored in Memory. On the other hand, Extrinsic 
states are things that are not constant and need to be calculated on the Fly; hence, they can not be 
stored in memory.
Eg: Shape can be constant, color of that shape object can be varied.

*/

/*
Scenario:
---------
Suppose in your application you want to create 240000 circle objects with different colors.
The following are the three components involved in the Flyweight Design Pattern.
1. Flyweight
2. ConcreteFlyweight
3. FlyweightFacory
*/

using System;
using System.Collections.Generic;

// Flyweight Interface: It defines the members of the flyweight objects.
public interface IShape
{
    void Draw();
}

// ConcreteFlyweight: Inherits from the Flyweight Interface.
public class Circle : IShape
{
    public string Color { get; set; }

    //The following Properties Values are going to be the same for all Circle Shape Object
    private readonly int XCor = 10;
    private readonly int YCor = 20;
    private readonly int Radius = 30;

    public void SetColor(string Color) {
        this.Color = Color;
    }

    public void Draw() {
        Console.WriteLine(" Circle: Draw() [Color : " + Color + ", X Cor : " + XCor + ", YCor :" 
            + YCor + ", Radius :" + Radius);
    }
}


// FlyweightFacory
// This is a factory class used to create concrete objects of the ConcreteFlyweight type
public class ShapeFactory
{
    //The Following Dictionary is going to act as our Cache Memory
    private static Dictionary<string, IShape> shapeMap = new Dictionary<string, IShape>();

    public static IShape GetShape(string shapeType) {
        IShape shape = null;
        if (shapeType.Equals("circle", StringComparison.InvariantCultureIgnoreCase)) {
            if (shapeMap.TryGetValue("circle", out shape)) { }
            // If the key shapeType i.e. circle is stored in the Cache, then return value of it.
            else {
                shape = new Circle();
                shapeMap.Add("circle", shape);
                Console.WriteLine(" Creating circle object with out any color in shapefactory \n");
            }
        }
        return shape;
    }
}

// Main
class Program
{
    static void Main(string[] args)
    {
        //Creating Circle Objects with Red Color
        Console.WriteLine("\n Red color Circles ");
        for (int i = 0; i < 3; i++)
        {
            Circle circle = (Circle)ShapeFactory.GetShape("circle");
            circle.SetColor("Red");
            circle.Draw();
        }

        //Creating Circle Objects with Green Color
        Console.WriteLine("\n Green color Circles ");
        for (int i = 0; i < 3; i++)
        {
            Circle circle = (Circle)ShapeFactory.GetShape("circle");
            circle.SetColor("Green");
            circle.Draw();
        }

        //Creating Circle Objects with Blue Color
        Console.WriteLine("\n Blue color Circles");
        for (int i = 0; i < 3; ++i)
        {
            Circle circle = (Circle)ShapeFactory.GetShape("circle");
            circle.SetColor("Green");
            circle.Draw();
        }

        //Creating Circle Objects with Orange Color
        Console.WriteLine("\n Orange color Circles");
        for (int i = 0; i < 3; ++i)
        {
            Circle circle = (Circle)ShapeFactory.GetShape("circle");
            circle.SetColor("Orange");
            circle.Draw();
        }

        //Creating Circle Objects with Black Color
        Console.WriteLine("\n Black color Circles");
        for (int i = 0; i < 3; ++i)
        {
            Circle circle = (Circle)ShapeFactory.GetShape("circle");
            circle.SetColor("Black");
            circle.Draw();
        }
    }
}
// Only once, circle is created in the factory. Rest it is cached.

/* 
Advantages:
-----------
Reduced Memory Footprint: Significantly reduces memory usage when dealing with large numbers 
of similar objects.
Improved Performance: This can improve performance in systems where object instantiation and 
garbage collection are bottlenecks.
*/