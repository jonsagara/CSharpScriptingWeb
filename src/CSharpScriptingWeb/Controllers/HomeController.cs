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
            var scriptOptions = ScriptOptions.Default
                .WithImports(_importNamespaces)
                .WithReferences(Assembly.GetExecutingAssembly());

            var script = CSharpScript.Create<Person>(
                TheScript,
                globalsType: typeof(Person),
                options: scriptOptions
            );

            var swCompileStarted = Stopwatch.GetTimestamp();
            var compileDiagnostics = script.Compile();
            var swCompileElapsed = Stopwatch.GetElapsedTime(swCompileStarted);
            _logger.LogInformation($"Compiling a {nameof(CSharpScript)} took: {{swCompileElapsed}}.", swCompileElapsed);

            var personToCopy = CreatePersonToCopy();
            var result = await script.RunAsync(personToCopy);

            model.PeopleEqual = personToCopy == result.ReturnValue;
            model.OriginalPersonJson = JsonSerializer.Serialize(personToCopy, _jsonSerializerOptions);
            model.ScriptedPersonJson = JsonSerializer.Serialize(result.ReturnValue, _jsonSerializerOptions);
            model.ScriptCompilationElapsed = swCompileElapsed;
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
