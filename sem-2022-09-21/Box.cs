// // Solution: hse
// // Created at 2022-09-21 17:17
// // Author: Черных Владимир Артемович
// // Group: БПИ229

using System.Text.Json;

namespace sem_2022_09_20;

internal class Box
{
    public long Id { get; }
    private static long _curId = -1;
    public uint Capacity { get; }
    public List<object> Items { get; }

    public Box(uint capacity)
    {
        Id = ++_curId;
        Capacity = capacity;
        Items = new List<object>((int)capacity);
    }

    public Box(uint capacity, List<object> items) : this(capacity)
    {
        if (items.Count > Capacity)
        {
            throw new OverflowException("Overflow");
        }

        Items.AddRange(items);
    }

    public Box(uint capacity, Box box) : this(capacity)
    {
        if (Items.Count + box.Items.Count > Capacity)
        {
            throw new OverflowException("Overflow");
        }

        Items.AddRange(box.Items);
    }

    public bool Add(object item)
    {
        if (Items.Count + 1 > Capacity)
        {
            return false;
        }

        Items.Add(item);
        return true;
    }

    public bool AddRange(List<object> newItems)
    {
        if (Items.Count + newItems.Count > Capacity)
        {
            return false;
        }

        Items.AddRange(newItems);
        return true;
    }

    public override string ToString()
        // => $"Box #{Id}: {{ capacity: {Capacity}, items: [{Environment.NewLine}\t\t{String.Join($",{Environment.NewLine}\t\t", Items)}{Environment.NewLine}]}}";
        => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
}