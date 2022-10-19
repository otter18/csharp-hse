// Solution: hse - sem-2022-10-12 - MkdirConsoleCommand.cs
// Created at 2022-10-16 00:09
// Author: 
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class MkdirConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var cmd = inpCommand.Split();
        if (cmd.Length >= 2)
        {
            return MakeDir(cmd[1], currentDir);
        }
        
        throw new CommandErrorException("Wrong mkdir command");
    }

    private static ConsoleState MakeDir(string delPath, DirectoryInfo currentDir)
    {
        var truePath = new DirectoryInfo(
            delPath[0] == Path.DirectorySeparatorChar
                ? Path.Combine(ConsoleEngine.RootDir.FullName, delPath[1..])
                : Path.Combine(currentDir.FullName, delPath)
        );

        try
        {
            Directory.CreateDirectory(truePath.FullName);
        }
        catch (Exception e)
        {
            throw new CommandErrorException("Fatal error!", e);
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