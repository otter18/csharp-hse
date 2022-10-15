// Solution: hse - sem-2022-10-12 - CdConsoleCommand.cs
// Created at 2022-10-15 22:23
// Author: $YOUR NAME$
// Group: БПИ229

namespace sem_2022_10_12;

public class CdConsoleCommand : IConsoleCommand
{
    // BUG: Current implementation is for testing. Improve it!
    // TODO: Changes currentDir accordingly to passed params. Should support ., / and other special paths. Can't go outside sandbox/
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var newCurrentDir = new DirectoryInfo(currentDir + "/" + inpCommand.Split(" ")[1]);
        return new ConsoleState
        {
            CurrentDir = newCurrentDir,
            Result = ""
        };
    }

    public string GetHelpMessage()
    {
        return "cd {path}";
    }
}