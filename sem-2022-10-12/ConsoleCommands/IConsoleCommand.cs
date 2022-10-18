// Solution: hse - sem-2022-10-12 - IConsoleCommand.cs
// Created at 2022-10-15 22:17
// Author: Черных Владимир
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public interface IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir);
    public string GetHelpMessage();
}