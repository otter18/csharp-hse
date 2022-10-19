// Solution: hse - sem-2022-10-12 - ConsoleEngineUtils.cs
// Created at 2022-10-17 12:27
// Author: Черных Владимир
// Group: БПИ229

namespace sem_2022_10_12;

public static partial class ConsoleEngine
{
    private static string RenderAutoComplete(string inp)
    {
        var args = inp.Split();
        var targetDir = args[^1].Contains(Path.DirectorySeparatorChar)
            ? new DirectoryInfo(_currentDir.FullName + Path.DirectorySeparatorChar +
                                args[^1][..args[^1].LastIndexOf(Path.DirectorySeparatorChar)])
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
        return filePath.Replace(rootDir.FullName + Path.DirectorySeparatorChar, "");
    }

    private static string FormatDirPath(DirectoryInfo directoryInfo, DirectoryInfo rootDir)
    {
        return directoryInfo.FullName.Replace(rootDir.FullName + Path.DirectorySeparatorChar, "");
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

    private static void WriteLine(string s, ConsoleColor? consoleColor = null)
    {
        if (consoleColor is not null)
        {
            Console.ForegroundColor = (ConsoleColor)consoleColor;
        }

        Console.WriteLine(s);
        Console.ResetColor();
    }

    private static void Write(string s, ConsoleColor? consoleColor = null)
    {
        if (consoleColor is not null)
        {
            Console.ForegroundColor = (ConsoleColor)consoleColor;
        }

        Console.Write(s);
        Console.ResetColor();
    }

    private static void OutputResult(string result, ConsoleColor? consoleColor = null, DirectoryInfo? outFile = null)
    {
        // ignore empty result
        if (result == "")
        {
            return;
        }

        // output to console if outFile is null
        if (outFile is null)
        {
            WriteLine(result, consoleColor);
            return;
        }

        // otherwise write to file
        File.WriteAllText(outFile.FullName, result);
    }

    private static DirectoryInfo GetFullPath(string path)
    {
        return path.StartsWith(Path.DirectorySeparatorChar)
            ? new DirectoryInfo(Path.Combine(RootDir.FullName, path[1..]))
            : new DirectoryInfo(Path.Combine(_currentDir.FullName, path));
    }

    /// <summary>
    /// Checks if the root directory of the ConsoleEngine is parent
    /// of the given directory.
    /// </summary>
    public static bool IsInRoot(DirectoryInfo directoryToCheck)
    {
        return directoryToCheck.FullName.StartsWith(ConsoleEngine.RootDir.FullName);
    }
}