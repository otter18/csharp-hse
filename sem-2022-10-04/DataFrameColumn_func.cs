// Solution: hse - sem-2022-10-04 - DataFrameColumn_func.cs
// Created at 2022-10-04 22:51
// Author: Рудинский Матвей Александрович 
// Group: БПИ229

namespace sem_2022_10_04;

public partial class DataFrameColumn
{
    public double Sum()
    {
        double s = 0;
        for (var i = 0; i < Data.Count; i++)
        {
            if (double.TryParse(Data[i].ToString(), out var c))
            {
                s += c;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        return s;
    }

    public double Mean() => Sum() / Data.Count;

    public double Max()
    {
        double s = Double.MinValue;
        for (var i = 0; i < Data.Count; i++)
        {
            if (double.TryParse(Data[i].ToString(), out var c))
            {
                s = Math.Max(s, c);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        return s;
    }

    public double Min()
    {
        double s = Double.MaxValue;
        for (var i = 0; i < Data.Count; i++)
        {
            if (double.TryParse(Data[i].ToString(), out var c))
            {
                s = Math.Min(s, c);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        return s;
    }
}