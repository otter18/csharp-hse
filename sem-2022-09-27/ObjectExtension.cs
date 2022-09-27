// Solution: hse - sem-2022-09-27 - ObjectExtension.cs
// Created at 2022-09-27 19:03
// Author: Черных Владимир Артемович
// Group: БПИ229

namespace sem_2022_09_27;

using System.Text.Json;

static class ObjectExtension
{
    public static string Beautify<T>(this T x)
    {
        return JsonSerializer.Serialize(x, new JsonSerializerOptions { WriteIndented = true });
    }
}