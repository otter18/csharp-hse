// Solution: hse - sem-2022-10-04 - DataFrame.cs
// Created at 2022-10-04 19:01
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_10_04;

internal class DataFrame
{
    private Dictionary<string, List<object>> _data;

    public DataFrame(Dictionary<string, List<object>> data)
    {
        _data = data;
    }

    public string[] Columns => _data.Keys.ToArray();
    public Tuple<int, int> Shape => new Tuple<int, int>(_data.Keys.Count, _data[this.Columns[0]].Count);

    public override string ToString()
    {
        var res = "";
        return default;
    }
}