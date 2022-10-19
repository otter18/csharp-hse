// Solution: hse - sem-2022-10-12 - CatConsoleCommand.cs
// Created at 2022-10-15 23:26
// Author: Виктор Филимонов
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class CatConsoleCommand : IConsoleCommand
{
    // Static thing to remember the previous position
    // to move between pages using arrows.
    private bool _numerateLines = false; // initialized for clarity
    private bool _skipLines = false;
    private bool _printPage = false;
    private int _pageNumber = 1;
    private int _pageSize = 20;

    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var lines = new List<string>();
        if (inpCommand.Length < 4)
        {
            throw new CommandErrorException("Invalid syntax");
        }
        var flagsAndFiles = inpCommand[4..];
        var outMessage = "";

        foreach (var element in flagsAndFiles.Split())
        {
            if (element[0] == '-')
            {
                // got a flag
                var flag = element[..2];
                var value = 0;
                var parameterSet = false;

                // get flag parameter value
                if (element.Length > 2 && element[2] == '=')
                {
                    try
                    {
                        value = int.Parse(element[3..]);
                    }
                    catch (Exception e)
                    {
                        throw new CommandErrorException($"Wrong value of integer parameter", e);
                    }

                    parameterSet = true;
                }


                switch (flag)
                {
                    case "-n":
                        _numerateLines = true;
                        break;
                    case "-s":
                        _skipLines = true;
                        break;
                    case "-p":
                        if (!parameterSet)
                        {
                            value = 1;
                        }

                        if (value < 1)
                        {
                            throw new CommandErrorException("Inappropriate page number parameter");
                        }

                        _pageNumber = value;
                        _printPage = true;
                        break;
                    case "-P":
                        if (value < 1)
                        {
                            throw new CommandErrorException("Inappropriate value of page size parameter");
                        }

                        if (!parameterSet)
                        {
                            throw new CommandErrorException("Page size parameter must be set");
                        }

                        _pageSize = value;
                        break;
                    default:
                        throw new CommandErrorException($"Unknown cat flag: {element}");
                }
            }
            else
            {
                // got a file

                // get file full path
                var fileFullPath = element;
                // if the file doesn't start with 'sandbox', assume that
                // it's path is relative
                if (!fileFullPath.StartsWith("sandbox"))
                {
                    fileFullPath = currentDir.FullName
                                   + Path.DirectorySeparatorChar
                                   + element;
                }
                else
                {
                    fileFullPath = ConsoleEngine.RootDir.FullName
                                   + Path.DirectorySeparatorChar
                                   + element[7..];
                }

                // try to read from the file
                try
                {
                    lines.AddRange(File.ReadLines(fileFullPath));
                }
                catch (FileNotFoundException)
                {
                    throw new CommandErrorException("File doesn't exist");
                }
                catch (Exception e)
                {
                    throw new CommandErrorException("Unexpected exception occured while reading from file", e);
                }
            }
        }

        // return the result if not empty
        if (outMessage != "")
        {
            return new ConsoleState { Result = outMessage, CurrentDir = currentDir };
        }

        // counts the number of skipped during -s lines to keep the numeration right
        var skippedLines = 0;

        // the start and the end of the loop
        var low = 0;
        var high = lines.Count;

        // if -p used, iterate only through required lines
        if (_printPage)
        {
            low = (_pageNumber - 1) * _pageSize;
            high = Math.Min(_pageNumber * _pageSize, lines.Count);
        }

        for (var i = low; i < high; ++i)
        {
            var line = lines[i];

            // implements -s flag
            if (_skipLines && i > 0 && lines[i - 1] == "" && line == "")
            {
                ++skippedLines;
                continue;
            }

            // implements -s flag, considering -n flag and the beginning of the loop
            if (_numerateLines)
            {
                line = $"{i - skippedLines + 1 - low}.\t {line}";
            }


            outMessage = outMessage + line + "\n";
        }

        return new ConsoleState { Result = outMessage, CurrentDir = currentDir };
    }

    public string GetHelpMessage()
    {
        const string message =
            "returns the result of concatenating of one or more files\n"
            + "-n  numerates all lines of result \n"
            + "-s  delete duplicating empty lines\n"
            + "-P=x  set page size equal to x"
            + "-p=x  print page number x\n"
            + "the number of output lines can be less than x when using -s\n"
            + "because cat first chooses the required number of lines\n"
            + "and only after that deletes unneeded lines";
        return message;
    }
}