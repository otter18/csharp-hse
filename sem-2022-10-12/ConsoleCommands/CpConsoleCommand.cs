// Solution: hse - sem-2022-10-12 - CpConsoleCommand.cs
// Created at 2022-10-16 00:10
// Author: 
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class CpConsoleCommand:IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        throw new NotImplementedException();
    }

    public string GetHelpMessage()
    {
        return "botva";
    }
}