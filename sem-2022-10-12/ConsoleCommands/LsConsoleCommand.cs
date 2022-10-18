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
            var filesInCurrentDir = GetFilesFromCurrentDir(currentDir);
            var dirInCurrentDir = GetDirectoriesFromCurrentDir(currentDir);
            var resultTable = GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir);
            return new ConsoleState
            { 
                Result = resultTable.Trim(),
                CurrentDir = currentDir
            };
        }
        /*switch (flag[1])
        {
            case ;
        }
        */
        return new ConsoleState();
    }

    public List<string> GetFilesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetFiles().Select(x => x.Name).ToList();
    }
    
    public List<string> GetDirectoriesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetDirectories().Select(x => x.Name).ToList();
    }

    public string GenerResultTable(ref List<string> filesInCurrentDir, ref List<string> dirInCurrentDir)
    {
        var resultTable = new string('-', 45)
                          + "\n" + string.Format("|{0,20}|{1,20}|", "Directories", "Files")
                          + "\n" + new string('-', 45) +"\n";
        
        for (int i = 0; i < Math.Max(filesInCurrentDir.Count(), dirInCurrentDir.Count()); i++)
        {
            var fileName = i < filesInCurrentDir.Count ? filesInCurrentDir[i] : "";
            var dirName = i < dirInCurrentDir.Count ? dirInCurrentDir[i] : "";
            resultTable += string.Format("|{0, 20}|{1,20}|", dirName, fileName) + "\n";
        }

        resultTable += new string('-', 45);
        return resultTable;
    }

    public string GetHelpMessage()
    {
        var mess = "I'll write this part tomorrow \n and now I want to present you a rabbit \n" +
                    "(\\__/)\n"+
                    "(='.'=)\n"+
                    "(\")_(\")";
           
        return mess;
    }
}