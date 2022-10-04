// Solution: hse - sem-2022-10-04 - DataFrame.cs
// Created at 2022-10-04 19:01
// Author: Филимонов Виктор Павлович
// Group: БПИ229

namespace sem_2022_10_04;

public class DataFrameColumn
{
    public string Name { get; set; }
    public List<object> _data;
    public int Len => _data.Count;
    
    public object this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }
    
    public static DataFrameMask operator ==(DataFrameColumn a, object b)
    {
        var resMask = new DataFrameMask(a.Len);
        for (var i = 0; i < a.Len; ++i)
        {
            resMask[i] = a[i].Equals(b);
        }

        return resMask;
    }

    public static DataFrameMask operator !=(DataFrameColumn a, object b)
    {
        return !(a == b);
    }

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

    public static DataFrameMask operator <(DataFrameColumn a, object b)
    {
        return (a != b) & !(a > b); // а это правда вообще?
    }

    public static DataFrameMask operator >=(DataFrameColumn a, object b)
    {
        return (a == b) | (a > b);
    }

    public static DataFrameMask operator <=(DataFrameColumn a, object b)
    {
        return (a == b) | (a < b);
    }

    public DataFrameColumn(string name, List<object> data)
    {
        Name = name;
        _data = data;
    }
}