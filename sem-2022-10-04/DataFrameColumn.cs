// Solution: hse - sem-2022-10-04 - DataFrameColumn.cs
// Created at 2022-10-04 19:01
// Author: Филимонов Виктор Павлович
// Group: БПИ229

using System.Text.Json;

namespace sem_2022_10_04;

public partial class DataFrameColumn
{
    private bool Equals(DataFrameColumn other)
    {
        return Data.Equals(other.Data) && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DataFrameColumn)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Data, Name);
    }

    // Column name
    public string? Name { get; init; }

    // Column data (should be renamed, but we haven't chosen it's access modifier)
    public readonly List<object> Data;

    // the length of _data
    public int Len => Data.Count;

    /// <summary>
    /// Provides access to _data
    /// </summary>
    /// <param name="index"></param>
    public object this[int index]
    {
        get => Data[index];
        set => Data[index] = value;
    }

    /// <summary>
    /// Returns a mask, showing all the elements equal to b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator ==(DataFrameColumn a, object b)
    {
        var resMask = new DataFrameMask(a.Len);
        for (var i = 0; i < a.Len; ++i)
        {
            resMask[i] = a[i].Equals(b);
        }

        return resMask;
    }

    /// <summary>
    /// Returns a mask, showing all the elements not equal to b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator !=(DataFrameColumn a, object b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Returns a mask, showing all the elements more than b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator >(DataFrameColumn a, object b)
    {
        var resMask = new DataFrameMask(a.Len);

        if (!double.TryParse(b.ToString(), out var doubleB)) return resMask;

        for (var i = 0; i < a.Len; ++i)
        {
            resMask[i] = double.Parse(a[i].ToString()!) > doubleB;
        }

        return resMask;
    }

    /// <summary>
    /// Returns a mask, showing all the elements less than b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator <(DataFrameColumn a, object b)
    {
        return (a != b) & !(a > b); // а это правда вообще?
    }

    /// <summary>
    /// Returns a mask, showing all the elements more than or equal to b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator >=(DataFrameColumn a, object b)
    {
        return (a == b) | (a > b);
    }

    /// <summary>
    /// Returns a mask, showing all the elements less than or equal to b
    /// </summary>
    /// <param name="a">Колонка</param>
    /// <param name="b">число для сравнения</param>
    /// <returns></returns>
    public static DataFrameMask operator <=(DataFrameColumn a, object b)
    {
        return (a == b) | (a < b);
    }

    public DataFrameColumn(string? name, List<object> data)
    {
        Name = name;
        Data = data;
    }

    public DataFrameColumn(List<object> data)
    {
        Data = data;
    }

    public override string ToString()
    {
        return $"{Name}: {JsonSerializer.Serialize(Data)}";
    }
}