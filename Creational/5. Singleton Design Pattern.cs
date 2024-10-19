// Singleton Design Pattern - Creational DP

/*

The Singleton is a creational design pattern that allows us to create a single instance of an object and to 
share that instance with all the users that require it. There is a common opinion that the Singleton pattern 
is not recommended because it presents a code smell, but there are some cases where it fits perfectly.

For example, some components have no reason to be instanced more than once in a project. Take a logger for 
example. It is quite common to register logger class as a singleton component because all we have to do is 
to provide a string to be logged and the logger is going to write it to the file. Then multiple classes may 
require to write in the same file at the same time from different threads, so having one centralized place 
for that purpose is always a good solution.

A proper singleton class is going to be a thread-safe which is a crucial requirement when implementing a 
Singleton pattern.

-----
We are going to start with a simple console application in which we are going to read all the data from a 
file (which consists of cities with their population) and then use that data.

*/

using System;
using System.Collections.Generic;
using System.IO;

public interface ISingletonContainer
{
    int GetPopulation(string name);
}

public class SingletonDataContainer : ISingletonContainer 
{
    private Dictionary<string, int> _capitals = new Dictionary<string, int>();

    private SingletonDataContainer() {
        Console.WriteLine("Initializing singleton object");

        //var elements = File.ReadAllLines("capitals.txt");
		string[] elements = {"Washington, D.C.", "5"};
        for (int i = 0; i < elements.Length; i+=2)
        {
            _capitals.Add(elements[i], int.Parse(elements[i + 1]));
        }
    }

    public int GetPopulation(string name) {
        return _capitals[name];
    }

    private static Lazy<SingletonDataContainer> instance = new Lazy<SingletonDataContainer>(
        () => new SingletonDataContainer());
        
    public static SingletonDataContainer Instance => instance.Value;
}

/*
Here, our implementation is ideal. It is thread-safe singleton.
Let’s construct our object the non-lazy way.
We will modify our class to implement a non-thread-safe Singleton.

(Line 52 & 55)
private static SingletonDataContainer instance = new SingletonDataContainer();

public static SingletonDataContainer Instance => instance;
*/

public class Run
{
    public static void Main(string[] args) {
        var instance = SingletonDataContainer.Instance;
        var instance2 = SingletonDataContainer.Instance;
        var instance3 = SingletonDataContainer.Instance;
        var instance4 = SingletonDataContainer.Instance;
        /* We can see that we are calling our instance four times but it is initialized only 
        once, which is exactly what we want. */
        Console.WriteLine(instance.GetPopulation("Washington, D.C."));
    }
}