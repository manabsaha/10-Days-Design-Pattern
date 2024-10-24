// Command Design Pattern - Behavioral Design Pattern

/*

The Command pattern is a behavioral design pattern that we can use to turn a request into an object 
that contains all the information about the request.

The Command design pattern is quite popular in C#, especially when we want to delay or queue a 
request’s execution or when we want to keep track of our operations. Furthermore, this possibility 
to keep track of our operations gives us the opportunity to undo them as well.

The Command design pattern consists of the Invoker class, Command class/interface, Concrete command 
classes, and the Receiver class.

Usage examples: The Command pattern is pretty common in C# code. Most often it’s used as an alternative 
for callbacks to parameterizing UI elements with actions. It’s also used for queueing tasks, tracking 
operations history, etc.

Identification: The Command pattern is recognizable by behavioral methods in an abstract/interface 
type (sender) which invokes a method in an implementation of a different abstract/interface type (receiver) 
which has been encapsulated by the command implementation during its creation. Command classes are usually 
limited to specific actions.

*/

/*
Scenario:
---------
We are going to write a simple app in which we are going to modify the price of the product.
It can increase and decrease the price of the product.
*/

using System;
using System.Linq;
using System.Collections.Generic;

// Receiver class 
public class Product 
{
    public string Name {get; set;}
    public int Price {get; set;}
	
	public Product(string name, int price) {
        Name = name;
        Price = price;
    }

    public void IncreasePrice(int amount) {
        Price += amount;
        Console.WriteLine($"The price for the {Name} has been increased by {amount}$.");
    }

    public bool DecreasePrice(int amount) {
        if(amount < Price) {
            Price -= amount;
            Console.WriteLine($"The price for the {Name} has been decreased by {amount}$.");
            return true;
        }
        return false;
    }

    public override string ToString() => $"Current price for the {Name} product is {Price}$.";
}

/* Now the Client class can instantiate the Product class and execute the required actions. But 
the Command design pattern states that we shouldn’t use receiver classes directly. Instead, we 
should extract all the request details into a special class – Command. */

// Command Interface
public interface ICommand
{
    public void ExecuteAction();
    public void UndoAction();
}

public enum PriceActions 
{
    Increase,
    Decrease
}

// Concrete Command
public class ProductCommand : ICommand
{
    private readonly Product _product;
    private readonly PriceActions _priceAction;
    private readonly int _amount;
    public bool IsCommandExecuted { get; private set; }

    public ProductCommand(Product product, PriceActions priceAction, int amount) {
        _product = product;
        _priceAction = priceAction;
        _amount = amount;
    }

    public void ExecuteAction() {
        if(_priceAction == PriceActions.Increase) {
            _product.IncreasePrice(_amount);
            IsCommandExecuted = true;
        }
        else {
            IsCommandExecuted = _product.DecreasePrice(_amount);
        }
    }

    public void UndoAction() {
        if(!IsCommandExecuted) {
            return;
        }
        
        if (_priceAction == PriceActions.Increase) {
            _product.DecreasePrice(_amount);
        }
        else {
            _product.IncreasePrice(_amount);
        }
    }
}   

// Invoker class 
public class ModifyPrice 
{
    private List<ICommand> _commands;
    private ICommand _command;

    public ModifyPrice() {
        _commands = new List<ICommand>();
    }

    public void SetCommand(ICommand command) {
        _command = command;
    }

    public void Invoke() {
        _commands.Add(_command);
        _command.ExecuteAction();
    }

    public void UndoActions() {
        foreach (var command in Enumerable.Reverse(_commands)) {
            command.UndoAction();
        }
    }
}

// Main
public class Run 
{
    public static void Main(string[] args) {
        var modifyPrice = new ModifyPrice();
        var product = new Product("Phone", 500);
        Execute(product, modifyPrice, new ProductCommand(product, PriceActions.Increase, 100));
          
        Execute(product, modifyPrice, new ProductCommand(product, PriceActions.Increase, 50));
        Execute(product, modifyPrice, new ProductCommand(product, PriceActions.Decrease, 25));
        Console.WriteLine(product);
        Console.WriteLine();
        modifyPrice.UndoActions();
        Console.WriteLine(product);
    }

    private static void Execute(Product product, ModifyPrice modifyPrice, ICommand productCommand) {
        modifyPrice.SetCommand(productCommand);
        modifyPrice.Invoke();
    }
}

/* Even though the Command design pattern introduces complexity to our code, it can be very useful.
With it, we can decouple classes that invoke operations from classes that perform these operations. 
Additionally, if we want to introduce new commands, we don’t have to modify existing classes. Instead, 
we can just add those new command classes to our project. */