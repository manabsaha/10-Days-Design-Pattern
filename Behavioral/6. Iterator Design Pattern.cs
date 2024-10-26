// Iterator Design Pattern - Behavioral Design Pattern

/*

The Iterator design pattern provides a way to access the elements of an aggregate object sequentially 
without exposing its underlying representation. 

The classes and objects participating in this pattern include:

1. Iterator  (Abstract Iterator) - defines an interface for accessing and traversing elements.
2. ConcreteIterator  (Iterator) - implements the Iterator interface. Keeps track of the current position 
in the traversal of the aggregate.
3. Aggregate  (Abstract Collection) - defines an interface for creating an Iterator object.
4.ConcreteAggregate  (Collection) - implements the Iterator creation interface to return an instance of 
the proper ConcreteIterator.

Usage examples: 
The pattern is very common in C# code. Many frameworks and libraries use it to provide a standard way 
for traversing their collections.

Identification: 
Iterator is easy to recognize by the navigation methods (such as next, previous and others). Client 
code that uses iterators might not have direct access to the collection being traversed.

*/

using System;
using System.Collections.Generic;

// Abstract Aggregate
public abstract class Aggregate
{
    public abstract Iterator CreateIterator();
}

// Abstract Iterator
public abstract class Iterator
{
    public abstract object First();
    public abstract object Next();
    public abstract bool IsDone();
    public abstract object CurrentItem();
}

// Concrete Aggregate
public class ConcreteAggregate : Aggregate
{
    List<object> items = new List<object>();

    public override Iterator CreateIterator() {
        return new ConcreteIterator(this);
    }

    public int Count {
        get { return items.Count; }
    }

    // Indexer
    public object this[int index]
    {
        get { return items[index]; }
        set { items.Insert(index, value); }
    }
}

// Concrete Iterator
public class ConcreteIterator : Iterator
{
    ConcreteAggregate aggregate;
    int current = 0;

    public ConcreteIterator(ConcreteAggregate aggregate) {
        this.aggregate = aggregate;
    }

    public override object First() {
        return aggregate[0];
    }

    public override object Next() {
        object ret = null;
        if (current < aggregate.Count - 1) {
            ret = aggregate[++current];
        }
        return ret;
    }

    public override object CurrentItem() {
        return aggregate[current];
    }

    public override bool IsDone() {
        return current >= aggregate.Count;
    }
}

// Main
public class Program
{
    public static void Main(string[] args) {
        ConcreteAggregate a = new ConcreteAggregate();
        a[0] = "Item A";
        a[1] = "Item B";
        a[2] = "Item C";
        a[3] = "Item D";

        // Create Iterator and provide aggregate
        Iterator i = a.CreateIterator();
        Console.WriteLine("Iterating over collection:");
        object item = i.First();
        while (item != null) {
            Console.WriteLine(item);
            item = i.Next();
        }
    }
}