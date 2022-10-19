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

    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var flagCommand = inpCommand.Split();

        if (flagCommand.Length == 1)
        {
            var filesInCurrentDir = GetFilesNamesFromCurrentDir(currentDir);
            var dirInCurrentDir = GetDirectoriesNamesFromCurrentDir(currentDir);
            var resultTable = GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, false, new List<long>());
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
    
    /// <summary>
    /// to get files' names in current directory
    /// </summary>
    /// <param name="currentDir"> current directory </param>
    /// <returns> list of files' names in currentDir </returns>
    public List<string> GetFilesNamesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetFiles().Select(x => x.Name).ToList();
    }
    
    /// <summary>
    /// to get directories' names in current directory
    /// </summary>
    /// <param name="currentDir"> current directory </param>
    /// <returns> list of directories' names in currentDir </returns>
    public List<string> GetDirectoriesNamesFromCurrentDir(DirectoryInfo currentDir)
    {
        return currentDir.GetDirectories().Select(x => x.Name).ToList();
    }
    
    /// <summary>
    ///  to get a result string for Console State
    /// </summary>
    /// <param name="filesInCurrentDir"> list of files' names in current directory </param>
    /// <param name="dirInCurrentDir"> list of files' names in current directory </param>
    /// <param name="index"> if we need to output files and directories with indexes: true,  else: false</param>
    /// <param name="filesSize"> list of files' sizes (if we need to output files' sizes), else: empty list</param>
    /// <returns> result string for ConsoleState.Result </returns>
    public string GenerResultTable(
        ref List<string> filesInCurrentDir, 
        ref List<string> dirInCurrentDir, 
        bool index, 
        List<long> filesSize)
    {
        var resultTable="";
        
        if (index)
        {
            resultTable = new string('-', 86)
                              + "\n" + string.Format("|{0,20}|{1,20}|{2, 20}|{3, 20}|", "DirIndex", "Directories", "FileIndex", "Files")
                              + "\n" + new string('-', 86) + "\n";
        }
        
        else if (filesSize.Count != 0)
        {
            resultTable = new string('-', 74)
                          + "\n" + string.Format("|{0,20}|{1,20}|{2, 30}|", "Directories", "Files", "FilesSize (bytes)")
                          + "\n" + new string('-', 74) + "\n"; 
        }
        
        else
        {
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
            var fileSize = (fileName == "" || filesSize.Count == 0) ? "" : (filesSize[i]).ToString();
            
            if (index)
            {
                resultTable += string.Format("|{0,20}|{1,20}|{2, 20}|{3, 20}|", 
                    dirIndex, dirName, fileIndex, fileName) + "\n";
            }
            
            else if (filesSize.Count != 0)
            {
                resultTable += fileSize == "" ? 
                    string.Format("|{0,20}|{1,20}|  {2, 28}|", dirName,  fileName, fileSize) + "\n" :
                    string.Format("|{0,20}|{1,20} =>{2, 28}|", dirName,  fileName, fileSize) + "\n";
            }
            
            else
            {
                resultTable += string.Format("|{0, 20}|{1,20}|", dirName, fileName) + "\n";
            }
        }

        resultTable += new string('-', index ? 86: filesSize.Count != 0 ? 74 :43);
        
        return resultTable;
    }
    
    /// <summary>
    /// to generate result strings for inpCommand with flags
    /// </summary>
    /// <param name="flagCommand"> flag from inpCommand </param>
    /// <param name="currentDir"> current directory </param>
    /// <returns> result string for ConsoleState.Result </returns>
    public string GenerResultForFlags(string flagCommand, DirectoryInfo currentDir)
    {
        var filesInCurrentDir = GetFilesNamesFromCurrentDir(currentDir);
        var dirInCurrentDir = GetDirectoriesNamesFromCurrentDir(currentDir);
        
        switch (flagCommand)
        {
            case "-I":
                return GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, true, new List<long>()).Trim();
            
            case "-s":
                var filesSize = currentDir.GetFiles().Select(x =>  x.Length).ToList();
                return GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, false, filesSize).Trim();
        }
        return "";
    }

    public string GetHelpMessage()
    {
        var mess = "\nls [--flags]\n"
                   + "Outputs a list of folders and files in currentDir\n\n"
                   + "Flags:\n"
                   + "-I list file's inode index number\n"
                   + "-s list file size\n"
                   + "-t sort by time & data\n"
                   + "-S sort by file size\n"
                   + "-d list directories - with '*/'\n";
            
        return mess;
    }
}