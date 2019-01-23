using System;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommandAttribute : Attribute
{

    private string command;
    public string CommandName { get { return command; } }

    private object[] defaultParameters;
    public object[] DefaultParameters { get { return defaultParameters; } }

    /// <param name="command">
    /// The command required to call to execute this function.
    /// </param>
    public ConsoleCommandAttribute(string command)
    {
        this.command = command;
    }

    /// <param name="command">
    /// The command required to call to execute this function.
    /// </param>
    /// <param name="defaultParameters">
    /// Default parameters for this function. These will get used if this command get's called without any parameters.
    /// </param>
    public ConsoleCommandAttribute(string command, params object[] defaultParameters)
    {
        this.command = command;
        this.defaultParameters = defaultParameters;
    }
}
