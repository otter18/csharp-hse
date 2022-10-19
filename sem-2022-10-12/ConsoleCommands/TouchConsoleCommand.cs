// Solution: hse - sem-2022-10-12 - TouchConsoleCommand.cs
// Created at 2022-10-15 23:57
// Author: Рудинский Матвей 
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class TouchConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var cmd = inpCommand.Split();
        if (cmd.Length >= 2)
        {
            return MakeFile(cmd[1], currentDir);
        }

        throw new CommandErrorException("Wrong touch command");
    }

    private static ConsoleState MakeFile(string delPath, DirectoryInfo currentDir)
    {
        var truePath = new DirectoryInfo(
            delPath[0] == Path.DirectorySeparatorChar
                ? Path.Combine(ConsoleEngine.RootDir.FullName, delPath[1..])
                : Path.Combine(currentDir.FullName, delPath)
        );

        try
        {
            using var fileStream = File.Create(truePath.FullName);
        }
        catch (Exception e)
        {
            throw new CommandErrorException("Path doesn't exist!", e);
        }

        return new ConsoleState
        {
            Result = "",
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        return "touch <path> \n" +
               "Creating new file by given path";
    }
}