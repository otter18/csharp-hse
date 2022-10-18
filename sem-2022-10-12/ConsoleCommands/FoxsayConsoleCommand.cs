// Solution: hse - sem-2022-10-12 - FoxsayConsoleCommand.cs
// Created at 2022-10-17 20:33
// Author: Черных Владимир
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class FoxsayConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        return new ConsoleState()
        {
            Result = "   /|_/|\n" +
                     $"  / ^ ^(_o  <  {inpCommand[("foxsay".Length + 1)..]}\n" +
                     " /    __.'\n" +
                     " /     \\\n" +
                     "(_) (_) '._\n" +
                     "  '.__     '. .-''-'.\n" +
                     "     ( '.   ('.____.''\n" +
                     "     _) )'_, )\n" +
                     "    (__/ (__/\n",
            CurrentDir = currentDir
        };
    }

    public string GetHelpMessage()
    {
        return "What does the fox say?... 🎵🎵🎵\nfoxsay <message>";
    }
}