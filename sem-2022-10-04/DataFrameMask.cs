// Solution: hse - sem-2022-10-04 - DataFrameMask.cs
// Created at 2022-10-04 19:44
// Author: Черных Владимир Артемович
// Group: БПИ229

using System.Text.Json;

namespace sem_2022_10_04;

public class DataFrameMask
{
    public int len => _mask.Count;
    public List<bool> _mask;

    public bool this[int index]
    {
        get => _mask[index];
        set => _mask[index] = value;
    }

    public static DataFrameMask operator &(DataFrameMask mask1, DataFrameMask mask2)
    {
        var resMask = new DataFrameMask(mask1.len);
        for (var i = 0; i < resMask.len; i++)
        {
            resMask[i] = mask1[i] & mask2[i];
        }

        return resMask;
    }
    
    public static DataFrameMask operator !(DataFrameMask mask)
    {
        var resMask = new DataFrameMask(mask.len);
        for (int i = 0; i < resMask.len; i++)
        {
            resMask[i] = !mask[i];
        }

        return resMask;
    }

    public DataFrameMask(int len)
    {
        _mask = new List<bool>(len);
        for (var i = 0; i < len; i++)
        {
            _mask.Add(false);
        }
    }

    public DataFrameMask(List<bool> mask)
    {
        _mask = mask;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(_mask);
    }
}