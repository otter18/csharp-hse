using System.Text.Json;
using sem_2022_09_20;

var cat1 = new Cat("python", 12, Cat.GenderType.Male);
var cat2 = new Cat("cpp", 20, Cat.GenderType.Female);
var cat3 = new Cat("csharp", 20, Cat.GenderType.Unknown);

var box1 = new Box(2, new List<object> { cat1, cat2 });
Console.WriteLine(box1);

var box2 = new Box(1);
Console.WriteLine($"box2.Add(cat1): {box2.Add(cat1)}, box2.Add(cat2): {box2.Add(cat2)}");
Console.WriteLine(box2);

var box3 = new Box(3);
Console.WriteLine(
    $"add 1: {box3.AddRange(new List<object> { cat1, cat2 })}, add 2: {box3.AddRange(new List<object> { cat1, cat2 })}");
Console.WriteLine($"add 3: {box3.Add(new Cat("pascal", 15))}");
Console.WriteLine(box3);

var box4 = new Box(10, box3);
box4.Add(cat3);
Console.WriteLine(box4);

box4.Add(new List<int> { 1, 7, 9 });

var cat5 = box4.Items[^1] as Cat;
Console.WriteLine(cat5);
Console.WriteLine(cat5?.Name);



var cat57 = new Cat("qwerty", 13);


var s = cat57.ToJson();
Console.WriteLine(s);

var cat58 = JsonSerializer.Deserialize<Cat>(s);
Console.WriteLine(cat58);