// Interpreter Design Pattern - Behavioral Design Pattern

/*

The Interpreter Design Pattern is a Behavioral Design Pattern that defines a grammatical representation of a 
language and provides an interpreter to deal with this grammar. The main idea is to define a domain language 
(a small language specific to your application) and interpret expressions in that language. It is useful when 
interpreting sentences in a language according to a defined grammar.

The classes and objects participating in this pattern include:
1. AbstractExpression: Declares an abstract Interpret operation common to all nodes in the abstract syntax tree.
2.TerminalExpression: Implements the Interpret operation associated with terminal symbols in the grammar. It’s 
an instance of AbstractExpression.
3. NonTerminalExpression: One level of grammar expression. For grammar rules that require multiple instances of 
AbstractExpression, you’d use the NonTerminalExpression.
4. Context: Contains global information and is typically defined outside the grammar.
5. Client: Builds (or is provided) the abstract syntax tree representing a particular sentence in the grammar. 
The tree is then evaluated by invoking the Interpret operation.

Usage examples:
The Interpreter Design Pattern Provides a way to evaluate language grammar or expression. This pattern is used 
in SQL Parsing, Symbol Processing Engines, Implementing Domain-Specific Languages (DSLs), Rule-Based Systems, 
Compilers and Interpreters, Syntax Trees for Expressions, etc.

*/

/*
Scenario:
---------
Suppose you want a date in MM-DD-YYYY format; then, you must pass the Context value and the Date Expression 
you want (i.e., MM-DD-YYYY) to the interpreter. What the interpreter will do is it will convert the context 
value into the date expression format you passed to it. we define a class for each type of grammar, such as 
Month, Year, Day, and separator. So, using this grammar, you can create any date format.
*/  

using System;
using System.Collections.Generic;

// Context
public class Context
{
    //The Expression Property is going to hold the Output
    public string Expression { get; set; }

    //The Date Property is going to hold the Input
    public DateTime Date { get; set; }

    public Context(DateTime date) {
        Date = date;
    }
}

// Abstract Expression
public interface IExpression 
{
    void Evaluate(Context context);
}

// Terminal Expressions starts here ----->

public class DayExpression : IExpression
{
    public void Evaluate(Context context) {
        string expression = context.Expression;
        context.Expression = expression.Replace("DD", context.Date.Day.ToString());
    }
}

public class MonthExpression : IExpression
{
    public void Evaluate(Context context) {
        string expression = context.Expression;
        context.Expression = expression.Replace("MM", context.Date.Month.ToString());
    }
}

public class YearExpression : IExpression
{
    public void Evaluate(Context context) {
        string expression = context.Expression;
        context.Expression = expression.Replace("YYYY", context.Date.Year.ToString());
    }
}

public class SeparatorExpression : IExpression
{
    public void Evaluate(Context context) {
        string expression = context.Expression;
        context.Expression = expression.Replace(" ", "-");
    }
}

// <------  Terminal Expressions ends here

// Main
public class Program
{
    public static void Main(string[] args) {
        //The following is going to be our Expression Tree
        List<IExpression> objExpressions = new List<IExpression>();

        Context context = new Context(DateTime.Now);
        Console.WriteLine("Please Select the Expression  : MM DD YYYY or YYYY MM DD or DD MM YYYY ");
        context.Expression = Console.ReadLine();

        //Split Expression which the user selects to an array to apply different Expression rules
        string[] strArray = context.Expression.Split(' ');

        // Adding the Appropriate Expression with the Expression Tree
        foreach (var item in strArray) {
            if (item == "DD") {
                objExpressions.Add(new DayExpression());
            }
            else if (item == "MM") {
                objExpressions.Add(new MonthExpression());
            }
            else if (item == "YYYY") {
                objExpressions.Add(new YearExpression());
            }
        }

        // Adding the SeparatorExpression
        objExpressions.Add(new SeparatorExpression());
        foreach (var obj in objExpressions) {
            obj.Evaluate(context);
        }

        Console.WriteLine(context.Expression);
    }
}

/*
Advantages:
-----------
1. Easy to Change and Extend Grammars: Grammars can be changed or extended by creating new expressions.
2. Separation of Concerns: Separates the grammar definition and interpretation logic from the main 
application logic.
3. Reusable Components: Individual grammar rules can be reused across the application.
*/