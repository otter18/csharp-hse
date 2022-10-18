// Solution: hse - sem-2022-10-12 - EchoConsoleCommand.cs
// Created at 2022-10-18 20:59
// Author: Черных Владимир
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class EchoConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        return new ConsoleState()
        {
            Result = inpCommand["echo ".Length..],
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        return "echo <text>\n" +
               "Just prints out input text\n" +
               "Use echo <text> > <file> to save output to file";
    }
}