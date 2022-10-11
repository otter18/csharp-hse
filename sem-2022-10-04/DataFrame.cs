// Solution: hse - sem-2022-10-04 - DataFrame.cs
// Created at 2022-10-04 19:01
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_04;

public partial class DataFrame
{
    private readonly Dictionary<string, DataFrameColumn> _data;

    public DataFrame(Dictionary<string, List<object>> data)
    {
        _data = new Dictionary<string, DataFrameColumn>(data.Count);
        foreach (var key in data.Keys)
        {
            _data.Add(key, new DataFrameColumn(key, data[key]));
        }
    }

    public string[] Columns => _data.Keys.ToArray();
    public Tuple<int, int> Shape => new Tuple<int, int>(Columns.Length, _data[Columns[0]].Len);

    public override string ToString()
    {
        var colWidth = new List<int>(Shape.Item1);
        foreach (var key in Columns)
        {
            var maxWidth = key.Length;
            foreach (var elem in _data[key].Data)
            {
                maxWidth = Math.Max(maxWidth, elem.ToString()!.Length);
            }

            colWidth.Add(maxWidth);
        }

        var res = "";
        for (var i = 0; i < Shape.Item1; i++)
        {
            res += Columns[i].PadRight(colWidth[i]);
            res += "  ";
        }

        res += '\n';

        for (var j = 0; j < Shape.Item2; j++)
        {
            for (var i = 0; i < Shape.Item1; i++)
            {
                res += _data[Columns[i]][j]
                    .ToString()!
                    .PadRight(colWidth[i]);
                res += "  ";
            }

            res += '\n';
        }

        return res;
    }
}