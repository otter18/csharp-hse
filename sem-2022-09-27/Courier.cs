// Solution: hse - sem-2022-09-27 - Courier.cs
// Created at 2022-09-27 18:33
// Author: Рудинский Матвей Александрович 
// Group: БПИ229

namespace sem_2022_09_27;

public class Courier : Person
{
    private List<Box> _parcels = new List<Box>();
    public int AmountOfParcels { get; private set; } = 0;

    private int _salary = -1;
    private int _yearsOfWorkExperience = -1;
    private int _boxesDelivered = 0;

    public int Salary
    {
        get
            => _salary;
        set
            => _salary = value;
    }

    public void GetNewParcel(Box parcel)
    {
        _parcels.Add(parcel);
        AmountOfParcels++;
    }

    public void BoxDelivered()
    {
        AmountOfParcels--;
        _boxesDelivered++;

        Random rnd = new Random();
        if (AmountOfParcels == 0)
        {
            throw new ArgumentException("No one have shown up");
        }

        _parcels.RemoveAt(rnd.Next() % AmountOfParcels);
    }

    public Courier(string name, int age, GenderType gender,
        int salary, int yearsOfWorkExpirience = 0, int boxesDelivered = 0)
        : base(name, age, gender)
    {
        this._yearsOfWorkExperience = yearsOfWorkExpirience;
        this._salary = salary;
        this._boxesDelivered = boxesDelivered;
    }
}