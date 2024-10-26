// Visitor Design Pattern - Behavioral Design Pattern 

/*

The Visitor pattern allows decoupling algorithms from the objects on which they operate. It 
allows adding and changing functionalities in a type-safe and SOLID manner.
This is one of the most complex classic design patterns.

The simplest way to explain the Visitor pattern structure is by explaining its main actors – 
the Visitor and the Visitable. The latter is often referred to as the Element, but calling it 
Visitable (or Visitee) shows its purpose better.

The Visitor is the class that adds the extra functionalities to the elements that it visits. 
The Visitable is the class that can accept a Visitor to become enriched with extra functionalities.

Double dispatch:
The act of visiting is the crucial point where these two meet and interact. Visiting assures type 
safety and extensibility, by the magic of double dispatch. In simple terms, double dispatch means 
that first, the Visitee accepts the Visitor, and only then does the Visitor visit the Visitee.

This pattern consists of two areas:
1. The pattern structure area [IVisitable, Visitable(s), IVisitor]
2. The extensibility area [Visitor(s)]

The extensibility contract is the 'IVisitableElement' interface and 'IVisitor' interface. These two 
interfaces are a way of saying "the domain objects allow to be extended (they accept the visitor) 
and they don’t care how (they don’t know the visitor type)".

By creating new visitors we are extending the domain elements without changing them. That’s one of 
the ways of adhering to the Open/Closed Principle. 

Invoking the Visitors and Visitable Elements:
---------------------------------------------
So, how and where do those parts meet?
They meet when we call the Accept method on a collection of IVisitableElements.

public void UseTheVisitorPattern(IVisitableElement[] elements, IVisitor visitor)
{
    foreach (IVisitableElement element in elements)
    {
        element.Accept(visitor);
    }
}

How Does the Pattern Achieve Type Safety?
-----------------------------------------
Thanks to polymorphism we can store many types of IVisitableElements in a single collection. 
This is very useful, as it allows us to treat them in the same way, provided we only need to 
access the common part of these types. Things get complicated when we need to access members 
declared on the derived types. In order to do that, we need to introduce casting:

foreach (IVisitableElement element in elements)
{
    switch (element)
    {
        case ConcreteElementOne one: visitor.Visit(one); break;
        case ConcreteElementTwo two: visitor.Visit(two); break;
    }
}
This creates a sort of a ‘single dispatch’ visitor (not a real thing!). There are several drawbacks 
to this. The switch block has to specify cases for all the types of visitable elements. The C# compiler 
will not warn us about a type that is not covered. 

This is where the double dispatch of the Visitor pattern comes into play. Let’s have a look at the flow 
of control step by step. 

1. The first dispatch happens when the IVisitableElement accepts the visitor.
element.Accept(visitor);
2. The second dispatch happens when the visitor gets the instance of the concrete visitable element passed 
to its Visit method.
public void Accept(IVisitor visitor) => visitor.Visit(this);

We should note that this code is written in every concrete implementation of the IVisitableElement. This 
means that the compiler knows whether the IVisitor supports this type or not. 

Usage examples: 
Visitor isn’t a very common pattern because of its complexity and narrow applicability.

*/

/* 
Scenario:
---------
Let’s have a look into how the visitor pattern fits in a project. To make the example interesting and 
not-too-trivial, we are going to assume we’re working on a next-generation automated medical analysis 
system. The system allows the analysis of a stream of data coming from various medical tests (blood tests, 
x-ray, ECG, etc.) to check for possibilities of various diseases.
*/

using System;
using System.Collections.Generic;
using System.Linq;

public enum AlertReport 
{
    LowRisk,
    NotAnalyzable,
    HighRisk
}

// The pattern structure area starts here ------->
// Visitable contract
public interface ISicknessAlertVisitable
{
    AlertReport Accept(ISicknessAlertVisitor visitor);
}

// Visitor Contract
public interface ISicknessAlertVisitor
{
    AlertReport Visit(BloodSample blood);
    AlertReport Visit(XRayImage rtg);
    AlertReport Visit(EcgReading sample);
}

// Concrete Visitable - 1 
public class BloodSample : ISicknessAlertVisitable
{
    public AlertReport Accept(ISicknessAlertVisitor visitor) => visitor.Visit(this);
}

// Concrete Visitable - 2 
public class XRayImage : ISicknessAlertVisitable
{
    public AlertReport Accept(ISicknessAlertVisitor visitor) => visitor.Visit(this);
}

// Concrete Visitable - 3 
public class EcgReading : ISicknessAlertVisitable
{
    public AlertReport Accept(ISicknessAlertVisitor visitor) => visitor.Visit(this);
}
// <-------- The pattern structure area ends here

// The extensibility area starts here ----------->
// Concrete visitor - 1
public class HivDetector : ISicknessAlertVisitor
{
    public AlertReport Visit(BloodSample blood) {
        Console.WriteLine($"{GetType().Name} - Checking blood sample");
        return AlertReport.LowRisk;
    }

    public AlertReport Visit(XRayImage rtg) {
        Console.WriteLine($"{GetType().Name} - currently cannot detect HIV based on X-Ray");
        return AlertReport.NotAnalyzable;
    }

    public AlertReport Visit(EcgReading sample) {
        Console.WriteLine($"{GetType().Name} - Checking heart rate abnormalities");
        return AlertReport.HighRisk;
    }
}
/* The team works on similar detectors for cancers, covid and other sicknesses. However, not every 
test result is useful to detect every disease. This actually visualizes one of the drawbacks of the 
Visitor pattern. A lot of methods need at least a formal implementation, even though they are not needed. */

// <----------  The extensibility area ends here

// Client 
public class TestResultsMonitoringApp
{
    private readonly List<ISicknessAlertVisitor> _detectors;

    public TestResultsMonitoringApp(List<ISicknessAlertVisitor> detectors)
    {
        _detectors = detectors;
    }

    public List<AlertReport> AnalyzeResultsBatch(IEnumerable<ISicknessAlertVisitable> testResults)
    {
        var alertReports = new List<AlertReport>();

        foreach (var sample in testResults)
        {
            foreach (var detector in _detectors)
            {
                alertReports.Add(sample.Accept(detector));
            }
        }
        return alertReports;
    }
}

// Main
public class Run 
{
    public static void Main(string[] args) {
        HivDetector hivDetector1 = new HivDetector();
        List<ISicknessAlertVisitor> detectors = new List<ISicknessAlertVisitor> {hivDetector1};
        TestResultsMonitoringApp testResultsMonitoringApp = new TestResultsMonitoringApp(detectors);
        BloodSample bloodSample = new BloodSample();
        XRayImage xRayImage = new XRayImage();
        EcgReading ecgReading = new EcgReading();
        List<ISicknessAlertVisitable> tests = new List<ISicknessAlertVisitable> {bloodSample, xRayImage, ecgReading};
        var results = testResultsMonitoringApp.AnalyzeResultsBatch(tests);
		for(int i=0; i<results.Count; i++)
		{
			Console.WriteLine("Test result: " + results[i]);
		}
    }
}

/*
Pros:
-----
It allows extending the functionality of the objects without altering them. The responsibilities 
for functionalities controlled by the visitors can be then transferred to a separate team. That 
team does not require in-depth knowledge of the domain objects – or even access to them.
Cons:
-----
Moving certain functionalities outside of a class increases its cohesion. 
Another thing that the pattern delivers is the type safety without the conditional type checking 
and casting logic.

Arguably, the amount of indirection, additional layers, and complexity that it adds does not outweigh 
the benefits. For that reason, it tends to find its place more often in enterprise projects than 
small-to-mid scale.
*/