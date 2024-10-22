// Bridge Design Pattern - Structural Design Pattern

/*

The Bridge Design Pattern is a structural pattern that helps us decouple a class’s implementation 
from its abstraction so the two can vary independently.

Generally, the term “abstraction” refers to an interface (or abstract class) construct, and the 
“implementation” refers to concrete sub-classes. However, they convey different meanings in the 
context of the bridge pattern.

The Abstraction defines the top-level control layer of the feature and serves as an entry point 
from the client code. It can be either an interface or an abstract class.

Each RefinedAbstraction is a concrete sub-class that implements the Abstraction interface. Such 
classes may exhibit different strategies as a top-level workflow, but they all communicate to the 
implementation layer through the Implementor interface. 

The Implementor defines an implementation part of the feature that can be developed independently 
and acts as a connecting point from the Abstraction hierarchy. It can be either an interface or an 
abstract class. In other words, the Implementor represents the hierarchy of a designated implementation 
layer.

ConcreteImplementor represents a concrete sub-class of the Implementor interface and holds the actual 
business implementation. 

To sum up, the bridge pattern offers a means to bridge between the Abstraction and Implementor hierarchies.

*/

/* 
Scenario:
---------
We aim to provide a Calculate() method that takes on a base price parameter and returns the calculated 
total price for order-pricing calculator app. The calculation is subjected to different discount options 
and delivery fees.
*/

using System;

// Interface - Face of the hierarchy
public interface IDiscount
{
    decimal GetDiscount(decimal price);
}

// Standalone classes - Under IDiscount
public class NoDiscount : IDiscount
{
    public decimal GetDiscount(decimal price) => 0;
}

// Helper Enum - 'For Discounts'
public enum PromoCode
{
    Free5,
    Free10
}

public class PromoCodeDiscount(PromoCode promoCode) : IDiscount
{
    public decimal GetDiscount(decimal price) {
        var factor = promoCode switch {
            PromoCode.Free5 => 0.05m,
            PromoCode.Free10 => 0.10m,
            _ => throw new NotImplementedException()
        };

        return price * factor;
    }
}

public class HotSaleDiscount : IDiscount
{
    public decimal GetDiscount(decimal price) => price > 50 ? 20 : 0;    
}
// The Implementor end of the bridge is ready, let’s take a look at the whole bridge plan.

// Abstraction - 'Actual Client'
public abstract class PriceCalculator(IDiscount discount)
{
    public decimal Calculate(decimal basePrice) {
        var deliveryFee = GetDeliveryFee();

        var discountAmount = discount.GetDiscount(basePrice);

        return basePrice + deliveryFee - discountAmount;
    }

    protected abstract decimal GetDeliveryFee();
}

// Concrete Definition
public class StandardPriceCalculator(IDiscount discount) : PriceCalculator(discount)
{
    protected override decimal GetDeliveryFee() => 2.5m;
}

public class ExpeditedPriceCalculator(IDiscount discount) : PriceCalculator(discount)
{
    protected override decimal GetDeliveryFee() => 5m;
}

// Main
public class Run 
{
    public static void Main(string[] args) {
        var productPrice = 100m;

        var standard = new StandardPriceCalculator(new NoDiscount())
            .Calculate(productPrice);
        Console.WriteLine($"Standard: {standard}");

        var standardPromo = new StandardPriceCalculator(new PromoCodeDiscount(PromoCode.Free10))
            .Calculate(productPrice);
        Console.WriteLine($"Standard Promo: {standardPromo}");

        var standardHotSale = new StandardPriceCalculator(new HotSaleDiscount())
            .Calculate(productPrice);
        Console.WriteLine($"Standard HotSale: {standardHotSale}");

        var expedite = new ExpeditedPriceCalculator(new NoDiscount())
            .Calculate(productPrice);
        Console.WriteLine($"Expedite: {expedite}");

        var expeditePromo = new ExpeditedPriceCalculator(new PromoCodeDiscount(PromoCode.Free10))
            .Calculate(productPrice);
        Console.WriteLine($"Expedite Promo: {expeditePromo}");

        var expediteHotSale = new ExpeditedPriceCalculator(new HotSaleDiscount())
            .Calculate(productPrice);
        Console.WriteLine($"Expedite HotSale: {expediteHotSale}");
    }
}

/* So we successfully decouple the discount model from the calculator model.
That means we can pair up a calculator with any variant of the discount model on demand. */

/*
Pattern Caveats:
----------------
Undoubtedly, the Bridge pattern is a powerful technique that helps us design highly decoupled structures 
to mitigate problems we face in a complex monolithic class. That said, there are certain factors that we 
should be aware of.

The biggest concern with the bridge pattern is the complexity. It’s crucial to identify the independent 
extensible points and their scope boundaries. This demands a greater understanding of the overall features 
and forecast for further expansion.

While composition-based workflow allows us to combine the components on demand, it also shifts the 
responsibility from internal mechanisms toward the client code. This may lead to incompatible pairings 
and unexpected behavior if not carefully designed.

In a nutshell, we should carefully assess the worth of bridging between hierarchies, otherwise, we may 
end up with an unnecessarily convoluted design.
*/