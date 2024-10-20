// Adapter Design Pattern - Structural Design Pattern

/*

The Adapter design pattern is a structural pattern that allows incompatible interfaces 
to work together. By doing so, we allow objects from different interfaces to exchange data.

*/

/*
Scenario:
---------
Let’s imagine that we have functionality in which we convert the list of car manufacturers 
into JSON format and write it to the screen. But instead of a list, we have been provided with 
an API that provides us with all the manufacturers in the XML format.

Let’s say we can’t modify the existing API functionality (because of the technical restrictions 
such as being imported into our project from another solution that we mustn’t modify or as a 
NuGet package) so we have to find a way around it.

And the proper way to do it is to implement the Adapter pattern to solve this problem.
*/

using System;
using Newtonsoft.Json;

// Model
public class Manufacturer
{
    public string Name { get; set; }
    public string City { get; set; }
    public int Year { get; set; }
}

// Data 
public static class ManufacturerDataProvider
{
    public List<Manufacturer> GetData() =>
       new List<Manufacturer> {
            new Manufacturer { City = "Italy", Name = "Alfa Romeo", Year = 2016 },
            new Manufacturer { City = "UK", Name = "Aston Martin", Year = 2018 },
            new Manufacturer { City = "USA", Name = "Dodge", Year = 2017 },
            new Manufacturer { City = "Japan", Name = "Subaru", Year = 2016 },
            new Manufacturer { City = "Germany", Name = "BMW", Year = 2015 }
       };
}

// XML Converter
public class XmlConverter
{
    public XDocument GetXML() {
        var xDocument = new XDocument();
        var xElement = new XElement("Manufacturers");
        var xAttributes = ManufacturerDataProvider.GetData()
            .Select(m => new XElement("Manufacturer", 
                                new XAttribute("City", m.City),
                                new XAttribute("Name", m.Name),
                                new XAttribute("Year", m.Year)));

        xElement.Add(xAttributes);
        xDocument.Add(xElement);

        Console.WriteLine(xDocument);

        return xDocument;
    }
}

//----------------- (Existing code ends here) ----------------

// JsonConverter class - Without 'Adapter'. (Not actual solution)
public class JsonConverter
{
    private IEnumerable<Manufacturer> _manufacturers;

    public JsonConverter(IEnumerable<Manufacturer> manufacturers) {
        _manufacturers = manufacturers;
    }

    public void ConvertToJson() {
        var jsonManufacturers = JsonConvert.SerializeObject(_manufacturers, Formatting.Indented);

        Console.WriteLine("\nPrinting JSON list\n");
        Console.WriteLine(jsonManufacturers);
    }
} 

/* Here we need to pass the actual manufacturers list. But how to combine those two interfaces 
to accomplish our task, which is converting manufacturers from XML to JSON format..?? */

// Adapter Implementation
public interface IXmlToJson
{
    void ConvertXmlToJson();
}

public class XmlToJsonAdapter : IXmlToJson
{
    private readonly XmlConverter _xmlConverter;

    public XmlToJsonAdapter(XmlConverter xmlConverter) {
        _xmlConverter = xmlConverter;
    }

    public void ConvertXmlToJson() {
        var manufacturers = _xmlConverter.GetXML()
                .Element("Manufacturers")
                .Elements("Manufacturer")
                .Select(m => new Manufacturer
                             {
                                City = m.Attribute("City").Value,
                                Name = m.Attribute("Name").Value,
                                Year = Convert.ToInt32(m.Attribute("Year").Value)
                             });

        new JsonConverter(manufacturers).ConvertToJson();
    }
}

public class Run
{
   public static void Main(string[] args)
    {
        var xmlConverter = new XmlConverter();
        var xmlDocument = xmlConverter.GetXML();
        // using adapter to convert xml to json.
        var adapter = new XmlToJsonAdapter(xmlConverter);
        var jsonDocument = adapter.ConvertXmlToJson();
    }
}

/* We have enabled collaboration between two completely different interfaces by 
just introducing an adapter class to our project. */

/*
When to Use Adapter?
--------
We should use the Adapter class whenever we want to work with the existing class but 
its interface is not compatible with the rest of our code. Basically, the Adapter pattern 
is a middle-layer which serves as a translator between the code implemented in our project 
and some third party class or any other class with a different interface.

Furthermore, we should use the Adapter when we want to reuse existing classes from our project 
but they lack a common functionality. By using the Adapter pattern in this case, we don’t need 
to extend each class separately and create a redundant code.

The Adapter pattern is pretty common in the C# world and it is quite used when we have to adapt 
some existing classes to a new interface. It can increase a code complexity by adding additional 
classes (adapters) but it is worth an effort for sure.
*/