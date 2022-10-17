// Solution: hse - sem-2022-10-12 - TreeConsoleCommand.cs
// Created at 2022-10-17 17:03
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class TreeConsoleCommand : IConsoleCommand
{
    private static void RecursiveRender(ref string res, DirectoryInfo currentDir, int depth = -1, int currDepth = 0)
    {
        if (depth != -1 && depth < currDepth)
        {
            return;
        }

        foreach (var dir in currentDir.GetDirectories())
        {
            res += $"{string.Concat(Enumerable.Repeat("│  ", currDepth))}├─ {dir.Name}/\n";
            RecursiveRender(ref res, dir, depth, currDepth + 1);
        }

        foreach (var file in currentDir.GetFiles())
        {
            res += $"{string.Concat(Enumerable.Repeat("│  ", currDepth))}├─ {file.Name}\n";
        }
    }

    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var args = inpCommand.Split();
        var res = "";
        if (args.Length == 2 && int.TryParse(args[1], out var depth))
        {
            RecursiveRender(ref res, currentDir, depth);
        }
        else
        {
            RecursiveRender(ref res, currentDir);
        }

        return new ConsoleState()
        {
            Result = res,
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        return "tree <depth: 1, 2 ... n>\nOutputs file tree. Depth can be specified.";
    }
}