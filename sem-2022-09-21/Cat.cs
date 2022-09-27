// // Solution: hse
// // Created at 2022-09-21 17:16
// // Author: Черных Владимир Артемович
// // Group: БПИ229

using System.Text.Json;

namespace sem_2022_09_20;

internal class Cat
{
    public enum GenderType
    {
        Unknown,
        Male,
        Female
    }

    private static long _currId = -1;
    public long Id { get; }

    private string _name = "";

    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is >= 3 and <= 10)
            {
                _name = value;
            }
            else
            {
                throw new ArgumentException("Wrong name length");
            }
        }
    }

    private byte _age;

    public byte Age
    {
        get => _age;
        init =>
            _age = value is >= 0 and <= 20
                ? value
                : throw new ArgumentException("Wrong age value, should be from 0 to 20");
    }

    public GenderType Gender { get; }

    public Cat(string name, byte age, GenderType gender = GenderType.Unknown)
    {
        Id = ++_currId;
        Name = name;
        Age = age;
        Gender = gender;
    }

    public override string ToString()
        // => $"Cat #{Id}: {{ name: {Name}, age: {Age}, gender: {Gender} }}";
        => JsonSerializer.Serialize(this);
}