// Solution: hse - sem-2022-09-27 - Courier.cs
// Created at 2022-09-27 18:33
// Author: Рудинский Матвей Александрович 
// Group: БПИ229

namespace sem_2022_09_27;

public class Courier : Person
{
    private readonly List<Box> _parcels = new List<Box>();
    public int AmountOfParcels => _parcels.Count;

    private int _yearsOfWorkExperience = -1;
    private int _boxesDelivered = 0;

    public int Salary { get; set; } = -1;

    public void GetNewParcel(Box parcel)
    {
        _parcels.Add(parcel);
    }

    public void BoxDelivered()
    {
        _boxesDelivered++;

        var rnd = new Random();
        if (AmountOfParcels == 0)
        {
            throw new ArgumentException("No one have shown up");
        }

        _parcels.RemoveAt(rnd.Next() % AmountOfParcels);
    }

    public Courier(
        string name,
        int age,
        GenderType gender,
        int salary,
        int yearsOfWorkExperience = 0,
        int boxesDelivered = 0)
        : base(name, age, gender)
    {
        this._yearsOfWorkExperience = yearsOfWorkExperience;
        this.Salary = salary;
        this._boxesDelivered = boxesDelivered;
    }
}