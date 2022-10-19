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
        if (inc.Length < 2) throw new CommandErrorException("Arguments are required.");
        if (inc[1] == "-r")
        {
            if (inc.Length != 4) throw new CommandErrorException("Three arguments are required.");
            return DoCPr(inc, currentDir);
        }

        if (inc.Length < 3) throw new CommandErrorException("Two or more arguments are required.");
        return DoCP(inc, currentDir);
    }


    private void DoCPrAll(string ofp, DirectoryInfo idi, DirectoryInfo odi)
    {
        Directory.CreateDirectory(odi.FullName);

        // Copy each file into the new directory.
        foreach (var fi in idi.GetFiles())
        {
            // Console.WriteLine($"Copying {Path.Combine(odi.FullName, fi.Name)}");
            fi.CopyTo(Path.Combine(odi.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (var idiSubDir in idi.GetDirectories())
        {
            if (idiSubDir.FullName == ofp) continue;
            var odiSubDir = odi.CreateSubdirectory(idiSubDir.Name);
            DoCPrAll(ofp, idiSubDir, odiSubDir);
        }
    }


    private ConsoleState DoCPr(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = "";
        var ofp = "";
        // expand "to_files" to full path
        ofp = inc[3][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[3])
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[3]));
        // Console.WriteLine($">>> {ofp}");
        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}"))
        {
            throw new CommandErrorException($"Path is not found: {inc[3]}");
        }

        // expand "from_folder" to full path & test exist
        ifp = inc[2][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[2])
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[2]));
        if (!ifp.EndsWith(Path.DirectorySeparatorChar)) ifp += Path.DirectorySeparatorChar;
        // Console.WriteLine($">>> {ifp}");
        if (!ifp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !Directory.Exists(ifp))
        {
            throw new CommandErrorException($"Path is not found: {inc[2]}");
        }

        // do copy
        DirectoryInfo idi = new DirectoryInfo(ifp);
        DirectoryInfo odi = new DirectoryInfo(ofp);
        try
        {
            DoCPrAll(ofp, idi, odi);
        }
        catch
        {
            throw new CommandErrorException($"Unable to copy to the path {inc[3]}");
        }

        // return
        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }


    private ConsoleState DoCP(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        // test input arguments count
        return inc.Length == 3
            ? DoCP_ff(inc, currentDir)
            : DoCP_ffp(inc, currentDir);
    }


    private ConsoleState DoCP_ff(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        var ifp = "";
        var ofp = "";
        // expand "to_file" to full path & test for directory & test path exist
        ofp = inc[2][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[2])
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[2]));
        // Console.WriteLine($">>> {ofp}");
        if (Directory.Exists(ofp) || ofp.EndsWith(Path.DirectorySeparatorChar))
        {
            return DoCP_ffp(inc, currentDir);
        }

        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}"))
        {
            throw new CommandErrorException($"Path is not found: {inc[2]}");
        }

        // expand "from_file" to full path & test file exist
        ifp = inc[1][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[1])
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[1]));
        // Console.WriteLine($">>> {ifp}");
        if (!ifp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") || !File.Exists(ifp))
        {
            throw new CommandErrorException($"File is not found: {inc[1]}");
        }

        // do copy with overwrite
        try
        {
            // Console.WriteLine($">>> {Path.GetDirectoryName(ofp)}");
            Directory.CreateDirectory(Path.GetDirectoryName(ofp));
            File.Copy(ifp, ofp, true);
        }
        catch
        {
            throw new CommandErrorException($"Unable to copy to the file: {inc[2]}");
        }

        return new ConsoleState() { Result = "", CurrentDir = currentDir };
    }


    private ConsoleState DoCP_ffp(string[] inc, DirectoryInfo currentDir)
    {
        // Console.WriteLine($"!!! {new StackTrace().GetFrame(0).GetMethod().Name}");
        int i, j;
        var ifp = new string[inc.Length - 2];
        var ofp = "";
        // expand "from_files" to full path & test exist
        for (i = 1, j = 0; i < inc.Length - 1; ++i, ++j)
        {
            ifp[j] = inc[i][0] == Path.DirectorySeparatorChar
                ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[i])
                : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
            // Console.WriteLine($">>> {ifp[j]}");
            if (!ifp[j].StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}") ||
                !File.Exists(ifp[j]))
            {
                throw new CommandErrorException($"File is not found: {inc[i]}");
            }
        }

        // expand "to_folder" to full path & test exist
        ofp = inc[i][0] == Path.DirectorySeparatorChar
            ? Path.GetFullPath(ConsoleEngine.RootDir.FullName + inc[i])
            : Path.GetFullPath(Path.Combine(currentDir.FullName, inc[i]));
        // Console.WriteLine($">>> {ofp}");
        if (!ofp.StartsWith($"{ConsoleEngine.RootDir.FullName}{Path.DirectorySeparatorChar}"))
        {
            throw new CommandErrorException($"Path is not found: {inc[i]}");
        }

        try
        {
            if (!ofp.EndsWith(Path.DirectorySeparatorChar)) ofp += Path.DirectorySeparatorChar;
            Directory.CreateDirectory(Path.GetDirectoryName(ofp));
        }
        catch
        {
            throw new CommandErrorException($"Unable to create the directory: {inc[i]}");
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
                throw new CommandErrorException($"Unable to copy to the directory: {inc[i]}");
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