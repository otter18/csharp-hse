// Solution: hse - sem-2022-09-27 - Manager.cs
// Created at 2022-09-27 18:42
// Author: Филимонов Виктор Павлович
// Group: БПИ229

namespace sem_2022_09_27;

public class Manager : Person
{
    // суммарное количество обработанных заказов
    public static int AllOrdersCount { get; private set; } = 0;

    // Список всех курьеров, с которыми может работать
    private List<Courier> ListOfCouriers { get; } = new List<Courier>();

    public int CurrentCouriersCount => ListOfCouriers.Count;


    public void AddCourier(Courier courier)
    {
        ListOfCouriers.Add(courier);
    }

    public void AddRangeCourier(List<Courier> couriers)
    {
        ListOfCouriers.AddRange(couriers);
    }

    public bool RemoveCourier(Courier courier)
    {
        return ListOfCouriers.Remove(courier);
    }

    public void ProcessOrder(Box box)
    {
        var random = new Random();
        int courierId = random.Next(ListOfCouriers.Count);
        if (ListOfCouriers.Count == 0)
        {
            throw new ArgumentException("No one has shown up");
        }

        ListOfCouriers[courierId].GetNewParcel(box); // функция Матвея по доставке коробке
        ++AllOrdersCount;
    }

    public Manager(
        string name = "Unknown",
        int age = 20,
        GenderType gender = GenderType.Unknown)
        : base(name, age, gender)
    {
    }
}