// Title: quiz1
// Date: 2022-09-14
// Author: Черных Владимир Артемович
// Group: БПИ229

using System.Text.RegularExpressions;

try
{
    var inp = Console.ReadLine();
    Utils.ValidationType t;
    while ((t = Utils.Validate(inp)) != Utils.ValidationType.Exit)
    {
        if (t == Utils.ValidationType.Ok)
        {
            Console.WriteLine($"Result {Utils.Solve(inp)}");
        }
        else if (t == Utils.ValidationType.Overflow)
        {
            Console.WriteLine($"Input numbers should be from {Utils.MIN} to {Utils.MAX}");
        }
        else
        {
            Console.WriteLine($"Invalid input, try again");
        }

        inp = Console.ReadLine();
    }
}
catch
{
    Console.WriteLine("Unexpected error, restart");
}


static class Utils
{
    public const int MIN = -100;
    public const int MAX = 100;
    const string PAT = @"(-?\d+)(\+|-)(-?\d+)";

    public enum ValidationType
    {
        Ok,
        Overflow,
        Exit,
        Error
    }

    /// <summary>
    /// This method validates user input
    /// </summary>
    /// <param name="inp">user input</param>
    /// <returns>ValidationType obj</returns>
    public static ValidationType Validate(string inp)
    {
        if (inp.StartsWith("exit"))
        {
            return ValidationType.Exit;
        }

        Regex r = new Regex(PAT, RegexOptions.IgnoreCase);
        var m = r.Match(inp);
        if (m.Success)
        {
            if (!int.TryParse(m.Groups[1].Value, out var a) ||
                !int.TryParse(m.Groups[3].Value, out var b) ||
                !(MIN <= a && a <= MAX && MIN <= b && b <= MAX))
            {
                return ValidationType.Overflow;
            }

            return ValidationType.Ok;
        }

        return ValidationType.Error;
    }

    /// <summary>
    /// This method solves correct equation
    /// </summary>
    /// <param name="inp">Input string too parse</param>
    /// <returns>Result</returns>
    public static int Solve(string inp)
    {
        Regex r = new Regex(PAT, RegexOptions.IgnoreCase);
        var m = r.Match(inp);
        int a = int.Parse(m.Groups[1].Value);
        string sign = m.Groups[2].Value;
        int b = int.Parse(m.Groups[3].Value);
        return sign == "+" ? a + b : a - b;
    }
}