// Solution: hse - sem-2022-10-12 - TouchConsoleCommand.cs
// Created at 2022-10-15 23:57
// Author: $YOUR NAME$
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class TouchConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        string[]? cmd = inpCommand.Split();
        if (cmd is not null && cmd.Length >= 2)
        {
            return MakeFile(cmd[1], currentDir);
        }
        else
        {
            throw new CommandErrorException("Wrong touch command");
        }
    }
    
    public ConsoleState MakeFile(string delPath, DirectoryInfo currentDir)
    {

        string truePath = delPath[0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + delPath)
            : Path.GetFullPath(Path.Combine(currentDir.FullName, delPath));

        if(Directory.Exists(string.Join('\\', truePath.Split('\\')[0..^1])))
        {
            File.Create(truePath);
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
        return "touch <path> \n" +
               "Creating new file by given path";
    }
}