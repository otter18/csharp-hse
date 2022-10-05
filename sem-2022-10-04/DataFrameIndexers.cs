// Solution: hse - sem-2022-10-04 - DataFrameIndexers.cs
// Created at 2022-10-04 19:44
// Author: Тот Андраш Чабович
// Group: БПИ229

namespace sem_2022_10_04;

public partial class DataFrame
{
    /// <summary>
    /// Returns the representation of the column with the given name.
    /// </summary>
    public DataFrameColumn this[string columnName]
    {
        get
        {
            ValidateColumnName(columnName);
            return new DataFrameColumn(columnName, _data[columnName].Data);
        }
        set
        {
            ValidateColumnName(columnName);
            if (Columns.Contains(columnName))
            {
                _data[columnName] = value;
            }
            else
            {
                _data.Add(columnName, new DataFrameColumn(columnName, value.Data));
            }
        }
    }


    /// <summary>
    /// Accesses the element with the given index from the column with given name. 
    /// </summary>
    public object this[string columnName, int index]
    {
        get
        {
            ValidateColumnNameIndex(columnName, index);
            return _data[columnName][index];
        }
        set
        {
            ValidateColumnNameIndex(columnName, index);
            _data[columnName][index] = value;
        }
    }

    /// <summary>
    /// Accesses the element with the given index from the column with given name. 
    /// </summary>
    public DataFrame this[Range a, Range b]
    {
        get
        {
            var res = new Dictionary<string, List<object>>();
            var newColumns = Columns[a];
            foreach (var col in newColumns)
            {
                res.Add(col, _data[col].Data.ToArray()[b].ToList());
            }

            return new DataFrame(res);
        }
    }


    /// <summary>
    /// Returns the number of elements of the column with the given name according to the given mask.
    /// </summary>
    public DataFrame this[DataFrameMask mask]
    {
        get
        {
            ValidateMask(mask);

            var res = new Dictionary<string, List<object>>();
            foreach (var key in Columns)
            {
                var col = new List<object>();
                for (var i = 0; i < Shape.Item2; i++)
                {
                    if (mask[i])
                    {
                        col.Add(_data[key][i]);
                    }
                }

                res.Add(key, col);
            }

            return new DataFrame(res);
        }
    }

    /// <summary>
    /// Validates the name of the column.
    /// </summary>
    private void ValidateColumnName(string? columnName)
    {
        if (columnName is null)
        {
            throw new ArgumentNullException(nameof(columnName));
        }
    }

    /// <summary>
    /// Validates the index in the for the column with given name.
    /// </summary>
    private void ValidateIndex(string? columnName, int index)
    {
        if (index >= _data[columnName!].Len)
        {
            throw new ArgumentException($"No element with such index in {columnName} column: {index}");
        }
    }

    /// <summary>
    /// Validates the mask.
    /// </summary>
    private void ValidateMask(DataFrameMask? mask)
    {
        if (mask is null)
        {
            throw new ArgumentNullException(nameof(mask));
        }

        if (mask.Len != Shape.Item2)
        {
            throw new ArgumentException("The dataframe and the given mask are not of the same size.");
        }
    }

    private void ValidateColumnNameIndex(string? columnName, int index)
    {
        ValidateColumnName(columnName);
        ValidateIndex(columnName!, index);
    }
}