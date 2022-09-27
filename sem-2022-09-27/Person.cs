// Solution: hse - sem-2022-09-27 - Person.cs
// Created at 2022-09-27 18:27
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_09_27;

using System.Text.Json;

public class Person
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
        set =>
            _name = value.Length is >= 3 and <= 20
                ? value
                : throw new ArgumentException("Wrong name length");
    }

    private int _age;

    public int Age
    {
        get => _age;
        init =>
            _age = value is >= 18 and <= 60
                ? value
                : throw new ArgumentException("Wrong age value, should be from 18 to 65");
    }

    public GenderType Gender { get; }

    public Person() { }
    
    public Person(string name, int age, GenderType gender = GenderType.Unknown)
    {
        Id = ++_currId;
        Name = name;
        Age = age;
        Gender = gender;
    }
}