// Observer Design Pattern - Behavioral Design Pattern

/*

The Observer design pattern allows us to establish a notification mechanism between objects. It 
enables multiple objects to subscribe to another object and get notified when an event occurs to 
this observed object. So, on one hand, we have a Provider (sometimes called a Subject or a Publisher) 
which is the observed object. On the other hand, there are one or more Observers, which are objects 
subscribing to the Provider. An Observer can subscribe to a Provider and get notified whenever a 
predefined condition happens. This predefined condition is usually an event or a state change. 

When to Use Observer Design Pattern?
This pattern is helpful whenever we want to implement some kind of distributed notification system 
within our application. Let’s say we have an e-commerce application, where some customers are interested 
in the products of a particular seller. So, instead of checking for new products every now and then, 
they can just subscribe to the seller and receive real-time updates.
Also, the Observer pattern is pretty common in C# code, especially in the GUI components. It provides a 
way to react to events happening in other objects without coupling to their classes.

Identification: 
The pattern can be recognized by subscription methods, that store objects in a list and by calls to 
the update method issued to objects in that list.

C# has two powerful interfaces that are designed particularly to implement the Observer pattern: 
'IObserver<T>' and 'IObservable<T>'. For an object to be a Provider it has to implement IObservable<T>, 
while to be an Observer it has to implement IObserver<T>; where T is the type of notification object 
sent from the Provider to its Observers.

*/

/* 
Scenario:
---------
Let’s say we are developing a submission system for a company where applicants can apply for jobs. So, 
we want to notify HR specialists whenever a new applicant applies for a job. 
*/

using System;
using System.Collections.Generic;
using System.Linq;

// Model
public class Application
{
    public int JobId {get; set;}
    public string ApplicantName {get; set;}

    public Application(int jobId, string applicantName) {
        JobId = jobId;
        ApplicantName = applicantName;
    }
}

// Provider - Applications Repository 
public class ApplicationsHandler : IObservable<Application>
{
    private List<IObserver<Application>> _observers;
    public List<Application> Applications {get; set;}

    public ApplicationsHandler() {
        _observers = new();
        Applications = new();
    }

    public IDisposable Subscribe(IObserver<Application> observer) {
        if(!_observers.Contains(observer)) {
            _observers.Add(observer);
            foreach(var item in Applications) {
                observer.OnNext(item);
            }
        }
        return new Unsubscriber(_observers, observer);
    }

    // For adding application
    public void AddApplication(Application app) {
        Applications.Add(app);
        foreach (var observer in _observers)
            observer.OnNext(app);
    }

    public void CloseApplications() {
        foreach (var observer in _observers)
            observer.OnCompleted();
        _observers.Clear();
    }
}

// Unsubscription
public class Unsubscriber : IDisposable 
{
    private readonly List<IObserver<Application>> _observers;
    private readonly IObserver<Application> _observer;

    public Unsubscriber(List<IObserver<Application>> observers, IObserver<Application> observer) {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose() {
        if (_observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}

// Observer - HR Specialist
public class HRSpecialist : IObserver<Application>
{
    public string Name {get; set;}
    public List<Application> Applications {get; set;}
	private IDisposable _cancellation;

    public HRSpecialist(string name) {
        Name = name;
        Applications = new();
    }

    public void ListApplications()
    {
        if(Applications.Any()) {
            foreach (var app in Applications) {
                Console.WriteLine($"Hey, {Name}! {app.ApplicantName} has just applied for job no. {app.JobId}");
            }
        }
        else {
            Console.WriteLine($"Hey, {Name}! No applications yet.");
        }
    }

    // Implementations
    public virtual void Subscribe(ApplicationsHandler provider) {
		Console.WriteLine($"{Name} has subscribed");
        _cancellation = provider.Subscribe(this);
    }

    public virtual void Unsubscribe() {
		Console.WriteLine($"{Name} has unsubscribed");
        _cancellation.Dispose();
        Applications.Clear();
    }

    public void OnCompleted() {
        Console.WriteLine($"Hey, {Name}! We are not accepting any more applications");
    }

    public void OnError(Exception error) {
        // This is called by the provider if any exception is raised, no need to implement it here
    }

    public void OnNext(Application value) {
        Applications.Add(value);
    }
}

// Main
public class Run
{
    public static void Main(string[] args) {
        var observer1 = new HRSpecialist("Bill");
        var observer2 = new HRSpecialist("John");

        var provider = new ApplicationsHandler();

        observer1.Subscribe(provider);
        observer2.Subscribe(provider);
        provider.AddApplication(new(1, "Jesus"));
        provider.AddApplication(new(2, "Dave"));

        observer1.ListApplications();
        observer2.ListApplications();

        observer1.Unsubscribe();

        Console.WriteLine();
        provider.AddApplication(new(3, "Sofia"));

        observer1.ListApplications();
        observer2.ListApplications();

        Console.WriteLine();

        provider.CloseApplications();
    }
}