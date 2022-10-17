// Solution: hse - sem-2022-10-12 - CdConsoleCommand.cs
// Created at 2022-10-15 22:23
// Author: Тот Андраш
// Group: БПИ229

namespace sem_2022_10_12.ConsoleCommands;

using System.IO;

/// <summary>
/// Represents Linux-alike cd command of console.
/// </summary>
public class CdConsoleCommand : IConsoleCommand
{
    /// <summary>
    /// Adds the directory separator at the beginning of the given string
    /// </summary>
    private string AddSeparatorToBeginning(string path)
    {
        return Path.DirectorySeparatorChar + path;
    }
    
    /// <summary>
    /// Reorganizing the path so it is indifferent to multiple operating systems.
    /// </summary>
    /// <returns>Indifferent to OS path.</returns>
    private string MakePathIndifferent(string path)
    {
        return AddSeparatorToBeginning(Path.Combine(path.Split('/', '\\')));
    }

    /// <summary>
    /// Gets the path the user requests in a ready to use form.
    /// </summary>
    private string GetRequestedPath(string path)
    {
        var pathSplitted = path.Split(" ",
            StringSplitOptions.RemoveEmptyEntries);

        if (pathSplitted.Length < 2)
        {
            throw new CdCommnandEmptyPathException();
        }
        
        return MakePathIndifferent(pathSplitted[1]);
    }

    /// <summary>
    /// Returns a directory supposed to be requested by the user.
    /// </summary>
    private DirectoryInfo ManagePath(string requestedPath, string currentPath)
    {
        // If the command was 'cd .' than return the current directory.
        if (requestedPath == AddSeparatorToBeginning("."))
        {
            return new DirectoryInfo(currentPath);
        }

        // If the command was 'cd /' than return to the root directory
        if (requestedPath == AddSeparatorToBeginning(""))
        {
            return ConsoleEngine.RootDir;
        }
        
        DirectoryInfo currentDirectory = new DirectoryInfo(currentPath);
        ManagePathDots(ref requestedPath, ref currentDirectory);

        var pathAfterDots = currentDirectory.FullName;
        if (requestedPath != string.Empty)
        {
            if (requestedPath.StartsWith(currentPath))
            {
                requestedPath = requestedPath[currentPath.Length..];
            }
            pathAfterDots += requestedPath;
        }

        return new DirectoryInfo(pathAfterDots);
    }

    /// <summary>
    /// Changes the given directory if there are .. commands in path.
    /// Removes managed .. command from the path.
    /// </summary>
    private void ManagePathDots(ref string path, ref DirectoryInfo currentDirectory)
    {
        var command = AddSeparatorToBeginning("..");
        while (path.StartsWith(command))
        {
            currentDirectory = currentDirectory.Parent ?? currentDirectory;
            // Remove the managed command from the path.
            path = path[command.Length..];
        }
    }

    public string GetHelpMessage() => "cd {path}" + 
                                      "\n The path can be absolute or relative.";
    
    public ConsoleState Process(string inpCommand, DirectoryInfo currentDir)
    {
        // Getting the path the user has given.
        string requestedPath;
        try
        {
            requestedPath = GetRequestedPath(inpCommand);
        }
        catch (CdCommnandEmptyPathException e)
        {
            throw new CommandErrorException("No path was given.", e);
        }
        
        // The directory to work with while managing user's commands.
        DirectoryInfo directoryUnderChange = ManagePath(requestedPath, currentDir.FullName);
        
        // Checking if the result directory is still in the root directory.
        if (ConsoleEngine.IsInRoot(directoryUnderChange) is false)
        {
            throw new CommandErrorException(
                $"Cannot leave the root directory: {ConsoleEngine.RootDir.FullName}");
        }
        
        // Checking if the result directory exists.
        if (Directory.Exists(directoryUnderChange.FullName) is false)
        {
            throw new CommandErrorException("The given directory does not exist");
        }

        // If everything is OK, return new DirectoryInfo.
        return new ConsoleState
        {
            CurrentDir = directoryUnderChange,
            Result = ""
        };
    }
}

/// <summary>
/// Occurs when user types cd command without giving any path.
/// </summary>
public class CdCommnandEmptyPathException : ApplicationException
{
    public CdCommnandEmptyPathException() : base("No path was given."){}
    public CdCommnandEmptyPathException(string message) : base(message) {}
    public CdCommnandEmptyPathException(string message, Exception inner) : base(message, inner){}
}