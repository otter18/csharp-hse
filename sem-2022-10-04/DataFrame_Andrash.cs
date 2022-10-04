namespace sem_2022_10_04;

// Solution: hse - sem-2022-10-04 - DataFrameMask.cs
// Created at 2022-10-04 19:44
// Author: Тот Андраш Чабович
// Group: БПИ229


internal partial class DataFrame
{
    /// <summary>
    /// Returns the representation of the column with the given name.
    /// </summary>
    public DataFrameColumn this[string columnName]
    {
        get
        {
            ValidateColumnName(columnName);
            return new DataFrameColumn(columnName, _data[columnName]);
        }
        set
        {
            ValidateColumnName(columnName);
            _data[columnName] = value._data;
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
    /// Returns the number of elements of the column with the given name according to the given mask.
    /// </summary>
    public List<object> this[string columnName, DataFrameMask mask]
    {
        get
        {
            ValidateColumnName(columnName);
            List<object> chosenColumn = _data[columnName];
            
            ValidateMask(mask, chosenColumn.Count);

            List<object> res = new List<object>();
            for (int i = 0; i < Math.Min(chosenColumn.Count, mask.len); i++)
            {
                if (mask[i])
                {
                    res.Add(chosenColumn[i]);
                }
            }

            return res;
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
        
        if (_data.ContainsKey(columnName!) is false)
        {
            throw new ArgumentException($"No column with such name: {columnName}");
        }
    }

    /// <summary>
    /// Validates the index in the for the column with given name.
    /// </summary>
    private void ValidateIndex(string columnName, int index)
    {
        if (index >= _data[columnName!].Count)
        {
            throw new ArgumentException($"No element with such index in {columnName} column: {index}");
        }
    }

    /// <summary>
    /// Validates the mask.
    /// </summary>
    private void ValidateMask(DataFrameMask? mask, int columnLength)
    {
        if (mask is null)
        {
            throw new ArgumentNullException(nameof(mask));
        }

        if (mask.len != columnLength)
        {
            throw new ArgumentException("The column and the given mask are not of the same size.");
        }
    }
    
    private void ValidateColumnNameIndex(string? columnName, int index)
    {
        ValidateColumnName(columnName);
        ValidateIndex(columnName!, index);
    }
}