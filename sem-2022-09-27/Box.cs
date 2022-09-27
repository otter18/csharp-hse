// Solution: hse - sem-2022-09-27 - Box.cs
// Created at 2022-09-27 18:33
// Author: Тот Андраш Чабович
// Group: БПИ229

namespace sem_2022_09_27;

public class Box
{
    private static long _firstFreeIndex = 1;

    public long Id { get; private init; }

    /// <summary>
    /// Адрес, куда будет доставлена посылка.
    /// </summary>
    public string Adress { get; private init; }

    /// <summary>
    /// Содержимое коробки.
    /// </summary>
    public BoxValue Value { get; private init; }

    public Box(string adress, BoxValue value)
    {
        Id = _firstFreeIndex++;

        Adress = adress ?? throw new ArgumentNullException();
        Value = value ?? throw new ArgumentException();
    }

    public override string ToString()
        => $"Кробка с id {Id}, которая должна быть доставлена по адресу {Adress}. Содержит {Value.Description}.";
}