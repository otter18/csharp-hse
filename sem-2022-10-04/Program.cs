using sem_2022_10_04;

// var a = new DataFrame(new Dictionary<string, List<object>>
// {
//     {
//         "A", new List<object> { 1, 2, 3 }
//     },
//     {
//         "B", new List<object> { 4, 5, 6 }
//     }
// });

var mask1 = new DataFrameMask(new List<bool> { false, true, true });
var mask2 = new DataFrameMask(new List<bool> { false, true, false });
var mask3 = mask1 & mask2;

Console.WriteLine(mask3);

/*
A B
1 4
2 5
3 6    


a[f() => a["A"]]

a[a["A"] == 10]
a[(a["A"] == 10) & (a["B"] > 10)]

*/