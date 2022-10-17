// Solution: hse - sem-2022-10-12 - CatConsoleCommand.cs
// Created at 2022-10-15 23:26
// Author: 
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class CatConsoleCommand: IConsoleCommand
{
    // TODO: Viewer for text files
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        throw new NotImplementedException();
    }

    public string GetHelpMessage()
    {
        return "botva";
    }
}