// Solution: hse - sem-2022-09-21 - ObjectExtension.cs
// Created at 2022-09-21 18:58
// Author: Черных Владимир Артемович
// Group: БПИ229

using System.Text.Json;

internal static class ObjectExtension
{
    public static string ToJson<T>(this T x)
    {
        return JsonSerializer.Serialize(x);
    }
}