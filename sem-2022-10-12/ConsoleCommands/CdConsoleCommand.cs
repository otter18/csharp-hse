// Solution: hse - sem-2022-10-12 - CdConsoleCommand.cs
// Created at 2022-10-15 22:23
// Author: Тот Андраш
// Group: БПИ229

namespace sem_2022_10_12;

using System.IO;

/// <summary>
/// Represents Linux-alike cd command of console.
/// </summary>
public class CdConsoleCommand : IConsoleCommand
{
    /// <summary>
    /// Reorganizing the path so it is indifferent to different operating systems.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Indifferent to OS path.</returns>
    private string MakePathIndifferent(string path)
    {
        // Path.DirectorySeparatorChar is vital! DO NOT REMOVE! 
        return Path.DirectorySeparatorChar +
               Path.Combine(path.Split('/', '\\'));
    }

    /// <summary>
    /// Checks if the root directory of the ConsoleEngine is parent
    /// of the given directory. 
    /// </summary>
    private bool IsInRoot(DirectoryInfo directoryToCheck)
    {
        return directoryToCheck.FullName.StartsWith(ConsoleEngine.RootDir.FullName);
    }

    /// <summary>
    /// Returns ConsoleState object with the same directory as it was
    /// before calling cd command with the description of the reason why
    /// the directory hasn't changed.
    /// </summary>
    private ConsoleState NoMoveDueError(
        string errorDescription,
        DirectoryInfo currentDirectory)
    {
        return new ConsoleState()
        {
            CurrentDir = currentDirectory,
            Result = errorDescription
        };
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
    /// Returns a directory is supposed to be requested by the user.
    /// </summary>
    private DirectoryInfo ManagePath(string requestedPath, string currentPath)
    {
        // If the command was cd . than return the current directory.
        if (requestedPath == Path.DirectorySeparatorChar + ".")
        {
            return new DirectoryInfo(currentPath);
        }
        
        DirectoryInfo currentDirectory = new DirectoryInfo(currentPath);
        ManagePathDots(ref requestedPath, ref currentDirectory);

        string pathAfterDots = currentDirectory.FullName;
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
        var command = Path.DirectorySeparatorChar + "..";
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
        catch (CdCommnandEmptyPathException)
        {
            return NoMoveDueError("No path was given.", currentDir);
        }
        
        
        // The directory to work with while managing user's commands.
        DirectoryInfo directoryUnderChange = ManagePath(requestedPath, currentDir.FullName);
        
        
        
        // Checking if the result directory is still in the root directory.
        if (IsInRoot(directoryUnderChange) is false)
        {
            return NoMoveDueError(
                $"Cannot leave the root directory: {ConsoleEngine.RootDir.FullName}",
                currentDir);
        }
        
        // Checking if the result directory exists.
        if (Directory.Exists(directoryUnderChange.FullName) is false)
        {
            return NoMoveDueError(
                "The given directory does not exist",
                currentDir);
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