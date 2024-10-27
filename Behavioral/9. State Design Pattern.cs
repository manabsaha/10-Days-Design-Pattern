// State Design Pattern - Behavioral Design Pattern

/*

The State Design Pattern allows an object to alter its behavior when its internal state changes. In simple 
words, we can say that the State Design Pattern allows an object to change its behavior depending on its 
current internal state. The State Design Pattern encapsulates varying behavior for the same object based on 
its internal state. 

This pattern is useful when an object needs to go through several states, and its behavior differs for each 
state. Instead of having conditional statements throughout a class to handle state-specific behaviors, the 
State Design Pattern delegates this responsibility to individual state classes.


The classes and objects participating in this pattern include:
1. Context (Account) : Defines the interface of interest to clients maintains an instance of a ConcreteState 
subclass that defines the current state.
2. State  (State) : defines an interface for encapsulating the behavior associated with a particular state 
of the Context.
3. Concrete State  (RedState, SilverState, GoldState) : each subclass implements a behavior associated with 
a state of Context

Usage examples: 
The State pattern is commonly used in C# to convert massive switch-base state machines into objects.

Identification: 
State pattern can be recognized by methods that change their behavior depending on the objects state, 
controlled externally.

*/

/*
Scenario:
--------- 
Let’s say the ATM machine’s internal state is Debit Card Not Inserted. Then what are all the operations 
you can do? The operations you can do are as follows.

1. You can insert the debit card.
2. You cannot eject the debit card as the debit card is not inserted into the ATM Machine.
3. Again, you cannot enter the PIN and withdraw money. So, the only allowed operation is he can insert the debit card.

Suppose you inserted the debit card into the machine. So, the state of the ATM (i.e., Context Object) is changed to 
Debit Card Inserted. Then what are all the operations you can do? The operations you can do are as follows.

1. You cannot insert the debit card as one is already inserted into the machine.
2. It allows you to eject the Debit card.
3. You can enter the PIN number and withdraw the money.

So, you must remember that if the state is Debit Card Not Inserted, it will allow you to perform certain operations. 
Changing the state to Debit Card Inserted will allow you to perform another set of operations. So, based on the 
internal state of the ATM machine, the behavior will be changed.
*/

using System;

// State Interface
public interface IATMState
{
    void InsertDebitCard();
    void EjectDebitCard();
    void EnterPin();
    void WithdrawMoney();
}

// Concrete State - 1 
// Implement various behaviors, associated with a state of the Context Object.
public class DebitCardNotInsertedState : IATMState
{
    public void InsertDebitCard() {
        Console.WriteLine("DebitCard Inserted");
    }

    public void EjectDebitCard() {
        Console.WriteLine("You cannot eject the Debit CardNo, as no Debit Card in ATM Machine slot");
    }

    public void EnterPin() {
        Console.WriteLine("You cannot enter the pin, as No Debit Card in ATM Machine slot");
    }

    public void WithdrawMoney() {
        Console.WriteLine("You cannot withdraw money, as No Debit Card in ATM Machine slot");
    }
}

// Concrete State - 2
public class DebitCardInsertedState : IATMState
{
    public void InsertDebitCard() {
        Console.WriteLine("You cannot insert the Debit Card, as the Debit card is already there ");
    }

    public void EjectDebitCard() {
        Console.WriteLine("Debit Card is ejected");
    }

    public void EnterPin() {
        Console.WriteLine("Pin number has been entered correctly");
    }

    public void WithdrawMoney() {
        Console.WriteLine("Money has been withdrawn");
    }
}

// The Context Class 
// Defines the interface which is going to be used by the clients. 
public class ATMMachine : IATMState
{
    // A reference to the current state of the Context.
    public IATMState AtmMachineState = null;

    public ATMMachine() {
        AtmMachineState = new DebitCardNotInsertedState();
    }

    public void InsertDebitCard() {
        AtmMachineState.InsertDebitCard();
        
        if (AtmMachineState is DebitCardNotInsertedState) {
            AtmMachineState = new DebitCardInsertedState();

            Console.WriteLine($"ATM Machine internal state has been changed to : {AtmMachineState.GetType().Name}");
        }
    }

    public void EjectDebitCard() {
        AtmMachineState.EjectDebitCard();
        
        if (AtmMachineState is DebitCardInsertedState) {
            AtmMachineState = new DebitCardNotInsertedState();
            Console.WriteLine($"ATM Machine internal state has been Changed to : {AtmMachineState.GetType().Name}");
        }
    }

    public void EnterPin() {
        AtmMachineState.EnterPin();
    }
    public void WithdrawMoney() {
        AtmMachineState.WithdrawMoney();
    }
}

// Main
public class Run
{
    public static void Main(string[] args) {
        ATMMachine atmMachine = new ATMMachine();

        Console.WriteLine("ATM Machine Current state : " + atmMachine.AtmMachineState.GetType().Name);
        Console.WriteLine();
        
        atmMachine.EnterPin();
        atmMachine.WithdrawMoney();
        atmMachine.EjectDebitCard();
        atmMachine.InsertDebitCard();
        Console.WriteLine();

        Console.WriteLine("ATM Machine Current state : " + atmMachine.AtmMachineState.GetType().Name);
        Console.WriteLine();

        atmMachine.EnterPin();
        atmMachine.WithdrawMoney();
        atmMachine.InsertDebitCard();
        atmMachine.EjectDebitCard();
        Console.WriteLine();

        Console.WriteLine("ATM Machine Current state : " + atmMachine.AtmMachineState.GetType().Name);
    }
}

/*
Advantages:
-----------
1. Encapsulation of State-Based Behavior: State-specific logic is encapsulated in state classes.
2. Easy to Add New States: Introducing new states doesn’t require changing the context or other states.
3. Eliminates Conditional Statements: It helps to eliminate conditional statements for behavior changes based on 
the state.
4. Maintainability and Flexibility: The state pattern makes it easier to maintain and extend state-based behavior.
5. Dynamic Behavior Change: The context can change its behavior at runtime depending on its state.
*/