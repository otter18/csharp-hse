using sem_2022_10_04;

var a = new DataFrame(new Dictionary<string, List<object>>
{
    {
        "Name", new List<object> { "Andrash", "Vova", "Vova", "Vitya" }
    },
    {
        "Num", new List<object> { 1, 179, 111, -57 }
    },
    {
        "Dec", new List<object> { 0.1, -1.5, 3.33, 12.1 }
    }
});

Console.WriteLine(a);

var b = a[a["Name"] == "Vova"];
Console.WriteLine(b);

Console.WriteLine(b["Dec"].Sum());
Console.WriteLine(b["Dec"].Min());
Console.WriteLine(b["Dec"].Max());
Console.WriteLine(b["Dec"].Mean());

