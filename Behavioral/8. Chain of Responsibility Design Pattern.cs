// Chain of Responsibility Design Pattern - Behavioral Design Pattern 

/*

The Chain of Responsibility Design Pattern states, “Avoid coupling the sender of a request to its receiver 
by giving more than one receiver object a chance to handle the request. Chain the receiving objects and pass 
the request along until an object handles it”.

This pattern allows multiple objects to handle the request without coupling the sender class to the concrete 
classes of the receivers.

In this design pattern, normally, each receiver contains a reference to the next receiver. If one receiver 
cannot handle the request, it passes the same request to the next receiver, and so on. In this case, one 
receiver can handle the request in the chain, or one or more receivers can handle the request.

The classes and objects participating in this pattern include:
1. Handler (Approver)
- defines an interface for handling the requests
- (optional) implements the successor link
2. ConcreteHandler (Director, VicePresident, President)
- handles requests it is responsible for
- can access its successor
- if the ConcreteHandler can handle the request, it does so; otherwise it forwards the request to 
  its successor
3. Client (ChainApp)
- initiates the request to a ConcreteHandler object on the chain

Usage examples: 
The Chain of Responsibility is pretty common in C#. It’s mostly relevant when your code operates with chains 
of objects, such as filters, event chains, etc.

Identification: 
The pattern is recognizable by behavioral methods of one group of objects that indirectly call the same methods 
in other objects, while all the objects follow the common interface.

*/

/*
Scenario:
---------
We have an ATM machine and four handlers. The TwoThousandhandler will give 2000 rupees. Similarly, the 
FiveHundredHandler will give 500 hundred rupees and the same for 200 and 100 handlers.
Now, Anurag wants to withdraw 4600 rupees from the ATM machine. So, what the ATM machine will do is send 
the request to the first handler, i.e., the TwoThousandHandler, and the TwoThousandHandler will check the 
amount and give two 2000 rupees notes, and then the remaining amount is 600 rupees follows the chain.
*/

using System;

// Handler
public abstract class Handler
{
    public Handler NextHandler;

    public void SetNextHandler(Handler NextHandler) {
        this.NextHandler = NextHandler;
    }

    public abstract void DispatchNote(long requestedAmount);
}

// Here, we will create four handlers (TwoThousandHandler, FiveHundredHandler, and 
// HundredHandler) to handle the respective currency.

//Concrete Handler 1
public class TwoThousandHandler : Handler
{
    public override void DispatchNote(long requestedAmount) {
        //First Check the Number of 2000 Notes To Be Dispatched
        long numberofNotesToBeDispatched = requestedAmount / 2000;
        if (numberofNotesToBeDispatched > 0) {
            if (numberofNotesToBeDispatched > 1) {
                Console.WriteLine(numberofNotesToBeDispatched + " Two Thousand notes are dispatched by TwoThousandHandler");
            }
            else {
                Console.WriteLine(numberofNotesToBeDispatched + " Two Thousand note is dispatched by TwoThousandHandler");
            }
        }

        //Then check the Pending amount
        long pendingAmountToBeProcessed = requestedAmount % 2000;

        //If the Pending amount is greater than 0, then call the next handler to handle the request
        if (pendingAmountToBeProcessed > 0) {
            //For TwoThousandHandler, the next handler is FiveHundredHandler 
            NextHandler.DispatchNote(pendingAmountToBeProcessed);
        }
    }
}

//Concrete Handler 2
public class FiveHundredHandler : Handler
{
    public override void DispatchNote(long requestedAmount) {
        //First Check the Number of 500 Notes To Be Dispatched
        long numberofNotesToBeDispatched = requestedAmount / 500;
        if (numberofNotesToBeDispatched > 0) {
            if (numberofNotesToBeDispatched > 1) {
                Console.WriteLine(numberofNotesToBeDispatched + " Five Hundred notes are dispatched by FiveHundredHandler");
            }
            else {
                Console.WriteLine(numberofNotesToBeDispatched + " Five Hundred note is dispatched by FiveHundredHandler");
            }
        }

        //Then check the Pending amount
        long pendingAmountToBeProcessed = requestedAmount % 500;

        //If Pending amount is greater than 0, then call the next handler to handle the request
        if (pendingAmountToBeProcessed > 0) {
            //For FiveHundredHandler, the next handler is HundredHandler  
            NextHandler.DispatchNote(pendingAmountToBeProcessed);
        }
    }
}

//Concrete Handler 3
public class HundredHandler : Handler
{
    public override void DispatchNote(long requestedAmount) {
        //First Check the Number of 100 Notes To Be Dispatched
        long numberofNotesToBeDispatched = requestedAmount / 100;
        if (numberofNotesToBeDispatched > 0) {
            if (numberofNotesToBeDispatched > 1) {
                Console.WriteLine(numberofNotesToBeDispatched + " Hundred notes are dispatched by HundredHandler");
            }
            else {
                Console.WriteLine(numberofNotesToBeDispatched + " Hundred note is dispatched by HundredHandler");
            }
        }
        //No Need to Check the Next Handler
    }
}

// Chaining the handlers
// This class managed the sequence in which all the handlers are going to be chained together
// This class initiates the request to a ConcreteHandler object on the chain
public class ATM
{
    private TwoThousandHandler twoThousandHandler = new TwoThousandHandler();
    private FiveHundredHandler fiveHundredHandler = new FiveHundredHandler();
    private HundredHandler hundredHandler = new HundredHandler();

    public ATM() {
        // Prepare the chain of Handlers
        twoThousandHandler.SetNextHandler(fiveHundredHandler);
        fiveHundredHandler.SetNextHandler(hundredHandler);
    }

    public void Withdraw(long requestedAmount) {
        //First check whether the amount is Divisible by 100 or not
        if(requestedAmount % 100 == 0) {
            twoThousandHandler.DispatchNote(requestedAmount);
        }
        else {
            Console.WriteLine($"You Enter Invalid Amount: {requestedAmount}");
        }
    }
}

// Main
public class Run
{
    public static void Main(string[] args) {
        ATM atm = new ATM();
        Console.WriteLine("Requested Amount 4600");
        atm.Withdraw(4600);

        Console.WriteLine("\nRequested Amount 2200");
        atm.Withdraw(1900);

        Console.WriteLine("\nRequested Amount 600");
        atm.Withdraw(600);

        Console.WriteLine("\nRequested Amount 750");
        atm.Withdraw(750);
    }
}

// Another use case can be approvals from higher levels, one after another.
