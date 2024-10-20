// Composite Design Pattern - Structural Design Pattern

/*

The Composite design pattern is a structural design pattern that allows us to compose objects 
into a tree structure and then work with that structure as if it is a single object. That also 
means using this design pattern makes sense when the part of our app can be represented as a tree.

The Composite design pattern consists of the following parts:
- Component
- Leaf
- Composite

A component is an interface that describes operations that are common to either simple or complex 
elements of the tree.

A leaf is a single object, that doesn’t have sub-elements. Our tree structure consists of more 
leaf objects.

A Composite is an object that does have sub-elements (leaves or other composite objects). 
Interesting thing is that the Composite object isn’t familiar with the concrete classes of its 
children. It communicates with its children via the Component interface.

Finally, we have a client, which works with all the elements through the Component interface.

*/

/* 
Scenario:
---------
Let’s imagine that we need to calculate the total price of a gift which we are selling in our shop. 
The gift could be a single element (toy) or it can be a complex gift that consists of a box with two 
toys and another box with maybe one toy and the box with a single toy inside. As we can see, we have 
a tree structure representing our complex gift so, implementing the Composite design pattern will be 
the right solution for us.
*/

// Model - 'The component'
public abstract class GiftBase
{
    protected string name;
    protected int price;

    public GiftBase(string name, int price) {
        this.name = name;
        this.price = price;
    }

    public abstract int CalculateTotalPrice();
}

/* Now, in many examples, we can see additional operations like add and remove inside the abstract 
class, but we are not going to add them in this class, because our Leaf class doesn’t need them. 
What we are going to create instead is a new interface */
public interface IGiftOperations
{
    void Add(GiftBase gift);
    void Remove(GiftBase gift);
}

// Composite class implementation
public class CompositeGift : GiftBase, IGiftOperations
{
    private List<GiftBase> _gifts;

    public CompositeGift(string name, int price) : base(name, price) {
        _gifts = new List<GiftBase>();
    }

    public void Add(GiftBase gift) {
        _gifts.Add(gift);
    }

    public void Remove(GiftBase gift) {
        _gifts.Remove(gift);
    }

    public override int CalculateTotalPrice() {
        int total = 0;

        Console.WriteLine($"{name} contains the following products with prices:");

        foreach (var gift in _gifts)
        {
            total += gift.CalculateTotalPrice();
        }

        return total;
    }
}

// Leaf class implementation
public class SingleGift : GiftBase
{
    public SingleGift(string name, int price) : base(name, price) {
    }

    public override int CalculateTotalPrice() {
        Console.WriteLine($"{name} with the price {price}");

        return price;
    }
}
/* That is all we need for the Leaf implementation because it doesn’t have a sub-levels 
so it doesn’t require to add or remove features at all. */

public class Run
{
    public static void Main(string[] args)
    {
        //single gift
        var phone = new SingleGift("Phone", 256);
        Console.WriteLine($"Total price of this single present is: {phone.CalculateTotalPrice()}");
        Console.WriteLine();

        //composite gift
        var rootBox = new CompositeGift("RootBox", 0);
        var truckToy = new SingleGift("TruckToy", 289);
        var plainToy = new SingleGift("PlainToy", 587);
        rootBox.Add(truckToy);
        rootBox.Add(plainToy);
        //composite gift under composite git
        var childBox = new CompositeGift("ChildBox", 0);
        var soldierToy = new SingleGift("SoldierToy", 200);
        childBox.Add(soldierToy);
        rootBox.Add(childBox);
        Console.WriteLine($"Total price of this composite present is: {rootBox.CalculateTotalPrice()}");
    }
}