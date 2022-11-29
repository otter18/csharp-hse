// Solution: hse - sem-2022-11-09 - Utils.cs
// Created at 2022-11-09 17:56
// Author: Черных Владимир
// Group: БПИ229

using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace sem_2022_11_09;

public static class Utils
{
    private static Regex urlPat = new(@"<a.*href=""(?<url>.+?)"".*>(?<name>.*?)</a>");
    private static Regex bodyPat = new(@"<body.*?>((.|\n|\t)*)</body>");
    private static Random rnd = new();

    public static IResult GenRandomName(string s, int n, int len)
    {
        if (s.Length == 0 || s.ToCharArray().Distinct().Count() != s.Length)
        {
            return Results.BadRequest("s should be non empty string of unique chars");
        }

        if (n <= 0)
        {
            return Results.BadRequest("n should be greater than 0");
        }

        var res = new List<string>();
        for (var i = 0; i < n; i++)
        {
            var tmp = "";
            for (var j = 0; j < len; j++)
            {
                tmp += s[rnd.Next(s.Length - 1)];
            }

            res.Add(tmp);
        }

        return Results.Ok(res);
    }


    public static async Task<IResult> ExtractUrls(string url)
    {
        try
        {
            using var client = new HttpClient();
            var resp = await client.GetStringAsync(url);
            resp = bodyPat.Match(resp).Groups[1].Value;

            var res = new List<object>();
            foreach (Match match in urlPat.Matches(resp))
            {
                var tmp = HttpUtility.UrlDecode(match.Groups["url"].Value);
                if (tmp.StartsWith("/wiki/"))
                {
                    res.Add(new
                    {
                        Name = match.Groups["name"].Value,
                        Url = url[..(url[^1] == '/' ? ^1 : url.Length)] + match.Groups["url"].Value,
                        CleanUrl = tmp
                    });
                }
            }

            return Results.Ok(res);
        }
        catch (Exception e)
        {
            return Results.BadRequest($"Oops! {e.Message}");
        }
    }
}