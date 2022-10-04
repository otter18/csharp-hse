// Solution: hse - sem-2022-10-04 - DataFrame.cs
// Created at 2022-10-04 19:01
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_04;

internal partial class DataFrame
{
    private Dictionary<string, DataFrameColumn> _data;

    public DataFrame(Dictionary<string, List<object>> data)
    {
        _data = new Dictionary<string, DataFrameColumn>(data.Count);
        foreach (var key in data.Keys)
        {
            _data.Add(key, new DataFrameColumn(key, data[key]));
        }
    }

    public string[] Columns => _data.Keys.ToArray();
    public Tuple<int, int> Shape => new Tuple<int, int>(_data.Keys.Count, _data[this.Columns[0]].Len);

    public override string ToString()
    {
        var res = "";
        return default;
    }
}