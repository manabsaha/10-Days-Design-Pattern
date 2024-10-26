// Template Design Pattern - Behavioral Design Pattern

/*

The Template Method is a design paradigm that emphasizes the reuse of an algorithm structure while 
letting subclasses redefine certain parts of the algorithm. As the name implies, this pattern tells 
us to design our concrete classes based on a “template method”.

Abstract class ->
public operation()
{private} step1()
{abstract} step2()
{virtual} step3()
{private} step 4()

Here, operation() is the template method that is defined in an abstract base class and each concrete 
descendant inherits it naturally. This routine outlines the combined algorithm and carries out a sequence 
of sub-routines. We can classify these sub-routines into three categories - 

1. Private/protected methods represent the static parts of the algorithm. These routines define the logic 
blocks that should not vary in descendants.
2. Abstract methods represent the dynamic steps of the algorithm which vary in descendant classes and hence 
must be implemented there.
3. Virtual methods (aka Hooks) allow the base class to provide a default implementation as well as let the 
sub-classes revise the logic if needed.

Usage examples: 
The Template Method pattern is quite common in C# frameworks. Developers often use it to provide framework 
users with a simple means of extending standard functionality using inheritance.

Identification: 
Template Method can be recognized if you see a method in base class that calls a bunch of other methods that 
are either abstract or empty.

*/

/* 
Scenario:
---------
As an example, let’s think of a reporting service that sends XML output to a recipient email address.
We need to have another service, that will send JSON, PDF  output to a recipient email address.
*/

using System;
using System.Collections.Generic;

// Model
public class Product 
{
    public string Name {get; set;}
    public double Price {get; set;}

    public Product(string name, double price) {
        Name = name;
        Price = price;
    }
}

public abstract class ProductReportingBase
{
    // operation()
    public void Send() {
        var products = GetProducts();

        var output = Transform(products);

        var recipient = GetRecipient();

        SendEmail(output, recipient);
    }

    // sub-routines()
    private IEnumerable<Product> GetProducts() {
        IEnumerable<Product> products = new List<Product> {
            new Product("Phone", 13000.0),
            new Product("Laptop", 58000.0),
            new Product("AC", 32000.0),
        };
        return products;
    }

    protected abstract string Transform(IEnumerable<Product> products);

    private string GetRecipient()
        => "default recipient";

    private void SendEmail(string output, string recipient)
        => Console.WriteLine($"Sent {output} to {recipient}");
}

// Implementation classes starts ----->
public class ProductXmlReporting : ProductReportingBase
{
    protected override string Transform(IEnumerable<Product> products) 
        => "XML output";
}

public class ProductJsonReporting : ProductReportingBase
{
    protected override string Transform(IEnumerable<Product> products) 
        => "JSON output";
}

public class ProductPdfReporting : ProductReportingBase
{
    protected override string Transform(IEnumerable<Product> products) 
        => "PDF output";
}
// <------- implementation classes ends

// We can change the subroutines to protected-virtual if others steps needs to be 
// implemented by the implementor classes on top of default implementation.

// Main
public class Run 
{
    public static void Main(string[] args) {
        ProductXmlReporting productXmlReporting = new ProductXmlReporting();
        productXmlReporting.Send();
        ProductJsonReporting productJsonReporting = new ProductJsonReporting();
        productJsonReporting.Send();
        ProductPdfReporting productPdfReporting = new ProductPdfReporting();
        productPdfReporting.Send();
    }
}

/*
Caveats of Template Method Pattern
----------------------------------
Template Method is one of the basic design practices that we can follow in our everyday programming. 
However, it has some trade-offs that we should be aware of.

The fact that the base class owns the control point may also make it less adaptable and more error-prone. 
Also, maintaining the right sequence of steps to cover all possible scenarios might be challenging when 
there are too many steps involved. 

Another potential source of side effects may arise when we employ hooks. Such algorithms often expect the 
subclasses to call the base method before/after the override is done. Failing to call the base method at 
the right point results in an unexpected behavior which is quite a common mistake in such scenarios.

Since the template pattern works on inheritance-based flow, it may eventually suffer from a deep hierarchy 
tree. A composition-based pattern like the Bridge Pattern may provide a better solution.
*/