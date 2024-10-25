// Strategy Design Pattern - Behavioral Design Pattern

/*

The Strategy design pattern is a behavioral design pattern that allows us to define different 
functionalities, put each functionality in a separate class, and make their objects interchangeable.

In other words, we have a main Context object that holds a reference toward a Strategy object and 
delegates it by executing its functionality. If we want to change the way the Context performs its 
work, we can just replace the current Strategy object with another one.

As we stated above, the Strategy design pattern consists of the Context object which maintains the 
reference towards the strategy object. But it is not the only part of the puzzle. For the complete 
implementation, we need the Strategy object (interface) to define a way for the Context object to 
execute the strategy and the Concrete Strategies objects that implement the Strategy interface.

The Strategy design pattern is quite common in the C# language due to its various uses to provide 
the changing behavior of a class without modifying it. This complies with the rules of the Open 
Closed Principle, which we talked about in one of our previous articles.

Usage examples: The Strategy pattern is very common in C# code. It’s often used in various frameworks 
to provide users a way to change the behavior of a class without extending it.
Identification: Strategy pattern can be recognized by a method that lets a nested object do the actual 
work, as well as a setter that allows replacing that object with a different one.

*/

/*
Scenario:
---------
The task is to calculate the total cost for the developer’s salaries, but for the different developer 
levels, the salary is calculated differently.
*/

using System;
using System.Linq;
using System.Collections.Generic;

// Types of Developer
public enum DeveloperLevel
{
    Senior,
    Junior
}

// Model
public class DeveloperReport
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DeveloperLevel Level { get; set; }
    public int WorkingHours { get; set; }
    public double HourlyRate { get; set; }

    public double CalculateSalary() => WorkingHours * HourlyRate; 
}

// Strategy Interface
public interface ISalaryCalculator
{
    double CalculateTotalSalary(IEnumerable<DeveloperReport> reports);
}

// Concrete Strategy - 1
public class JuniorDevSalaryCalculator : ISalaryCalculator 
{
    public double CalculateTotalSalary(IEnumerable<DeveloperReport> reports) {
        return reports.Where(x => x.Level == DeveloperLevel.Junior)
            .Select(x => x.CalculateSalary()).Sum();
    }
}

// Concrete Strategy - 2
public class SeniorDevSalaryCalculator : ISalaryCalculator 
{
    public double CalculateTotalSalary(IEnumerable<DeveloperReport> reports) {
        return reports.Where(x => x.Level == DeveloperLevel.Senior)
            .Select(x => x.CalculateSalary() * 1.2).Sum();
    }
    // As we can see, for the senior developers, we are adding a 20% bonus to the salary
}

// Context Object
public class SalaryCalculator 
{
    private ISalaryCalculator _salaryCalculator;

    public SalaryCalculator(ISalaryCalculator salaryCalculator) {
        _salaryCalculator = salaryCalculator;
    }

    public void SetCalculator(ISalaryCalculator salaryCalculator) => _salaryCalculator = salaryCalculator;

    public double CalculateSalary(IEnumerable<DeveloperReport> reports) => _salaryCalculator.CalculateTotalSalary(reports);
}
// In this context object, we provide initialization of the strategy object with the constructor in a 
// compile-time or with the SetCalculator method in the application’s runtime

// Main
public class Run
{
    public static void Main(string[] args)
    {
        var reports = new List<DeveloperReport>
        {
            new DeveloperReport {Id = 1, Name = "Dev1", Level = DeveloperLevel.Senior, HourlyRate = 30.5, WorkingHours = 160 },
            new DeveloperReport { Id = 2, Name = "Dev2", Level = DeveloperLevel.Junior, HourlyRate = 20, WorkingHours = 120 },
            new DeveloperReport { Id = 3, Name = "Dev3", Level = DeveloperLevel.Senior, HourlyRate = 32.5, WorkingHours = 130 },
            new DeveloperReport { Id = 4, Name = "Dev4", Level = DeveloperLevel.Junior, HourlyRate = 24.5, WorkingHours = 140 }
        };

        var calculatorContext = new SalaryCalculator(new JuniorDevSalaryCalculator());
        var juniorTotal = calculatorContext.CalculateSalary(reports);
        Console.WriteLine($"Total amount for junior salaries is: {juniorTotal}");

        calculatorContext.SetCalculator(new SeniorDevSalaryCalculator());
        var seniorTotal = calculatorContext.CalculateSalary(reports);
        Console.WriteLine($"Total amount for senior salaries is: {seniorTotal}");

        Console.WriteLine($"Total cost for all the salaries is: {juniorTotal+seniorTotal}");
    }
}

/* 
We should use this pattern whenever we have different variations for some functionality in an object 
and we want to switch from one variation to another in a runtime. Furthermore, if we have similar classes 
in our project that only differ on how they execute some behavior, the Strategy pattern should be the 
right choice for us.

We should consider introducing this pattern in situations where a single class has multiple conditions over 
different variations of the same functionality. That’s because the Strategy pattern lets us extract those 
variations into separate classes (concrete strategies). Then we can invoke them into the context class. 
*/
