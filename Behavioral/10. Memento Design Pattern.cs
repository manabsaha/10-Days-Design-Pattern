// Memento Design Pattern - Behavioral Design Pattern

/* 

The Memento Design Pattern is a Behavioral Design Pattern that can restore an object to its previous 
state. This pattern is useful for scenarios where you need to perform an undo or rollback operation 
in your application. The Memento pattern captures an object’s internal state so that the object can 
be restored to this state later. It is especially useful when implementing undo functionality in an 
application.

The classes and objects participating in this pattern include:
1. Memento  (Memento)
- stores internal state of the Originator object. The memento may store as much or as little of the 
originator's internal state as necessary at its originator's discretion.
- protect against access by objects of other than the originator. Mementos have effectively two 
interfaces. Caretaker sees a narrow interface to the Memento -- it can only pass the memento to the 
other objects. Originator, in contrast, sees a wide interface, one that lets it access all the data 
necessary to restore itself to its previous state. Ideally, only the originator that produces the 
memento would be permitted to access the memento's internal state.
2. Originator  (SalesProspect)
- creates a memento containing a snapshot of its current internal state.
- uses the memento to restore its internal state
3. Caretaker  (Caretaker)
- is responsible for the memento's safekeeping
- never operates on or examines the contents of a memento.

Usage Examples:
When you need to provide an undo mechanism in applications like text editors, graphic editors, or 
more complex transactional systems.

*/

/*
Scenario:
---------
A man bought 42 inch old TV with no USB and placed in the hall. Now he wants to replace it to 46 
inch TV with USB support. He later bought a 50 inch TV and placed it in the hall. The old TVs are 
now in storeroom. Now he thought of putting back the 42 inch old TV to hall. So, basically, we are 
rollbacking to its previous state.
In this example, the Hall is the Originator where we will store the Memento Object, and the Store 
Room is the Caretaker which is keeping the Memento. Led TV is the Memento i.e., it is used to hold 
the internal state of LED TV.
*/

using System;
using System.Collections.Generic;

// Model
public class LEDTV
{
    public string Size { get; set; }
    public string Price { get; set; }
    public bool USBSupport { get; set; }

    public LEDTV(string Size, string Price, bool USBSupport) {
        this.Size = Size;
        this.Price = Price;
        this.USBSupport = USBSupport;
    }

    public string GetDetails() {
        return "LEDTV [Size=" + Size + ", Price=" + Price + ", USBSupport=" + USBSupport + "]";
    }
}

// Memento
public class Memento
{
    public LEDTV LedTV { get; set; }

    public Memento(LEDTV ledTV) {
        LedTV = ledTV;
    }

    public string GetDetails() {
        return "Memento [LedTV=" + LedTV.GetDetails() + "]";
    }
}

// Caretaker
public class Caretaker
{
    private List<Memento> LedTvList = new List<Memento>();

    public void AddMemento(Memento m) {
        LedTvList.Add(m);
        Console.WriteLine("LED TV's snapshots Maintained by CareTaker :" + m.GetDetails());
    }

    public Memento GetMemento(int index) {
        return LedTvList[index];
    }
}

// Originator
public class Originator
{
    public LEDTV LedTV;

    public Memento CreateMemento() {
        return new Memento(LedTV);
    }

    public void SetMemento(Memento memento) {
        LedTV = memento.LedTV;
    }

    public string GetDetails() {
        return "Originator [LEDTV=" + LedTV.GetDetails() + "]";
    }
}

// Main
public class Program
{
    public static void Main(string[] args)
    {        
        Originator originator = new Originator
        {
            LedTV = new LEDTV("42-Inch", "60000", false)
        };
        Caretaker caretaker = new Caretaker();
        
        Memento memento = originator.CreateMemento();
        caretaker.AddMemento(memento);
        originator.LedTV = new LEDTV("46-Inch", "80000", true);
        
        memento = originator.CreateMemento();  
        caretaker.AddMemento(memento);
        originator.LedTV = new LEDTV("50-Inch", "100000", true);
        
        Console.WriteLine("\nOrignator Current State : " + originator.GetDetails());
        Console.WriteLine("\nOriginator Restoring to 42-Inch LED TV");
        originator.SetMemento(caretaker.GetMemento(0));
        Console.WriteLine("\nOrignator Current State : " + originator.GetDetails());
        Console.ReadKey();
    }
}

/*
Advantages:
-----------
1. Undo Mechanism: Provides an undo mechanism by saving the object’s previous state.
2. Preserving Encapsulation: It does not violate the originator’s encapsulation, as only the originator 
can store and retrieve information from the memento.
3. Ease of Restoration: The originator can restore its state to a previous point in time.
*/