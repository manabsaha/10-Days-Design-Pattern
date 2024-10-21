// Decorator Design Pattern - Structural Design Pattern

/*

A Decorator is a structural design pattern that allows us to extend the behavior of objects by 
placing these objects into a special wrapper class. The Decorator design pattern is quite popular 
in C# due to the fact that it helps us dynamically add behaviors to the wrapped objects.

The structure of this pattern consists of a 'Component class' and a 'Concrete Component class' from one 
part and a 'Decorator class' and a 'Concrete Decorator class' on the other side. The Concrete Decorator 
class is going to add additional behavior to our Concrete Component.

So, when do we use this pattern?
Well, we should use this pattern when we have a need to add additional behavior to our objects. 
Furthermore, we should use it when it is too complicated to use inheritance or when it doesn’t make 
sense at all (too many inherit layers, urge to modify existing inheritance hierarchy by adding some 
additional layers, etc.).

*/

/*
Scenario:
---------
Let’s imagine that we have a simple application to calculate the total order price in our shop. But 
also, we have some additional requests. If a buyer orders our products in a preorder period we are 
going to give a 10 percent discount.
*/

using System;
using System.Collections.Generic;

// Model - 'Product'
public class Product
{
    public string Name {get; set;}
    public double Price {get; set;}
}

// Component class
public abstract class OrderBase
{
    protected List<Product> products = new List<Product> {
        new Product {Name = "Phone", Price = 587},
        new Product {Name = "Tablet", Price = 800},
        new Product {Name = "PC", Price = 1200}
    };

    public abstract double CalculateTotalOrderPrice();
}

// Concrete Component class 1 - 'Regular Order'
public class RegularOrder : OrderBase
{
    public override double CalculateTotalOrderPrice() {
        Console.WriteLine("Calculating the total price of a regular order");
        double sum = 0;
		products.ForEach(x => sum += x.Price);
		return sum;
    }
}

// Concrete Component class 2 - 'Pre Order'
public class Preorder : OrderBase
{
    public override double CalculateTotalOrderPrice() {
        Console.WriteLine("Calculating the total price of a preorder.");
		double sum = 0;
		products.ForEach(x => sum += x.Price);
        return sum * 0.9;
    }
}

/* But now, we receive an additional request to allow an additional 10 percent discount 
to our premium users for the preorder. Of course, we could only change the Preorder class 
with one if statement to check if our user is a premium user, but that would break the Open 
Closed Principle. So, in order to implement this additional request, we are going to start 
with a Decorator class which is going to wrap our OrderBase object: */

// Decorator class - on top of the component classes of 'OrderBase' 
public class OrderDecorator : OrderBase
{
    protected OrderBase order;

    public OrderDecorator(OrderBase order) {
        this.order = order;
    }

    public override double CalculateTotalOrderPrice() {
        Console.WriteLine($"Calculating the total price in a decorator class");
        return order.CalculateTotalOrderPrice();
    }
}

// Concrete Decorator class - 'Premium Order'
public class PremiumPreorder : OrderDecorator
{
    public PremiumPreorder(OrderBase order) : base(order) { }

    public override double CalculateTotalOrderPrice() {
        Console.WriteLine($"Calculating the total price in the {nameof(PremiumPreorder)} class.");
        var preOrderPrice =  base.CalculateTotalOrderPrice();

        Console.WriteLine("Adding additional discount to a preorder price");
        return preOrderPrice * 0.9;
    }
}
// Now we can clearly see how our Decorator class wraps the preorder object.

public class Run
{
    public static void Main(string[] args) {
        var regularOrder = new RegularOrder();
        Console.WriteLine(regularOrder.CalculateTotalOrderPrice());
        Console.WriteLine();

        var preOrder = new Preorder();
        Console.WriteLine(preOrder.CalculateTotalOrderPrice());
        Console.WriteLine();

        var premiumPreorder = new PremiumPreorder(preOrder);
        Console.WriteLine(premiumPreorder.CalculateTotalOrderPrice());
    }
}