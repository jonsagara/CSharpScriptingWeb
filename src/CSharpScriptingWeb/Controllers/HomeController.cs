using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using CSharpScriptingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpScriptingWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        HomeIndexModel model = new();

        try
        {
            // Add namespaces and references.
            var createScriptOptionsStarted = Stopwatch.GetTimestamp();
            var scriptOptions = ScriptOptions.Default
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithImports(_importNamespaces)
                .WithReferences(Assembly.GetExecutingAssembly());
            model.CreateScriptOptionsElapsed = Stopwatch.GetElapsedTime(createScriptOptionsStarted);

            // Create the C# script object. Force it to be a new string.
            var createScriptStarted = Stopwatch.GetTimestamp();
            var script = CSharpScript.Create<Person>(
                $"{TheScript}",
                globalsType: typeof(Person),
                options: scriptOptions
                );
            model.CreateScriptElapsed = Stopwatch.GetElapsedTime(createScriptStarted);

            // Compile the C# script.
            var compileStarted = Stopwatch.GetTimestamp();
            var compileDiagnostics = script.Compile();
            model.CompileElapsed = Stopwatch.GetElapsedTime(compileStarted);
            _logger.LogInformation($"Compiling a {nameof(CSharpScript)} took: {{swCompileElapsed}}.", model.CompileElapsed);
            model.CompilationErrors.AddRange(compileDiagnostics.Select(d => d.ToString()));

            var personToCopy = CreatePersonToCopy();

            // Run the C# script, passing it data.
            var runStarted = Stopwatch.GetTimestamp();
            var result = await script.RunAsync(personToCopy);
            model.RunElapsed = Stopwatch.GetElapsedTime(runStarted);

            // Populate non-timing results.
            model.PeopleEqual = personToCopy == result.ReturnValue;
            model.OriginalPersonJson = JsonSerializer.Serialize(personToCopy, _jsonSerializerOptions);
            model.ScriptedPersonJson = JsonSerializer.Serialize(result.ReturnValue, _jsonSerializerOptions);
            model.ScriptText = TheScript;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in Home.Index");
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    //
    // Private methods
    //

    private Person CreatePersonToCopy()
    {
        return new Person
        {
            LastName = "Rizzo",
            FirstName = "Frank",
            MiddleName = "R-I-Z-Z-O",
            BirthDate = "05/04/1972",
            Address = "1234 Any Street",
            City = "New York City",
            State = "NY",
            Zip = "10001",
            County = "New York County",
            Social = "555-44-3333",
            Phone = "555-555-5555",
            Pounds = 165,
            Feet = 5,
            Inches = 6,
            BirthCity = "Trenton",
            BirthState = "NJ",
            Sex = "Male",
        };
    }


    //
    // Stuff
    //

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    private static readonly string[] _importNamespaces = new[]
    {
        "System",
        "System.Collections.Generic",
        typeof(Person).Namespace!,
    };

    // Repeat elements to emulate the large script files we have.
    private const string TheScript = """
List<Person> people = new();

var person1 = new Person();
person1.LastName = LastName;
person1.FirstName = FirstName;
person1.MiddleName = MiddleName;
person1.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person1.Address = Address;
person1.City = City;
person1.State = State;
person1.Zip = Zip;
person1.County = County;
person1.Social = Social;
person1.Phone = Phone;
person1.Pounds = Pounds;
person1.Feet = Feet;
person1.Inches = Inches;
person1.BirthCity = BirthCity;
person1.BirthState = BirthState;
person1.Sex = Sex;
people.Add(person1);

var person2 = new Person();
person2.LastName = LastName;
person2.FirstName = FirstName;
person2.MiddleName = MiddleName;
person2.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person2.Address = Address;
person2.City = City;
person2.State = State;
person2.Zip = Zip;
person2.County = County;
person2.Social = Social;
person2.Phone = Phone;
person2.Pounds = Pounds;
person2.Feet = Feet;
person2.Inches = Inches;
person2.BirthCity = BirthCity;
person2.BirthState = BirthState;
person2.Sex = Sex;
people.Add(person2);

var person3 = new Person();
person3.LastName = LastName;
person3.FirstName = FirstName;
person3.MiddleName = MiddleName;
person3.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person3.Address = Address;
person3.City = City;
person3.State = State;
person3.Zip = Zip;
person3.County = County;
person3.Social = Social;
person3.Phone = Phone;
person3.Pounds = Pounds;
person3.Feet = Feet;
person3.Inches = Inches;
person3.BirthCity = BirthCity;
person3.BirthState = BirthState;
person3.Sex = Sex;
people.Add(person3);

var person4 = new Person();
person4.LastName = LastName;
person4.FirstName = FirstName;
person4.MiddleName = MiddleName;
person4.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person4.Address = Address;
person4.City = City;
person4.State = State;
person4.Zip = Zip;
person4.County = County;
person4.Social = Social;
person4.Phone = Phone;
person4.Pounds = Pounds;
person4.Feet = Feet;
person4.Inches = Inches;
person4.BirthCity = BirthCity;
person4.BirthState = BirthState;
person4.Sex = Sex;
people.Add(person4);

var person5 = new Person();
person5.LastName = LastName;
person5.FirstName = FirstName;
person5.MiddleName = MiddleName;
person5.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person5.Address = Address;
person5.City = City;
person5.State = State;
person5.Zip = Zip;
person5.County = County;
person5.Social = Social;
person5.Phone = Phone;
person5.Pounds = Pounds;
person5.Feet = Feet;
person5.Inches = Inches;
person5.BirthCity = BirthCity;
person5.BirthState = BirthState;
person5.Sex = Sex;
people.Add(person5);

var person6 = new Person();
person6.LastName = LastName;
person6.FirstName = FirstName;
person6.MiddleName = MiddleName;
person6.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person6.Address = Address;
person6.City = City;
person6.State = State;
person6.Zip = Zip;
person6.County = County;
person6.Social = Social;
person6.Phone = Phone;
person6.Pounds = Pounds;
person6.Feet = Feet;
person6.Inches = Inches;
person6.BirthCity = BirthCity;
person6.BirthState = BirthState;
person6.Sex = Sex;
people.Add(person6);

var person7 = new Person();
person7.LastName = LastName;
person7.FirstName = FirstName;
person7.MiddleName = MiddleName;
person7.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person7.Address = Address;
person7.City = City;
person7.State = State;
person7.Zip = Zip;
person7.County = County;
person7.Social = Social;
person7.Phone = Phone;
person7.Pounds = Pounds;
person7.Feet = Feet;
person7.Inches = Inches;
person7.BirthCity = BirthCity;
person7.BirthState = BirthState;
person7.Sex = Sex;
people.Add(person7);

var person8 = new Person();
person8.LastName = LastName;
person8.FirstName = FirstName;
person8.MiddleName = MiddleName;
person8.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person8.Address = Address;
person8.City = City;
person8.State = State;
person8.Zip = Zip;
person8.County = County;
person8.Social = Social;
person8.Phone = Phone;
person8.Pounds = Pounds;
person8.Feet = Feet;
person8.Inches = Inches;
person8.BirthCity = BirthCity;
person8.BirthState = BirthState;
person8.Sex = Sex;
people.Add(person8);

var person9 = new Person();
person9.LastName = LastName;
person9.FirstName = FirstName;
person9.MiddleName = MiddleName;
person9.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person9.Address = Address;
person9.City = City;
person9.State = State;
person9.Zip = Zip;
person9.County = County;
person9.Social = Social;
person9.Phone = Phone;
person9.Pounds = Pounds;
person9.Feet = Feet;
person9.Inches = Inches;
person9.BirthCity = BirthCity;
person9.BirthState = BirthState;
person9.Sex = Sex;
people.Add(person9);

var person10 = new Person();
person10.LastName = LastName;
person10.FirstName = FirstName;
person10.MiddleName = MiddleName;
person10.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person10.Address = Address;
person10.City = City;
person10.State = State;
person10.Zip = Zip;
person10.County = County;
person10.Social = Social;
person10.Phone = Phone;
person10.Pounds = Pounds;
person10.Feet = Feet;
person10.Inches = Inches;
person10.BirthCity = BirthCity;
person10.BirthState = BirthState;
person10.Sex = Sex;
people.Add(person10);

var person11 = new Person();
person11.LastName = LastName;
person11.FirstName = FirstName;
person11.MiddleName = MiddleName;
person11.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person11.Address = Address;
person11.City = City;
person11.State = State;
person11.Zip = Zip;
person11.County = County;
person11.Social = Social;
person11.Phone = Phone;
person11.Pounds = Pounds;
person11.Feet = Feet;
person11.Inches = Inches;
person11.BirthCity = BirthCity;
person11.BirthState = BirthState;
person11.Sex = Sex;
people.Add(person11);

var person12 = new Person();
person12.LastName = LastName;
person12.FirstName = FirstName;
person12.MiddleName = MiddleName;
person12.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person12.Address = Address;
person12.City = City;
person12.State = State;
person12.Zip = Zip;
person12.County = County;
person12.Social = Social;
person12.Phone = Phone;
person12.Pounds = Pounds;
person12.Feet = Feet;
person12.Inches = Inches;
person12.BirthCity = BirthCity;
person12.BirthState = BirthState;
person12.Sex = Sex;
people.Add(person12);

var person13 = new Person();
person13.LastName = LastName;
person13.FirstName = FirstName;
person13.MiddleName = MiddleName;
person13.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person13.Address = Address;
person13.City = City;
person13.State = State;
person13.Zip = Zip;
person13.County = County;
person13.Social = Social;
person13.Phone = Phone;
person13.Pounds = Pounds;
person13.Feet = Feet;
person13.Inches = Inches;
person13.BirthCity = BirthCity;
person13.BirthState = BirthState;
person13.Sex = Sex;
people.Add(person13);

var person14 = new Person();
person14.LastName = LastName;
person14.FirstName = FirstName;
person14.MiddleName = MiddleName;
person14.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person14.Address = Address;
person14.City = City;
person14.State = State;
person14.Zip = Zip;
person14.County = County;
person14.Social = Social;
person14.Phone = Phone;
person14.Pounds = Pounds;
person14.Feet = Feet;
person14.Inches = Inches;
person14.BirthCity = BirthCity;
person14.BirthState = BirthState;
person14.Sex = Sex;
people.Add(person14);

var person15 = new Person();
person15.LastName = LastName;
person15.FirstName = FirstName;
person15.MiddleName = MiddleName;
person15.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person15.Address = Address;
person15.City = City;
person15.State = State;
person15.Zip = Zip;
person15.County = County;
person15.Social = Social;
person15.Phone = Phone;
person15.Pounds = Pounds;
person15.Feet = Feet;
person15.Inches = Inches;
person15.BirthCity = BirthCity;
person15.BirthState = BirthState;
person15.Sex = Sex;
people.Add(person15);

var person16 = new Person();
person16.LastName = LastName;
person16.FirstName = FirstName;
person16.MiddleName = MiddleName;
person16.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person16.Address = Address;
person16.City = City;
person16.State = State;
person16.Zip = Zip;
person16.County = County;
person16.Social = Social;
person16.Phone = Phone;
person16.Pounds = Pounds;
person16.Feet = Feet;
person16.Inches = Inches;
person16.BirthCity = BirthCity;
person16.BirthState = BirthState;
person16.Sex = Sex;
people.Add(person16);

var person17 = new Person();
person17.LastName = LastName;
person17.FirstName = FirstName;
person17.MiddleName = MiddleName;
person17.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person17.Address = Address;
person17.City = City;
person17.State = State;
person17.Zip = Zip;
person17.County = County;
person17.Social = Social;
person17.Phone = Phone;
person17.Pounds = Pounds;
person17.Feet = Feet;
person17.Inches = Inches;
person17.BirthCity = BirthCity;
person17.BirthState = BirthState;
person17.Sex = Sex;
people.Add(person17);

var person18 = new Person();
person18.LastName = LastName;
person18.FirstName = FirstName;
person18.MiddleName = MiddleName;
person18.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person18.Address = Address;
person18.City = City;
person18.State = State;
person18.Zip = Zip;
person18.County = County;
person18.Social = Social;
person18.Phone = Phone;
person18.Pounds = Pounds;
person18.Feet = Feet;
person18.Inches = Inches;
person18.BirthCity = BirthCity;
person18.BirthState = BirthState;
person18.Sex = Sex;
people.Add(person18);

var person19 = new Person();
person19.LastName = LastName;
person19.FirstName = FirstName;
person19.MiddleName = MiddleName;
person19.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person19.Address = Address;
person19.City = City;
person19.State = State;
person19.Zip = Zip;
person19.County = County;
person19.Social = Social;
person19.Phone = Phone;
person19.Pounds = Pounds;
person19.Feet = Feet;
person19.Inches = Inches;
person19.BirthCity = BirthCity;
person19.BirthState = BirthState;
person19.Sex = Sex;
people.Add(person19);

var person20 = new Person();
person20.LastName = LastName;
person20.FirstName = FirstName;
person20.MiddleName = MiddleName;
person20.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person20.Address = Address;
person20.City = City;
person20.State = State;
person20.Zip = Zip;
person20.County = County;
person20.Social = Social;
person20.Phone = Phone;
person20.Pounds = Pounds;
person20.Feet = Feet;
person20.Inches = Inches;
person20.BirthCity = BirthCity;
person20.BirthState = BirthState;
person20.Sex = Sex;
people.Add(person20);

var person21 = new Person();
person21.LastName = LastName;
person21.FirstName = FirstName;
person21.MiddleName = MiddleName;
person21.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person21.Address = Address;
person21.City = City;
person21.State = State;
person21.Zip = Zip;
person21.County = County;
person21.Social = Social;
person21.Phone = Phone;
person21.Pounds = Pounds;
person21.Feet = Feet;
person21.Inches = Inches;
person21.BirthCity = BirthCity;
person21.BirthState = BirthState;
person21.Sex = Sex;
people.Add(person21);

var person22 = new Person();
person22.LastName = LastName;
person22.FirstName = FirstName;
person22.MiddleName = MiddleName;
person22.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person22.Address = Address;
person22.City = City;
person22.State = State;
person22.Zip = Zip;
person22.County = County;
person22.Social = Social;
person22.Phone = Phone;
person22.Pounds = Pounds;
person22.Feet = Feet;
person22.Inches = Inches;
person22.BirthCity = BirthCity;
person22.BirthState = BirthState;
person22.Sex = Sex;
people.Add(person22);

var person23 = new Person();
person23.LastName = LastName;
person23.FirstName = FirstName;
person23.MiddleName = MiddleName;
person23.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person23.Address = Address;
person23.City = City;
person23.State = State;
person23.Zip = Zip;
person23.County = County;
person23.Social = Social;
person23.Phone = Phone;
person23.Pounds = Pounds;
person23.Feet = Feet;
person23.Inches = Inches;
person23.BirthCity = BirthCity;
person23.BirthState = BirthState;
person23.Sex = Sex;
people.Add(person23);

var person24 = new Person();
person24.LastName = LastName;
person24.FirstName = FirstName;
person24.MiddleName = MiddleName;
person24.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person24.Address = Address;
person24.City = City;
person24.State = State;
person24.Zip = Zip;
person24.County = County;
person24.Social = Social;
person24.Phone = Phone;
person24.Pounds = Pounds;
person24.Feet = Feet;
person24.Inches = Inches;
person24.BirthCity = BirthCity;
person24.BirthState = BirthState;
person24.Sex = Sex;
people.Add(person24);

var person25 = new Person();
person25.LastName = LastName;
person25.FirstName = FirstName;
person25.MiddleName = MiddleName;
person25.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person25.Address = Address;
person25.City = City;
person25.State = State;
person25.Zip = Zip;
person25.County = County;
person25.Social = Social;
person25.Phone = Phone;
person25.Pounds = Pounds;
person25.Feet = Feet;
person25.Inches = Inches;
person25.BirthCity = BirthCity;
person25.BirthState = BirthState;
person25.Sex = Sex;
people.Add(person25);

var person26 = new Person();
person26.LastName = LastName;
person26.FirstName = FirstName;
person26.MiddleName = MiddleName;
person26.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person26.Address = Address;
person26.City = City;
person26.State = State;
person26.Zip = Zip;
person26.County = County;
person26.Social = Social;
person26.Phone = Phone;
person26.Pounds = Pounds;
person26.Feet = Feet;
person26.Inches = Inches;
person26.BirthCity = BirthCity;
person26.BirthState = BirthState;
person26.Sex = Sex;
people.Add(person26);

var person27 = new Person();
person27.LastName = LastName;
person27.FirstName = FirstName;
person27.MiddleName = MiddleName;
person27.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person27.Address = Address;
person27.City = City;
person27.State = State;
person27.Zip = Zip;
person27.County = County;
person27.Social = Social;
person27.Phone = Phone;
person27.Pounds = Pounds;
person27.Feet = Feet;
person27.Inches = Inches;
person27.BirthCity = BirthCity;
person27.BirthState = BirthState;
person27.Sex = Sex;
people.Add(person27);

var person28 = new Person();
person28.LastName = LastName;
person28.FirstName = FirstName;
person28.MiddleName = MiddleName;
person28.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person28.Address = Address;
person28.City = City;
person28.State = State;
person28.Zip = Zip;
person28.County = County;
person28.Social = Social;
person28.Phone = Phone;
person28.Pounds = Pounds;
person28.Feet = Feet;
person28.Inches = Inches;
person28.BirthCity = BirthCity;
person28.BirthState = BirthState;
person28.Sex = Sex;
people.Add(person28);

var person29 = new Person();
person29.LastName = LastName;
person29.FirstName = FirstName;
person29.MiddleName = MiddleName;
person29.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person29.Address = Address;
person29.City = City;
person29.State = State;
person29.Zip = Zip;
person29.County = County;
person29.Social = Social;
person29.Phone = Phone;
person29.Pounds = Pounds;
person29.Feet = Feet;
person29.Inches = Inches;
person29.BirthCity = BirthCity;
person29.BirthState = BirthState;
person29.Sex = Sex;
people.Add(person29);

var person30 = new Person();
person30.LastName = LastName;
person30.FirstName = FirstName;
person30.MiddleName = MiddleName;
person30.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person30.Address = Address;
person30.City = City;
person30.State = State;
person30.Zip = Zip;
person30.County = County;
person30.Social = Social;
person30.Phone = Phone;
person30.Pounds = Pounds;
person30.Feet = Feet;
person30.Inches = Inches;
person30.BirthCity = BirthCity;
person30.BirthState = BirthState;
person30.Sex = Sex;
people.Add(person30);

var person31 = new Person();
person31.LastName = LastName;
person31.FirstName = FirstName;
person31.MiddleName = MiddleName;
person31.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person31.Address = Address;
person31.City = City;
person31.State = State;
person31.Zip = Zip;
person31.County = County;
person31.Social = Social;
person31.Phone = Phone;
person31.Pounds = Pounds;
person31.Feet = Feet;
person31.Inches = Inches;
person31.BirthCity = BirthCity;
person31.BirthState = BirthState;
person31.Sex = Sex;
people.Add(person31);

var person32 = new Person();
person32.LastName = LastName;
person32.FirstName = FirstName;
person32.MiddleName = MiddleName;
person32.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person32.Address = Address;
person32.City = City;
person32.State = State;
person32.Zip = Zip;
person32.County = County;
person32.Social = Social;
person32.Phone = Phone;
person32.Pounds = Pounds;
person32.Feet = Feet;
person32.Inches = Inches;
person32.BirthCity = BirthCity;
person32.BirthState = BirthState;
person32.Sex = Sex;
people.Add(person32);

var person33 = new Person();
person33.LastName = LastName;
person33.FirstName = FirstName;
person33.MiddleName = MiddleName;
person33.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person33.Address = Address;
person33.City = City;
person33.State = State;
person33.Zip = Zip;
person33.County = County;
person33.Social = Social;
person33.Phone = Phone;
person33.Pounds = Pounds;
person33.Feet = Feet;
person33.Inches = Inches;
person33.BirthCity = BirthCity;
person33.BirthState = BirthState;
person33.Sex = Sex;
people.Add(person33);

var person34 = new Person();
person34.LastName = LastName;
person34.FirstName = FirstName;
person34.MiddleName = MiddleName;
person34.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person34.Address = Address;
person34.City = City;
person34.State = State;
person34.Zip = Zip;
person34.County = County;
person34.Social = Social;
person34.Phone = Phone;
person34.Pounds = Pounds;
person34.Feet = Feet;
person34.Inches = Inches;
person34.BirthCity = BirthCity;
person34.BirthState = BirthState;
person34.Sex = Sex;
people.Add(person34);

var person35 = new Person();
person35.LastName = LastName;
person35.FirstName = FirstName;
person35.MiddleName = MiddleName;
person35.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person35.Address = Address;
person35.City = City;
person35.State = State;
person35.Zip = Zip;
person35.County = County;
person35.Social = Social;
person35.Phone = Phone;
person35.Pounds = Pounds;
person35.Feet = Feet;
person35.Inches = Inches;
person35.BirthCity = BirthCity;
person35.BirthState = BirthState;
person35.Sex = Sex;
people.Add(person35);

var person36 = new Person();
person36.LastName = LastName;
person36.FirstName = FirstName;
person36.MiddleName = MiddleName;
person36.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person36.Address = Address;
person36.City = City;
person36.State = State;
person36.Zip = Zip;
person36.County = County;
person36.Social = Social;
person36.Phone = Phone;
person36.Pounds = Pounds;
person36.Feet = Feet;
person36.Inches = Inches;
person36.BirthCity = BirthCity;
person36.BirthState = BirthState;
person36.Sex = Sex;
people.Add(person36);

var person37 = new Person();
person37.LastName = LastName;
person37.FirstName = FirstName;
person37.MiddleName = MiddleName;
person37.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person37.Address = Address;
person37.City = City;
person37.State = State;
person37.Zip = Zip;
person37.County = County;
person37.Social = Social;
person37.Phone = Phone;
person37.Pounds = Pounds;
person37.Feet = Feet;
person37.Inches = Inches;
person37.BirthCity = BirthCity;
person37.BirthState = BirthState;
person37.Sex = Sex;
people.Add(person37);

var person38 = new Person();
person38.LastName = LastName;
person38.FirstName = FirstName;
person38.MiddleName = MiddleName;
person38.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person38.Address = Address;
person38.City = City;
person38.State = State;
person38.Zip = Zip;
person38.County = County;
person38.Social = Social;
person38.Phone = Phone;
person38.Pounds = Pounds;
person38.Feet = Feet;
person38.Inches = Inches;
person38.BirthCity = BirthCity;
person38.BirthState = BirthState;
person38.Sex = Sex;
people.Add(person38);

var person39 = new Person();
person39.LastName = LastName;
person39.FirstName = FirstName;
person39.MiddleName = MiddleName;
person39.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person39.Address = Address;
person39.City = City;
person39.State = State;
person39.Zip = Zip;
person39.County = County;
person39.Social = Social;
person39.Phone = Phone;
person39.Pounds = Pounds;
person39.Feet = Feet;
person39.Inches = Inches;
person39.BirthCity = BirthCity;
person39.BirthState = BirthState;
person39.Sex = Sex;
people.Add(person39);

var person40 = new Person();
person40.LastName = LastName;
person40.FirstName = FirstName;
person40.MiddleName = MiddleName;
person40.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person40.Address = Address;
person40.City = City;
person40.State = State;
person40.Zip = Zip;
person40.County = County;
person40.Social = Social;
person40.Phone = Phone;
person40.Pounds = Pounds;
person40.Feet = Feet;
person40.Inches = Inches;
person40.BirthCity = BirthCity;
person40.BirthState = BirthState;
person40.Sex = Sex;
people.Add(person40);

var person41 = new Person();
person41.LastName = LastName;
person41.FirstName = FirstName;
person41.MiddleName = MiddleName;
person41.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person41.Address = Address;
person41.City = City;
person41.State = State;
person41.Zip = Zip;
person41.County = County;
person41.Social = Social;
person41.Phone = Phone;
person41.Pounds = Pounds;
person41.Feet = Feet;
person41.Inches = Inches;
person41.BirthCity = BirthCity;
person41.BirthState = BirthState;
person41.Sex = Sex;
people.Add(person41);

var person42 = new Person();
person42.LastName = LastName;
person42.FirstName = FirstName;
person42.MiddleName = MiddleName;
person42.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person42.Address = Address;
person42.City = City;
person42.State = State;
person42.Zip = Zip;
person42.County = County;
person42.Social = Social;
person42.Phone = Phone;
person42.Pounds = Pounds;
person42.Feet = Feet;
person42.Inches = Inches;
person42.BirthCity = BirthCity;
person42.BirthState = BirthState;
person42.Sex = Sex;
people.Add(person42);

var person43 = new Person();
person43.LastName = LastName;
person43.FirstName = FirstName;
person43.MiddleName = MiddleName;
person43.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person43.Address = Address;
person43.City = City;
person43.State = State;
person43.Zip = Zip;
person43.County = County;
person43.Social = Social;
person43.Phone = Phone;
person43.Pounds = Pounds;
person43.Feet = Feet;
person43.Inches = Inches;
person43.BirthCity = BirthCity;
person43.BirthState = BirthState;
person43.Sex = Sex;
people.Add(person43);

var person44 = new Person();
person44.LastName = LastName;
person44.FirstName = FirstName;
person44.MiddleName = MiddleName;
person44.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person44.Address = Address;
person44.City = City;
person44.State = State;
person44.Zip = Zip;
person44.County = County;
person44.Social = Social;
person44.Phone = Phone;
person44.Pounds = Pounds;
person44.Feet = Feet;
person44.Inches = Inches;
person44.BirthCity = BirthCity;
person44.BirthState = BirthState;
person44.Sex = Sex;
people.Add(person44);

var person45 = new Person();
person45.LastName = LastName;
person45.FirstName = FirstName;
person45.MiddleName = MiddleName;
person45.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person45.Address = Address;
person45.City = City;
person45.State = State;
person45.Zip = Zip;
person45.County = County;
person45.Social = Social;
person45.Phone = Phone;
person45.Pounds = Pounds;
person45.Feet = Feet;
person45.Inches = Inches;
person45.BirthCity = BirthCity;
person45.BirthState = BirthState;
person45.Sex = Sex;
people.Add(person45);

var person46 = new Person();
person46.LastName = LastName;
person46.FirstName = FirstName;
person46.MiddleName = MiddleName;
person46.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person46.Address = Address;
person46.City = City;
person46.State = State;
person46.Zip = Zip;
person46.County = County;
person46.Social = Social;
person46.Phone = Phone;
person46.Pounds = Pounds;
person46.Feet = Feet;
person46.Inches = Inches;
person46.BirthCity = BirthCity;
person46.BirthState = BirthState;
person46.Sex = Sex;
people.Add(person46);

var person47 = new Person();
person47.LastName = LastName;
person47.FirstName = FirstName;
person47.MiddleName = MiddleName;
person47.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person47.Address = Address;
person47.City = City;
person47.State = State;
person47.Zip = Zip;
person47.County = County;
person47.Social = Social;
person47.Phone = Phone;
person47.Pounds = Pounds;
person47.Feet = Feet;
person47.Inches = Inches;
person47.BirthCity = BirthCity;
person47.BirthState = BirthState;
person47.Sex = Sex;
people.Add(person47);

var person48 = new Person();
person48.LastName = LastName;
person48.FirstName = FirstName;
person48.MiddleName = MiddleName;
person48.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person48.Address = Address;
person48.City = City;
person48.State = State;
person48.Zip = Zip;
person48.County = County;
person48.Social = Social;
person48.Phone = Phone;
person48.Pounds = Pounds;
person48.Feet = Feet;
person48.Inches = Inches;
person48.BirthCity = BirthCity;
person48.BirthState = BirthState;
person48.Sex = Sex;
people.Add(person48);

var person49 = new Person();
person49.LastName = LastName;
person49.FirstName = FirstName;
person49.MiddleName = MiddleName;
person49.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person49.Address = Address;
person49.City = City;
person49.State = State;
person49.Zip = Zip;
person49.County = County;
person49.Social = Social;
person49.Phone = Phone;
person49.Pounds = Pounds;
person49.Feet = Feet;
person49.Inches = Inches;
person49.BirthCity = BirthCity;
person49.BirthState = BirthState;
person49.Sex = Sex;
people.Add(person49);

var person50 = new Person();
person50.LastName = LastName;
person50.FirstName = FirstName;
person50.MiddleName = MiddleName;
person50.BirthDate = DateTime.Parse(BirthDate).ToShortDateString();
person50.Address = Address;
person50.City = City;
person50.State = State;
person50.Zip = Zip;
person50.County = County;
person50.Social = Social;
person50.Phone = Phone;
person50.Pounds = Pounds;
person50.Feet = Feet;
person50.Inches = Inches;
person50.BirthCity = BirthCity;
person50.BirthState = BirthState;
person50.Sex = Sex;
people.Add(person50);

return people[0];
""";
}
