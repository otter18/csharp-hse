// Solution: hse - sem-2022-10-12 - MkdirConsoleCommand.cs
// Created at 2022-10-16 00:09
// Author: 
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class MkdirConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        string[]? cmd = inpCommand.Split();
        if (cmd is not null && cmd.Length >= 2)
        {
            return MakeDir(cmd[1], currentDir);
        }
        else
        {
            throw new CommandErrorException("Wrong mkdir command");
        }
    }

    public ConsoleState MakeDir(string delPath, DirectoryInfo currentDir)
    {

        string truePath = delPath[0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + delPath)
            : Path.GetFullPath(Path.Combine(currentDir.FullName, delPath));

        if(Directory.Exists(string.Join('\\', truePath.Split('\\')[0..^1])))
        {
            Directory.CreateDirectory(truePath);
        }
        else
        {
            throw new CommandErrorException("Path is not exist!");
        }

        return new ConsoleState()
        {
            Result = "",
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        return "mkdir <path> \n" +
               "Creating new directory by given path";
    }
}