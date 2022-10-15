// Solution: hse - sem-2022-10-12 - ConsoleState.cs
// Created at 2022-10-15 22:28
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_12;

public struct ConsoleState
{
    public DirectoryInfo CurrentDir { init; get; }
    public string Result { init; get; }

    public void Deconstruct(out string result, out DirectoryInfo directoryInfo)
    {
        result = Result;
        directoryInfo = CurrentDir;
    }
}