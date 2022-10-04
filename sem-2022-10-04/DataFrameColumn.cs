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

    public DataFrameColumn(string name, List<object> data)
    {
        Name = name;
        _data = data;
    }
}