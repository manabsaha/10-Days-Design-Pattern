// Mediator Design Pattern - Behavioral Design Pattern

/*

It defines an object that encapsulates how a set of objects interact with each other. Mediator promotes 
loose coupling by keeping objects from explicitly referring to each other and letting you vary their 
interaction independently. The Mediator Design Pattern reduces the communication complexity between 
multiple objects. 

The Mediator Design Pattern restricts direct communications between the objects and forces them to 
collaborate only via a mediator object. This pattern is used to centralize complex communications and 
control between related objects in a system. The Mediator object acts as the communication center for 
all objects.

It has 4 Components:
1. Mediator
2. Concrete Mediator
3. Colleague
4. Concrete Colleague(s)

Usage examples: 
The most popular usage of the Mediator pattern in C# code is facilitating communications between GUI 
components of an app. The synonym of the Mediator is the Controller part of MVC pattern.

*/

/*
Scenario:
---------
On Facebook, there is a group called Dot Net Tutorials, and in that group, many people are joined. Ram is 
sharing some messages in the group. Then, what this Facebook Group will do is it will send that message to 
all the members who have joined this group. So, here, the Facebook group is acting as a Mediator.
*/

using System;
using System.Collections.Generic;

// Mediator
public interface IFacebookGroupMediator
{
    void SendMessage(string msg, User user);
    void RegisterUser(User user);
}

// Concrete Mediator
public class ConcreteFacebookGroupMediator : IFacebookGroupMediator
{
    private List<User> UsersList = new List<User>();

    public void RegisterUser(User user)
    {
        UsersList.Add(user);
        user.Mediator = this;
    }

    public void SendMessage(string message, User user)
    {
        foreach (User u in UsersList)
        {
            if (u != user)
            {
                u.Receive(message);
            }
        }
    }
}

// Colleague
public abstract class User
{
    protected string Name;
    public IFacebookGroupMediator Mediator { get; set; }
    
    public User(string name) {
        this.Name = name;
    }
    
    public abstract void Send(string message);
    public abstract void Receive(string message);
}

// Concrete Colleague
public class ConcreteUser : User
{
    public ConcreteUser(string Name) : base(Name) { }

    public override void Receive(string message) {
        Console.WriteLine(this.Name + ": Received Message:" + message);
    }

    public override void Send(string message) {
        Console.WriteLine(this.Name + ": Sending Message=" + message + "\n");
        Mediator.SendMessage(message, this);
    }
}

// Main
class Program
{
    static void Main(string[] args)
    {
        //Create an Instance of Mediator i.e. Creating a Facebook Group
        IFacebookGroupMediator facebookMediator = new ConcreteFacebookGroupMediator();

        User Joe = new ConcreteUser("Joe");
        User Dave = new ConcreteUser("Dave");
        User Smith = new ConcreteUser("Smith");
        User John = new ConcreteUser("John");

        facebookMediator.RegisterUser(Joe);
        facebookMediator.RegisterUser(Dave);
        facebookMediator.RegisterUser(Smith);
        facebookMediator.RegisterUser(John);

        Dave.Send("dotnettutorials.net - this website is very good to learn Design Pattern");
        Console.WriteLine();

        Joe.Send("What is Design Patterns? Please explain ");
    }
}

/*
Advantages:
-----------
Reduced Complexity: It centralizes complex communications and controls logic between objects 
in a system.
Decoupled Objects: Colleague objects are less coupled to each other, which increases maintainability 
and reusability.
*/