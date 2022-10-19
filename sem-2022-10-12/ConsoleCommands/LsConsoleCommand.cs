// Solution: hse - sem-2022-10-12 - LsConsoleCommand.cs
// Created at 2022-10-15 22:25
// Author: Самилык Анастасия
// Group: БПИ229

using System.Diagnostics;
using System.IO.Enumeration;
using System.Text.Json;

namespace sem_2022_10_12.ConsoleCommands;

public class Comparer : IComparer<(string, long)>
{
    public int Compare((string, long) x, (string, long) y)
    {
        return x.Item2.CompareTo(y.Item2);
    }
}
public class LsConsoleCommand : IConsoleCommand
{
    // TODO: Show files in current directory. Include file size and etc... Something like ls in bash with flags and stuff
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        var flagCommand = inpCommand.Split();

        if (flagCommand.Length == 1)
        {
            var filesInCurrentDir = GetFilesNamesFromCurrentDir(currentDir);
            var dirInCurrentDir = GetDirectoriesNamesFromCurrentDir(currentDir);
            var resultTable = GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, false, new List<string>());
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
        List<string> filesSize)
    {
        var resultTable="";
        var maxLengthFileName = Math.Max(filesInCurrentDir.MaxBy(x => x.Length).Length, 20)+1;
        var maxLengthDirName = Math.Max(dirInCurrentDir.MaxBy(x => x.Length).Length, 20)+1;
        var maxLengthFileIndex = Math.Max(filesInCurrentDir.Count.ToString().Length, 20)+1;
        var maxLengthDirIndex = Math.Max(dirInCurrentDir.Count.ToString().Length, 20)+1;
        var maxLengthFileSize = filesSize.Count != 0 ? Math.Max(filesSize.MaxBy(x => x.Length).Length, 20)+1: 0;
        if (index)
        {
            resultTable = new string('-', maxLengthDirIndex+maxLengthFileIndex+maxLengthDirName+maxLengthFileName+5)+"\n"
                          + $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirIndex-8))}DirIndex|"
                          + $"{string.Concat(Enumerable.Repeat(" ", maxLengthDirName-11))}Directories|"
                          + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileIndex-9))}FileIndex|"
                          + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-5))}Files|\n"
                          + new string('-', maxLengthDirIndex+maxLengthFileIndex+maxLengthDirName+maxLengthFileName+5) + "\n";
        }
        
        else if (filesSize.Count != 0)
        {
            resultTable = new string('-', maxLengthFileIndex + maxLengthFileName + maxLengthFileSize + 4) + "\n"
                + $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName - 11))}Directories|"
                + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-5))}Files|"
                + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileSize- 17))}FilesSize (bytes)|\n"
            + new string('-', maxLengthFileSize+maxLengthDirName+maxLengthFileName+4) + "\n";
        }
        
        else
        {
            resultTable = new string('-', maxLengthDirName+maxLengthFileName+3)+"\n"
                + $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName-11))}Directories|"
                + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-5))}Files|\n"
                + new string('-', maxLengthDirName+maxLengthFileName+3) + "\n";
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
                resultTable += $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirIndex - dirIndex.Length))}{dirIndex}|"
                               + $"{string.Concat(Enumerable.Repeat(" ", maxLengthDirName - dirName.Length))}{dirName}|"
                               + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileIndex - fileIndex.Length))}{fileIndex}|"
                               + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName - fileName.Length))}{fileName}|\n";

            }
            
            else if (filesSize.Count != 0)
            {
                resultTable += $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName - dirName.Length))}{dirName}|"
                    + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-fileName.Length))}{fileName}|"
                    + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileSize- fileSize.Length))}{fileSize}|\n";
            }
            
            else
            {
                resultTable += $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName-dirName.Length))}{dirName}|"
                              + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-fileName.Length))}{fileName}|\n";
            }
        }

        resultTable += new string('-', maxLengthFileName+maxLengthDirName + 
                                       (index ?  maxLengthDirIndex + maxLengthFileIndex+5 
                                           : filesSize.Count != 0 
                                               ? maxLengthFileSize+4:3));
        
        return resultTable;
    }

    public string GenerResultDataTable(
        ref List<string> filesInCurrentDir,
        ref List<string> dirInCurrentDir,
        ref List<string> filesCreationTime)
    {
        var maxLengthFileName = Math.Max(filesInCurrentDir.MaxBy(x => x.Length).Length, 20)+1;
        var maxLengthDirName = Math.Max(dirInCurrentDir.MaxBy(x => x.Length).Length, 20)+1;
        var maxLengthCreatTime = Math.Max(dirInCurrentDir.MaxBy(x => x.Length).Length, 30) + 1;
        
        var resultTable = new string('-', maxLengthDirName+maxLengthFileName+maxLengthCreatTime+4)+"\n"
            + $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName-11))}Directories|"
            + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName-5))}Files|"
            + $"{string.Concat(Enumerable.Repeat(" ", maxLengthCreatTime-20))}Files' creation time|\n"
            + new string('-', maxLengthDirName+maxLengthFileName+maxLengthCreatTime+4) + "\n";

        for (int i = 0; i < Math.Max(filesInCurrentDir.Count(), dirInCurrentDir.Count()); i++)
        {
            var fileName = i < filesInCurrentDir.Count ? filesInCurrentDir[i] : "";
            var dirName = i < dirInCurrentDir.Count ? dirInCurrentDir[i] : "";
            var fileDate = (fileName == "") ? "" : (filesCreationTime[i]);


            resultTable +=
                $"|{string.Concat(Enumerable.Repeat(" ", maxLengthDirName - dirName.Length))}{dirName}|"
                + $"{string.Concat(Enumerable.Repeat(" ", maxLengthFileName - fileName.Length))}{fileName}|" +
                $"{string.Concat(Enumerable.Repeat(" ", maxLengthCreatTime - fileDate.Length))}{fileDate}|\n";
        }

        resultTable += new string('-', maxLengthDirName + maxLengthFileName + maxLengthCreatTime + 4);
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
                return GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, true, new List<string>()).Trim();
            
            case "-s":
                var filesSize = currentDir.GetFiles().Select(x =>  x.Length.ToString()).ToList();
                return GenerResultTable(ref filesInCurrentDir, ref dirInCurrentDir, false, filesSize).Trim();
            case "-S":
                
                var filesNamesAndSizes = currentDir.GetFiles().Select(
                    x => (x.Name, x.Length)).ToList();
                filesNamesAndSizes.Sort(new Comparer());
                var sizeSortedFiles = new List<string>();
                var sizeSortedFilesSize = new List<string>();
                foreach (var x in filesNamesAndSizes)
                {
                    sizeSortedFiles.Add(x.Name);
                    sizeSortedFilesSize.Add(x.Length.ToString());
                }
                
                return GenerResultTable(ref sizeSortedFiles, ref dirInCurrentDir, false, sizeSortedFilesSize).Trim();
            case "-t":
                var filesData = currentDir.GetFiles().Select(x =>  x.CreationTime.ToString()).ToList();
                return GenerResultDataTable(ref filesInCurrentDir, ref dirInCurrentDir,ref filesData).Trim();
        }
        return "the flag does not exist";
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
