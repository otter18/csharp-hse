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
        // split input command
        var inc = inpCommand.Split();
        if (inc.Length < 2)
        {
            return new ConsoleState() { Result = "Wrong arguments.\n", CurrentDir = currentDir };
        }
        else
        {
            if (inc[1] == "-r")
            {
                return inc.Length > 2 
                    ? DoRMr(inc, currentDir)
                    : new ConsoleState() { Result = "Wrong arguments.\n", CurrentDir = currentDir };
            }
            return DoRM(inc, currentDir);
        }
    }

    private ConsoleState DoRMr(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = new string[inc.Length-2];
        // expand "from_files" to full path & test exist
        for (int i=2, j=0; i < inc.Length; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[i]))
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (ifp[j] == ConsoleEngine.RootDir.FullName + Path.DirectorySeparatorChar || ifp[j] == ConsoleEngine.RootDir.FullName || ifp[j] == "/")
            {
                return new ConsoleState() { Result = $"Can not delete root directory: {inc[i]}\n", CurrentDir = currentDir };
            }
            if (Path.GetDirectoryName(ifp[j]) != ConsoleEngine.RootDir.FullName || !Directory.Exists(ifp[j]))
            {
                return new ConsoleState() { Result = $"Path not found: {inc[i]}\n", CurrentDir = currentDir };
            }
        }
        // remove files
        for (int j=0; j < ifp.Length; ++j)
        {
            // Console.WriteLine($">>> {ifp[j]}");
            try
            {
                Directory.Delete(ifp[j], true);
            }
            catch
            {
                // Console.WriteLine($"{ex.Message}");
                return new ConsoleState() { Result = $"File delete error: {inc[j+2]}\n", CurrentDir = currentDir };
            }
        }
        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }

    private ConsoleState DoRM(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = new string[inc.Length-1];
        // expand "from_files" to full path & test exist
        for (int i=1, j=0; i < inc.Length; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[i]))
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (Path.GetDirectoryName(ifp[j]) != ConsoleEngine.RootDir.FullName || !File.Exists(ifp[j]))
            {
                return new ConsoleState() { Result = $"File not found: {inc[i]}\n", CurrentDir = currentDir };
            }
        }
        // remove files
        for (int j=0; j < ifp.Length; ++j)
        {
            // Console.WriteLine($">>> {ifp[j]}");
            try
            {
                File.Delete(ifp[j]);
            }
            catch
            {
                return new ConsoleState() { Result = $"File delete error: {inc[j+1]}\n", CurrentDir = currentDir };
            }
        }
        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }

    public string GetHelpMessage()
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        return "rm file1 file2 ... fileN\n" +
               "cp -r path1 path2 ... pathN";
    }
}