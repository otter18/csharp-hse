// Solution: hse - sem-2022-10-12 - RmConsoleCommand.cs
// Created at 2022-10-16 00:11
// Author: Aleksa Khruleva
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class RmConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        // Console.WriteLine($"ConsoleEngine.RootDir.FullName:\n{ConsoleEngine.RootDir.FullName}");
        // split input command
        var inc = inpCommand.Split();
        if (inc.Length < 2) throw new CommandErrorException("Arguments required.");
        if (inc[1] == "-r")
        {
            if (inc.Length == 2) throw new CommandErrorException("Arguments required.");
            return DoRMr(inc, currentDir);
        }

        return DoRM(inc, currentDir);
    }


    private ConsoleState DoRMr(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = new string[inc.Length - 2];
        // expand "from_files" to full path & test exist
        for (int i = 2, j = 0; i < inc.Length; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[i])
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (ifp[j] == ConsoleEngine.RootDir.FullName + Path.DirectorySeparatorChar ||
                ifp[j] == ConsoleEngine.RootDir.FullName || ifp[j] == Path.DirectorySeparatorChar.ToString())
            {
                throw new CommandErrorException($"Unable to delete the root directory: {inc[i]}");
            }

            if (!ifp[j].StartsWith(ConsoleEngine.RootDir.FullName + Path.DirectorySeparatorChar) ||
                !Directory.Exists(ifp[j]))
            {
                throw new CommandErrorException($"Path is not found: {inc[i]}");
            }
        }

        // remove files
        for (int j = 0; j < ifp.Length; ++j)
        {
            // Console.WriteLine($">>> {ifp[j]}");
            try
            {
                Directory.Delete(ifp[j], true);
            }
            catch
            {
                throw new CommandErrorException($"Unable to delete the file: {inc[j + 2]}");
            }
        }

        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }


    private ConsoleState DoRM(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = new string[inc.Length - 1];
        // expand "from_files" to full path & test exist
        for (int i = 1, j = 0; i < inc.Length; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[i])
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (!ifp[j].StartsWith(ConsoleEngine.RootDir.FullName + Path.DirectorySeparatorChar) ||
                !File.Exists(ifp[j]))
            {
                throw new CommandErrorException($"File is not found: {inc[i]}");
            }
        }

        // remove files
        for (int j = 0; j < ifp.Length; ++j)
        {
            // Console.WriteLine($">>> {ifp[j]}");
            try
            {
                File.Delete(ifp[j]);
            }
            catch
            {
                throw new CommandErrorException($"Unable to delete the file: {inc[j + 1]}");
            }
        }

        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }


    public string GetHelpMessage()
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        return "rm file1 file2 ... fileN\n" +
               "rm -r path1 path2 ... pathN";
    }
}