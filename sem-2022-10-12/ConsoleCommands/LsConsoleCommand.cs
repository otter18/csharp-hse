// Solution: hse - sem-2022-10-12 - LsConsoleCommand.cs
// Created at 2022-10-15 22:25
// Author: 
// Group: БПИ229

using System.Text.Json;

namespace sem_2022_10_12.ConsoleCommands;

public class LsConsoleCommand : IConsoleCommand
{
    // TODO: Show files in current directory. Include file size and etc... Something like ls in bash with flags and stuff
    // BUG: Current implementation is for testing purposes
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        return new ConsoleState
        {
            Result = (string.Join('\n', currentDir.GetDirectories().Select(x => x.Name)) + '\n' +
                     string.Join('\n', currentDir.GetFiles().Select(x => x.Name))).Trim(),
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        throw new NotImplementedException();
    }
}