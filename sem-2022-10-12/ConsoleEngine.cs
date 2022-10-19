// Solution: hse - sem-2022-10-12 - ConsoleEngine.cs
// Created at 2022-10-15 20:01
// Author: Черных Владимир
// Group: БПИ229


namespace sem_2022_10_12;

using ConsoleCommands;

public static partial class ConsoleEngine
{
    private static bool _exit;

    private const string AsciiArt = "  ____                  _         ____         ___  \n | __ )    __ _   ___  | |__     |___ \\       / _ \\ \n |  _ \\   / _` | / __| | '_ \\      __) |     | | | |\n | |_) | | (_| | \\__ \\ | | | |    / __/   _  | |_| |\n |____/   \\__,_| |___/ |_| |_|   |_____| (_)  \\___/ \n                                                    ";

    public static DirectoryInfo RootDir { get; } = new("sandbox");
    private static DirectoryInfo _currentDir = new("sandbox");

    private static List<string> _history = new();

    private static Dictionary<string, IConsoleCommand> _commands = new()
    {
        { "cd", new CdConsoleCommand() },
        { "ls", new LsConsoleCommand() },
        { "cat", new CatConsoleCommand() },
        { "mkdir", new MkdirConsoleCommand() },
        { "touch", new TouchConsoleCommand() },
        { "cp", new CpConsoleCommand() },
        { "rm", new RmConsoleCommand() },
        { "tree", new TreeConsoleCommand() },
        { "foxsay", new FoxsayConsoleCommand() },
        { "echo", new EchoConsoleCommand() }
    };

    private static void Init()
    {
        if (!Directory.Exists(_currentDir.Name))
        {
            Directory.CreateDirectory(_currentDir.Name);
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(AsciiArt);
        Console.ResetColor();
    }

    private static void Loop()
    {
        DirectoryInfo? outFile = null;

        try
        {
            Write($"{FormatDirPath(_currentDir, RootDir.Parent!)} > ", ConsoleColor.Green);

            var lineHistory = new List<char>();
            var keyPressed = Console.ReadKey(true);
            ConsoleKeyInfo prevKeyPressed = new ConsoleKeyInfo();
            var historyInd = _history.Count;
            while (keyPressed.Key != ConsoleKey.Enter)
            {
                // reset historyInd if edited
                if (keyPressed.Key != ConsoleKey.UpArrow && keyPressed.Key != ConsoleKey.DownArrow)
                {
                    if (prevKeyPressed.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow)
                    {
                        _history.RemoveAt(_history.Count - 1);
                    }

                    historyInd = _history.Count;
                }

                // print actual symbols
                if (char.IsLetterOrDigit(keyPressed.KeyChar) ||
                    char.IsSymbol(keyPressed.KeyChar) ||
                    char.IsPunctuation(keyPressed.KeyChar))
                {
                    Console.Write(keyPressed.KeyChar);
                    lineHistory.Add(keyPressed.KeyChar);
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar &&
                         lineHistory.Count > 0 &&
                         lineHistory[^1] != keyPressed.KeyChar)
                {
                    Console.Write(keyPressed.KeyChar);
                    lineHistory.Add(keyPressed.KeyChar);
                }
                else
                {
                    // process pressed keys
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.Backspace when lineHistory.Count > 0:
                            Erase(1);
                            lineHistory.RemoveAt(lineHistory.Count - 1);
                            break;
                        case ConsoleKey.Tab:
                            Erase(lineHistory.Count);
                            var res = RenderAutoComplete(string.Join(null, lineHistory));
                            lineHistory.Clear();
                            foreach (var c in res)
                            {
                                Console.Write(c);
                                lineHistory.Add(c);
                            }

                            break;
                        case ConsoleKey.UpArrow when historyInd > 0:
                            if (historyInd == _history.Count)
                            {
                                _history.Add(string.Join(null, lineHistory));
                            }

                            Erase(lineHistory.Count);
                            historyInd--;
                            lineHistory.Clear();
                            foreach (var c in _history[historyInd])
                            {
                                Console.Write(c);
                                lineHistory.Add(c);
                            }

                            break;
                        case ConsoleKey.DownArrow when historyInd < _history.Count - 1:
                            Erase(lineHistory.Count);
                            historyInd++;
                            lineHistory.Clear();
                            foreach (var c in _history[historyInd])
                            {
                                Console.Write(c);
                                lineHistory.Add(c);
                            }

                            if (historyInd == _history.Count - 1)
                            {
                                _history.RemoveAt(_history.Count - 1);
                            }

                            break;
                    }
                }

                prevKeyPressed = keyPressed;
                keyPressed = Console.ReadKey(true);
            }

            Console.WriteLine();

            // return if input empty
            if (lineHistory.Count <= 0)
            {
                return;
            }

            // get string input
            var inpCommand = string.Join(null, lineHistory).Trim();
            _history.Add(inpCommand);

            var inpCommandName = inpCommand.Split(" ")[0];

            // check if result should be saved in file (command > file)
            if (inpCommand.Contains('>'))
            {
                var i = inpCommand.LastIndexOf('>');
                outFile = GetFullPath(inpCommand[(i + 1)..].Trim());
                inpCommand = inpCommand[..i].Trim();
            }

            // process generic commands
            if (_commands.ContainsKey(inpCommandName))
            {
                var (result, newCurrentDir) = _commands[inpCommandName].Process(inpCommand, _currentDir);
                _currentDir = newCurrentDir;
                OutputResult(result, outFile: outFile);

                return;
            }

            // process special commands
            switch (inpCommandName)
            {
                case "clear":
                    Console.Clear();
                    Init();
                    return;
                case "exit":
                    _exit = true;
                    return;
                case "help":
                    OutputResult(RenderHelpMessage(inpCommand), outFile: outFile);
                    return;
            }

            // error if command not found
            OutputResult(
                $"Command {inpCommandName} not found. Use help to list available commands",
                consoleColor: ConsoleColor.Red,
                outFile: outFile
            );
        }
        catch (CommandErrorException e)
        {
            OutputResult(
                e.Message,
                consoleColor: ConsoleColor.Red
            );
        }
        catch (Exception e)
        {
            OutputResult(
                e.ToString(),
                consoleColor: ConsoleColor.Red
            );
        }
    }


    public static void Run()
    {
        Init();
        while (!_exit)
        {
            try
            {
                Loop();
            }
            catch (Exception e)
            {
                WriteLine(e.ToString(), consoleColor: ConsoleColor.Red);
                _exit = true;
            }
        }
    }
}