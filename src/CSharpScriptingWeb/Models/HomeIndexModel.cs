namespace CSharpScriptingWeb.Models;

public class HomeIndexModel
{
    public bool PeopleEqual { get; set; }
    public string OriginalPersonJson { get; set; } = null!;
    public string ScriptedPersonJson { get; set; } = null!;
    public string ScriptText { get; set; } = null!;
    public List<string> CompilationErrors { get; private set; } = new();


    //
    // Timings
    //

    public TimeSpan CreateScriptOptionsElapsed { get; set; }
    public TimeSpan CreateScriptElapsed { get; set; }
    public TimeSpan CompileElapsed { get; set; }
    public TimeSpan RunElapsed { get; set; }
}
