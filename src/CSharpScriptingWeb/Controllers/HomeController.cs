using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using CSharpScriptingWeb.Models;
using Microsoft.AspNetCore.Mvc;
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
                .WithImports(_importNamespaces)
                .WithReferences(Assembly.GetExecutingAssembly());
            model.CreateScriptOptionsElapsed = Stopwatch.GetElapsedTime(createScriptOptionsStarted);

            // Create the C# script object.
            var createScriptStarted = Stopwatch.GetTimestamp();
            var script = CSharpScript.Create<Person>(
                TheScript,
                globalsType: typeof(Person),
                options: scriptOptions
                );
            model.CreateScriptElapsed = Stopwatch.GetElapsedTime(createScriptStarted);

            // Compile the C# script.
            var compileStarted = Stopwatch.GetTimestamp();
            var compileDiagnostics = script.Compile();
            model.CompileElapsed = Stopwatch.GetElapsedTime(compileStarted);
            _logger.LogInformation($"Compiling a {nameof(CSharpScript)} took: {{swCompileElapsed}}.", model.CompileElapsed);

            var personToCopy = CreatePersonToCopy();

            // Run the C# script, passing it data.
            var runStarted = Stopwatch.GetTimestamp();
            var result = await script.RunAsync(personToCopy);
            model.RunElapsed = Stopwatch.GetElapsedTime(runStarted);

            // Populate non-timing results.
            model.PeopleEqual = personToCopy == result.ReturnValue;
            model.OriginalPersonJson = JsonSerializer.Serialize(personToCopy, _jsonSerializerOptions);
            model.ScriptedPersonJson = JsonSerializer.Serialize(result.ReturnValue, _jsonSerializerOptions);
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
        typeof(Person).Namespace!,
    };

    private const string TheScript = """
var person = new Person();
                
person.LastName = LastName;
person.FirstName = FirstName;
person.MiddleName = MiddleName;
person.BirthDate = BirthDate;
person.Address = Address;
person.City = City;
person.State = State;
person.Zip = Zip;
person.County = County;
person.Social = Social;
person.Phone = Phone;
person.Pounds = Pounds;
person.Feet = Feet;
person.Inches = Inches;
person.BirthCity = BirthCity;
person.BirthState = BirthState;
person.Sex = Sex;

return person;
""";
}
