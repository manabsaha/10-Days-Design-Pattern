// Prototype Design Pattern - Creational DP

/*

The Prototype Design Pattern specifies the kind of objects to create using a prototypical instance and creates 
new objects by copying this prototype. The Prototype Design Pattern creates objects by copying an existing 
object, known as the prototype. 

To simplify the above definition, we can say that the Prototype Design Pattern gives us a way to create new or 
cloned objects from the existing object of a class. That means it clones the existing object with its data into 
a new object. If we make any changes to the cloned object (i.e., new object), it does not affect the original object.

This pattern is useful when the creation of an object is time-consuming or complex in terms of resources. 
Instead of going through the costly process of creating a fresh object from scratch, an existing instance 
serves as a prototype, and new objects can be created by copying this prototype.

All prototype classes should have a common interface that makes it possible to copy objects even if their 
concrete classes are unknown. Prototype objects can produce full copies since objects of the same class 
can access each other’s private fields.

*/

using System;

// Object Model 1 - 'Person'.
public class Person
{
    public int Age;
    public DateTime BirthDate;
    public string Name;
    public IdInfo IdInfo;

    public Person ShallowCopy() {
        return (Person) this.MemberwiseClone();
    }

    public Person DeepCopy() {
        Person clone = (Person) this.MemberwiseClone();
        clone.IdInfo = new IdInfo(IdInfo.IdNumber);
        clone.Name = String.Copy(Name);
        return clone;
    }
}

// Object Model 2 - 'Id info'.
public class IdInfo
{
    public int IdNumber;

    public IdInfo(int idNumber) {
        this.IdNumber = idNumber;
    }
}

// Main
public class Run
{
    public static void Main(string[] args) {
        Person p1 = new Person();
        p1.Age = 42;
        p1.BirthDate = Convert.ToDateTime("1977-01-01");
        p1.Name = "Jack";
        p1.IdInfo = new IdInfo(666);

        // Perform a shallow copy of p1 and assign it to p2.
        Person p2 = p1.ShallowCopy();
        // Make a deep copy of p1 and assign it to p3.
        Person p3 = p1.DeepCopy();

        // Display values of p1, p2 and p3.
        Console.WriteLine("Original values of p1, p2, p3:");
        Console.WriteLine("p1 instance values:"); DisplayValues(p1);
        Console.WriteLine("p2 instance values:"); DisplayValues(p2);
        Console.WriteLine("p3 instance values:"); DisplayValues(p3);

        // Change the value of p1 properties and display the values of p1, p2 and p3.
        p1.Age = 32;
        p1.BirthDate = Convert.ToDateTime("1900-01-01");
        p1.Name = "Frank";
        p1.IdInfo.IdNumber = 7878;

        Console.WriteLine("\nValues of p1, p2 and p3 after changes to p1:");
        Console.WriteLine("p1 instance values: "); DisplayValues(p1);
        Console.WriteLine("p2 instance values (reference values have changed):"); DisplayValues(p2);
        Console.WriteLine("p3 instance values (everything was kept the same):"); DisplayValues(p3);
    }

    public static void DisplayValues(Person p) {
        Console.WriteLine("Name: {0:s}, Age: {1:d}, BirthDate: {2:MM/dd/yy}", p.Name, p.Age, p.BirthDate);
        Console.WriteLine("ID#: {0:d}", p.IdInfo.IdNumber);
    }
}

/*
Shallow vs. Deep Copy:

Shallow Copy: The MemberwiseClone method in C# performs a shallow copy. It copies the values of the fields 
of an object to a new object. If the field is a value type, a bit-by-bit copy of the field is performed. 
For reference types, the reference is copied, not the object itself.

Deep Copy: If your object has reference-type fields, you might need to implement deep cloning to avoid 
shared references in your cloned objects. Deep cloning involves creating copies of the objects referenced 
by the fields.

*/