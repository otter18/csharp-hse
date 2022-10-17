// Solution: hse - sem-2022-10-12 - ConsoleEngineUtils.cs
// Created at 2022-10-17 12:27
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_12;

using ConsoleCommands;

public static partial class ConsoleEngine
{
    private static string RenderAutoComplete(string inp)
    {
        var args = inp.Split();
        var targetDir = args[^1].Contains('/')
            ? new DirectoryInfo(_currentDir.FullName + "/" + args[^1][..args[^1].LastIndexOf('/')])
            : _currentDir;

        var res = _commands.Keys.Where(command => command.StartsWith(args[^1]));
        if (targetDir.Exists)
        {
            res = res.Concat(
                targetDir.GetFiles()
                    .Where(fileName => FormatFilePath(fileName.FullName, _currentDir).StartsWith(args[^1]))
                    .Select(fileName => FormatFilePath(fileName.FullName, _currentDir))
            );
            res = res.Concat(
                targetDir.GetDirectories()
                    .Where(dirName => FormatDirPath(dirName, _currentDir).StartsWith(args[^1]))
                    .Select(dirName => FormatDirPath(dirName, _currentDir))
            );
        }

        if (res.Count() != 1)
        {
            return inp;
        }

        args[^1] = res.First();
        return string.Join(" ", args);
    }

    private static void Erase(int cnt)
    {
        for (var i = 0; i < cnt; i++)
        {
            Console.Write("\b \b");
        }
    }

    private static string FormatFilePath(string filePath, DirectoryInfo rootDir)
    {
        return filePath.Replace((rootDir.FullName ?? "") + "/", "");
    }

    private static string FormatDirPath(DirectoryInfo directoryInfo, DirectoryInfo rootDir)
    {
        return directoryInfo.FullName.Replace((rootDir.FullName ?? "") + "/", "");
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