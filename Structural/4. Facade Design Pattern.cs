// Facade Design Pattern - Structural Design Pattern

/*

As the name suggests, it represents a “facade” for the end-user to simplify the usage of the 
subsystems that are poorly designed and/or too complicated by hiding their implementation 
details. It also comes in handy when working with complex libraries and APIs.

A Facade Pattern is represented by a single-class interface that is simple, easy to read and 
use, without the trouble of changing the subsystems themselves. However, we must be careful 
since limiting the usage of the subsystem’s functionalities may not be enough for the power-
users.

*/

/* 
Scenario:
---------
Let’s say we have a list of restaurants. We open the restaurant’s page, find the dish that we 
like and add it to the cart. We do it as many times as we want and complete the order. When we 
submit the order, we get an order confirmation along with the price of the order.
*/

using System;
using System.Collections.Generic;

// Model - 'Order'
public class Order
{
    public string DishName { get; set; }
    public double DishPrice { get; set; }
    public string User { get; set; }
    public string ShippingAddress { get; set; }
    public double ShippingPrice { get; set; }

    public override string ToString() {
        return string.Format("User {0} ordered {1}. The full price is {2} dollars.",
            User, DishName, DishPrice + ShippingPrice);
    }
}

/* Furthermore, we have to create two more classes – an online restaurant and a shipping service. */

public class OnlineRestaurant
{
    private readonly List<Order> _cart;

    public OnlineRestaurant() {
        _cart = new List<Order>();
    }

    public void AddOrderToCart(Order order) {
        _cart.Add(order);
    }

    public void CompleteOrders() {
        Console.WriteLine("Orders completed. Dispatch in progress...");
    }
}

public class ShippingService
{
    private Order _order;

    public void AcceptOrder(Order order) {
        _order = order;
    }

    public void CalculateShippingExpenses() {
        _order.ShippingPrice = 15.5;
    }

    public void ShipOrder() {
        Console.WriteLine(_order.ToString());
        Console.WriteLine("Order is being shipped to {0}...", _order.ShippingAddress);
    }
}

// Facade Pattern Implementation 

/* One of the goals of the Facade Pattern is to hide the implementation details which indicates 
that having everything in the Main class doesn’t do the work. It is too much unnecessary 
information, therefore, we would like it better somewhere else. */

public class Facade
{
    private readonly OnlineRestaurant _restaurant;
    private readonly ShippingService _shippingService;

    public Facade(OnlineRestaurant restaurant, ShippingService shippingService) {
        _restaurant = restaurant;
        _shippingService = shippingService;
    }

    public void OrderFood(List<Order> orders) {
        foreach (var order in orders) {
            _restaurant.AddOrderToCart(order);
        }

        _restaurant.CompleteOrders();

        foreach (var order in orders) {
            _shippingService.AcceptOrder(order);
            _shippingService.CalculateShippingExpenses();
            _shippingService.ShipOrder();
        }
    }
}
/* The Facade class will act as a “middleware” between the User and the complexity of the 
system without changing the business logic. 

Here, instead of calling 'AddOrderToCart', 'CompleteOrders', 'AcceptOrder', 'CalculateShippingExpenses' 
and 'ShipOrder' from the main, we built a facade having 'OrderFood' doing all these jobs behind. */

// Main
public class Program
{
    public static void Main(string[] args)
    {
        var restaurant = new OnlineRestaurant();
        var shippingService = new ShippingService();

        var facade = new Facade(restaurant, shippingService);

        var chickenOrder = new Order() { DishName = "Chicken with rice", DishPrice = 20.0, 
            User = "User1", ShippingAddress = "Random street 123" };
        var sushiOrder = new Order() { DishName = "Sushi", DishPrice = 52.0, User = "User2", 
            ShippingAddress = "More random street 321" };

        facade.OrderFood(new List<Order>() { chickenOrder, sushiOrder });

        Console.ReadLine();
    }
}

/* Note: It can limit the user’s abilities to make use of the full potential of the application 
or library that we’re trying to simplify. */