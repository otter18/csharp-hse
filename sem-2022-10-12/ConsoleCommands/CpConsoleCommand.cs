// Solution: hse - sem-2022-10-12 - CpConsoleCommand.cs
// Created at 2022-10-16 00:10
// Author: Aleksa Khruleva
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

public class CpConsoleCommand : IConsoleCommand
{
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        // split input command
        var inc = inpCommand.Split();
        if (inc.Length < 3)
        {
            return new ConsoleState() { Result = "Wrong arguments.\n", CurrentDir = currentDir };
        }
        else
        {
            if (inc[1] == "-r")
            {
                return inc.Length == 4 
                    ? DoCPr(inc, currentDir)
                    : new ConsoleState() { Result = "Wrong arguments.\n", CurrentDir = currentDir };
            }
            return inc.Length > 2
                ? DoCP(inc, currentDir)
                : new ConsoleState() { Result = "Wrong arguments.\n", CurrentDir = currentDir };
        }
    }

    private void DoCPrAll(string ofp, DirectoryInfo dis, DirectoryInfo dit)
    {
        Directory.CreateDirectory(dit.FullName);

        // Copy each file into the new directory.
        foreach (var fi in dis.GetFiles())
        {
            // Console.WriteLine($"Copying {Path.Combine(dit.FullName, fi.Name)}");
            fi.CopyTo(Path.Combine(dit.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (var disSubDir in dis.GetDirectories())
        {
            if (disSubDir.FullName == ofp) continue;
            var dirSubDir = dit.CreateSubdirectory(disSubDir.Name);
            DoCPrAll(ofp, disSubDir, dirSubDir);
        }
    }

    private ConsoleState DoCPr(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = "";
        var ofp = "";
        // test input arguments count
        if (inc.Length != 4)
        {
            return new ConsoleState() { Result = $"Wrong arguments.\n", CurrentDir = currentDir };
        }
        // expand "from_files" to full path & test exist
        ofp = inc[3][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[3]))
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[3]));
        // Console.WriteLine($">>> {ofp}");
        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !Directory.Exists(ofp))
        {
            return new ConsoleState() { Result = $"Path not found: {inc[3]}\n", CurrentDir = currentDir };
        }
        // expand "to_folder" to full path & test exist
        ifp = inc[2][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[2]))
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[2]));
        if (!ifp.EndsWith("/")) ifp += Path.DirectorySeparatorChar;
        // Console.WriteLine($">>> {ifp}");
        if (!ifp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !Directory.Exists(ifp))
        {
            return new ConsoleState() { Result = $"Path not found: {inc[2]}\n", CurrentDir = currentDir };
        }
        // do copy
        DirectoryInfo dis = new DirectoryInfo(ifp);
        DirectoryInfo dit = new DirectoryInfo(ofp);
        try
        {
            DoCPrAll(ofp, dis, dit);
        }
        catch
        {
            return new ConsoleState() { Result = $"Error copy to path {inc[3]}\n", CurrentDir = currentDir };
        }
        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }

    private ConsoleState DoCP(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        // test input arguments count
        if (inc.Length == 3)
        {
            return DoCP_ff(inc, currentDir);
        }
        return inc.Length > 3
            ? DoCP_ffp(inc, currentDir)
            :  new ConsoleState() { Result = $"Wrong arguments.\n", CurrentDir = currentDir };
    }

    private ConsoleState DoCP_ff(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = "";
        var ofp = "";
        // expand "to_file" to full path & test for directory & test path exist
        ofp = inc[2][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[2]))
            : Path.GetFullPath(Path.Combine(currentDir.FullName,inc[2]));
        // Console.WriteLine($">>> {ofp}");
        if (Directory.Exists(ofp) || ofp.EndsWith("/"))
        {
            return DoCP_ffp(inc, currentDir);
        }
        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}"))
        {
            return new ConsoleState() { Result = $"Path not found: {inc[2]}\n", CurrentDir = currentDir };
        }
        // expand "from_file" to full path & test file exist
        ifp = inc[1][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[1]))
            : Path.GetFullPath(Path.Combine(currentDir.FullName,inc[1]));
        // Console.WriteLine($">>> {ifp}");
        if (!ifp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !File.Exists(ifp))
        {
            return new ConsoleState() { Result = $"File not found: {inc[1]}\n", CurrentDir = currentDir };
        }
        // do copy with overwrite
        try
        {
            File.Copy(ifp, ofp, true);
        }
        catch
        {
            return new ConsoleState() { Result = $"Error copy to file {inc[2]}\n", CurrentDir = currentDir };
        }
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }


    private ConsoleState DoCP_ffp(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        int i, j;
        var ifp = new string[inc.Length-2];
        var ofp = "";
        // expand "from_files" to full path & test exist
        for (i=1, j=0; i < inc.Length - 1; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[i]))
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (!ifp[j].StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !File.Exists(ifp[j]))
            {
                return new ConsoleState() { Result = $"File not found: {inc[i]}\n", CurrentDir = currentDir };
            }
        }
        // expand "to_folder" to full path & test exist
        ofp = inc[i][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(Path.Combine(ConsoleEngine.RootDir.FullName, inc[i]))
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
        // Console.WriteLine($">>> {ofp}");
        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !Directory.Exists(ofp))
        {
            return new ConsoleState() { Result = $"Path not found: {inc[i]}\n", CurrentDir = currentDir };
        }
        // do copy with overwrite
        foreach (var fp in ifp)
        {
            try
            {
                // Console.WriteLine($"Copy to: {Path.Combine(ofp, Path.GetFileName(fp))}");
                File.Copy(fp, Path.Combine(ofp, Path.GetFileName(fp)), true);
            }
            catch
            {
                return new ConsoleState() { Result = $"Error copy to directory {inc[i]}\n", CurrentDir = currentDir };
            }
        }
        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }

    public string GetHelpMessage()
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        return "cp from_file1 to_file2\n" +
               "cp from_file1 from_file2 ... from_fileN to_directory\n" +
               "cp -r from_path1 to_path2";
    }
}