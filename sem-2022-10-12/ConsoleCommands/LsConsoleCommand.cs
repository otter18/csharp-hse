// Solution: hse - sem-2022-10-12 - LsConsoleCommand.cs
// Created at 2022-10-15 22:25
// Author: 
// Group: БПИ229

using System.Diagnostics;
using System.IO.Enumeration;
using System.Text.Json;

namespace sem_2022_10_12.ConsoleCommands;

public class LsConsoleCommand : IConsoleCommand
{
    // TODO: Show files in current directory. Include file size and etc... Something like ls in bash with flags and stuff
    // BUG: Current implementation is for testing purposes
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var flagCommand = inpCommand.Split();

        if (flagCommand.Length == 1)
        {
            var filesInCurrentDir = GetFilesNamesFromCurrentDir(currentDir);
            var dirInCurrentDir = GetDirectoriesNamesFromCurrentDir(currentDir);
            var resultTable = GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, false);
            return new ConsoleState
            { 
                Result = resultTable.Trim(),
                CurrentDir = currentDir
            };
        }
        return new ConsoleState
        { 
            Result = GenerResultForFlags(flagCommand[1], currentDir).Trim(),
            CurrentDir = currentDir
        };
        


        return new ConsoleState();
    }

    public List<string> GetFilesNamesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetFiles().Select(x => x.Name).ToList();
    }
    
    public List<string> GetDirectoriesNamesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetDirectories().Select(x => x.Name).ToList();
    }

    public string GenerResultTable(ref List<string> filesInCurrentDir, ref List<string> dirInCurrentDir, bool index)
    {
        var resultTable="";
        if (index)
        {
            resultTable = new string('-', 86)
                              + "\n" + string.Format("|{0,20}|{1,20}|{2, 20}|{3, 20}|", "DirIndex", "Directories", "FileIndex", "Files")
                              + "\n" + new string('-', 86) + "\n";
        }
        else{
            resultTable = new string('-', 43)
                          + "\n" + string.Format("|{0,20}|{1,20}|", "Directories", "Files")
                          + "\n" + new string('-', 43) +"\n";
        }
        
        for (int i = 0; i < Math.Max(filesInCurrentDir.Count(), dirInCurrentDir.Count()); i++)
        {
            var fileName = i < filesInCurrentDir.Count ? filesInCurrentDir[i] : "";
            var dirName = i < dirInCurrentDir.Count ? dirInCurrentDir[i] : "";
            var fileIndex = fileName == "" ? "" : (i+1).ToString(); 
            var dirIndex = dirName == "" ? "" : (i+1).ToString();
            if (index)
            {
                resultTable += string.Format("|{0,20}|{1,20}|{2, 20}|{3, 20}|", 
                    dirIndex, dirName, fileIndex, fileName) + "\n";
            }
            else
            {
                resultTable += string.Format("|{0, 20}|{1,20}|", dirName, fileName) + "\n";
            }
        }

        resultTable += new string('-', index ? 86: 43);
        
        return resultTable;
    }

    public string GenerResultForFlags(string flagCommand, DirectoryInfo currentDir)
    {
        switch (flagCommand)
        {
            case "-I":
                var filesInCurrentDir = GetFilesNamesFromCurrentDir(currentDir);
                var dirInCurrentDir = GetDirectoriesNamesFromCurrentDir(currentDir);
                return GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, true).Trim();
     
        }

        return "";
    }

    public string GetHelpMessage()
    {
        var mess = "ls [--flags]\n"
                   + "Outputs a list of folders and files in currentDir\n\n"
                   + "-I list file's inode index number\n"
                   + "-s list file size\n"
                   + "-t sort by time & data\n"
                   + "-S sort by file size\n"
                   + "-d list directories - with '*/'";
            
        return mess;
    }
}