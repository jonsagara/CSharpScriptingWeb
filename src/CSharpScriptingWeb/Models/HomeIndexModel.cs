namespace CSharpScriptingWeb.Models;

public class HomeIndexModel
{
    public bool PeopleEqual { get; set; }
    public string OriginalPersonJson { get; set; } = null!;
    public string ScriptedPersonJson { get; set; } = null!;

    //
    // Timings
    //

    public TimeSpan ScriptCompilationElapsed { get; set; }
}
