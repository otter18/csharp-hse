// Solution: hse - sem-2022-09-27 - Box.cs
// Created at 2022-09-27 18:33
// Author: Тот Андраш Чабович
// Group: БПИ229


namespace sem_2022_09_27;

/// <summary>
/// Содержимое коробки.
/// </summary>
public abstract class BoxValue
{
    /// <summary>
    /// Описание содержимого
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Вес содержимого в кг.
    /// </summary>
    public abstract double Weight { get; }
}