using sem_2022_09_27;

Console.SetOut(new StreamWriter("result.txt"));

Console.WriteLine("---------- Person ----------");
var person = new Person("test-person", 22, Person.GenderType.Male);
Console.WriteLine(person.Beautify());

Console.WriteLine("\n\n---------- Courier ----------");
var courier = new Courier("test-courier", 22, Person.GenderType.Male, 1000);
Console.WriteLine(courier.Beautify());

Console.WriteLine("\n\n---------- Manager ----------");
var manager = new Manager("test-courier", 22, Person.GenderType.Male);
manager.AddCourier(courier);
Console.WriteLine(manager.Beautify());

Console.WriteLine("\n\n---------- Box ----------");
var box = new Box("HSE Moscow", new Table("literally table", -1.1));
Console.WriteLine(box.Beautify());

Console.WriteLine("\n\n---------- ProcessOrder ----------");
Console.WriteLine($"Before: courier.AmountOfParcels - {courier.AmountOfParcels}, Manager.AllOrdersCount - {Manager.AllOrdersCount}");
manager.ProcessOrder(box);
Console.WriteLine($"After: courier.AmountOfParcels - {courier.AmountOfParcels}, Manager.AllOrdersCount - {Manager.AllOrdersCount}");

Console.Out.Close();