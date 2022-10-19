// Solution: hse - sem-2022-10-12 - CommandErrorException.cs
// Created at 2022-10-17 19:45
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_12;

public class CommandErrorException : ApplicationException
{
    public CommandErrorException() : base("Undefined error! Try again")
    {
    }

    public CommandErrorException(string message) : base(message)
    {
    }

    public CommandErrorException(string message, Exception inner) : base($"{message}\n\n{inner}")
    {
    }
}