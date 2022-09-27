// Solution: hse - sem-2022-09-27 - Table.cs
// Created at 2022-09-27 19:09
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_09_27;

public class Table : BoxValue
{
    public override string Description { get; }
    public override double Weight { get; }

    public Table(string description, double weight)
    {
        Description = description;
        Weight = weight;
    }
}