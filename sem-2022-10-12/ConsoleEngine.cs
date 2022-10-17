// Solution: hse - sem-2022-10-12 - ConsoleEngine.cs
// Created at 2022-10-15 20:01
// Author: Черных Владимир Артемович
// Group: БПИ229


namespace sem_2022_10_12;

using ConsoleCommands;

public static class ConsoleEngine
{
    private static bool _exit;

    private const string AsciiArt = "  ____                  _         ____         ___  \n | __ )    __ _   ___  | |__     |___ \\       / _ \\ \n |  _ \\   / _` | / __| | '_ \\      __) |     | | | |\n | |_) | | (_| | \\__ \\ | | | |    / __/   _  | |_| |\n |____/   \\__,_| |___/ |_| |_|   |_____| (_)  \\___/ \n                                                    ";

    public static DirectoryInfo RootDir { get; } = new DirectoryInfo("sandbox");
    private static DirectoryInfo _currentDir = new DirectoryInfo("sandbox");

    private static List<string> _history = new List<string>();

    private static Dictionary<string, IConsoleCommand> _commands = new()
    {
        { "cd", new CdConsoleCommand() },
        { "ls", new LsConsoleCommand() },
        { "cat", new CatConsoleCommand() },
        { "mkdir", new MkdirConsoleCommand() },
        { "touch", new TouchConsoleCommand() },
        { "cp", new CpConsoleCommand() },
        { "rm", new RmConsoleCommand() }
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

    private static string AutoCompleteCommand(string inp)
    {
        // TODO: autocomplete
        throw new NotImplementedException();
    }

    private static void Erase(int cnt)
    {
        for (var i = 0; i < cnt; i++)
        {
            Console.Write("\b \b");
        }
    }

    private static string FormatDirPath(DirectoryInfo directoryInfo)
    {
        return directoryInfo.FullName.Replace((RootDir.Parent?.FullName ?? "") + "/", "");
    }

    private static void Loop()
    {
        Console.Write($"{FormatDirPath(_currentDir)} > ");

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
            else if (char.IsWhiteSpace(keyPressed.KeyChar) &&
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
                        // TODO: autocomplete
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

        if (lineHistory.Count <= 0)
        {
            return;
        }

        var inpCommand = string.Join(null, lineHistory).Trim();
        _history.Add(inpCommand);

        var inpCommandName = inpCommand.Split(" ")[0];


        // process generic commands
        foreach (var command in _commands.Keys.Where(command => inpCommandName == command))
        {
            try
            {
                var (result, newCurrentDir) = _commands[command].Process(inpCommand, _currentDir);
                _currentDir = newCurrentDir;
                if (result.Length != 0)
                {
                    Console.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }

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
            default:
                break;
        }

        // error if command not found
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Command {inpCommandName} not found. Use help to list available commands");
        Console.ResetColor();
    }


    public static void Run()
    {
        Init();
        while (!_exit)
        {
            Loop();
        }
    }
}