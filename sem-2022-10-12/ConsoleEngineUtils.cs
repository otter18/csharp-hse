// Solution: hse - sem-2022-10-12 - ConsoleEngineUtils.cs
// Created at 2022-10-17 12:27
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_12;

using ConsoleCommands;

public static partial class ConsoleEngine
{
    private static string AutoCompleteCommand(string inp)
    {
        // TODO: autocomplete
        throw new NotImplementedException();
    }

    private static void Erase(int cnt)
    {
        for (var i = 0; i < cnt; i++)
        {
            Console.Write("\b \b");
        }
    }

    private static string FormatDirPath(DirectoryInfo directoryInfo)
    {
        return directoryInfo.FullName.Replace((RootDir.Parent?.FullName ?? "") + "/", "");
    }

    private static string RenderHelpMessage(string inpCommand)
    {
        var args = inpCommand.Split();

        switch (args.Length)
        {
            // output all help messages
            case 1:
                var s = "";
                foreach (var (key, val) in _commands)
                {
                    s += $"---------- {key} ----------\n{val.GetHelpMessage()}\n\n";
                }

                return s;
            // help message for specified command
            case 2 when _commands.ContainsKey(args[1]):
                return _commands[args[1]].GetHelpMessage();
            // specified command not found
            case 2:
                throw new CommandErrorException($"Given command {args[1]} doesn't exit.\n" +
                                                $"Available commands: {string.Join(", ", _commands.Keys)}");
            // syntax error
            case > 2:
                throw new CommandErrorException($"Syntax error. Enter help or help <command>");
        }

        return "";
    }
}

internal class CommandErrorException : ApplicationException
{
    public CommandErrorException() : base("Undefined error! Try again")
    {
    }

    public CommandErrorException(string message) : base(message)
    {
    }

    public CommandErrorException(string message, Exception inner) : base(message, inner)
    {
    }
}