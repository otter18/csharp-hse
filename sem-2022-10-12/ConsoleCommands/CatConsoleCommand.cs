// Solution: hse - sem-2022-10-12 - CatConsoleCommand.cs
// Created at 2022-10-15 23:26
// Author: Виктор Филимонов
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class CatConsoleCommand : IConsoleCommand
{
    // TODO: Static thing to remember the previous position
    // TODO: to move between pages using arrows.

    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var numerateLines = false;
        var skipLines = false;
        var printPage = false;
        var pageNumber = 1;
        var pageSize = 20;

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
                        numerateLines = true;
                        break;
                    case "-s":
                        skipLines = true;
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

                        pageNumber = value;
                        printPage = true;
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

                        pageSize = value;
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

        // counts the number of skipped during -s lines to keep the numeration right
        var skippedLines = 0;

        // the start and the end of the loop
        var low = 0;
        var high = lines.Count;

        // if -p used, iterate only through required lines
        if (printPage)
        {
            low = (pageNumber - 1) * pageSize;
            high = Math.Min(pageNumber * pageSize, lines.Count);
        }

        for (var i = low; i < high; ++i)
        {
            var line = lines[i];

            // implements -s flag
            if (skipLines && i > 0 && lines[i - 1] == "" && line == "")
            {
                ++skippedLines;
                continue;
            }

            // implements -s flag, considering -n flag and the beginning of the loop
            if (numerateLines)
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
            "Returns the result of concatenating of one or more files\n"
            + "-n  numerates all lines of result \n"
            + "-s  delete duplicating empty lines\n"
            + "-P=x  set page size equal to x\n"
            + "-p=x  print page number x\n"
            + "Note: the number of output lines can be less than x when using -s\n"
            + "because cat first chooses the required number of lines\n"
            + "and only after that deletes unneeded lines";
        return message;
    }
}