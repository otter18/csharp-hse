// Solution: hse - sem-2022-11-09 - index.cs
// Created at 2022-11-15 18:58
// Author: Черных Владимир
// Group: БПИ229

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class Index : ControllerBase
{
    [HttpGet("/")]
    public ContentResult IndexPage()
    {
        const string html = "<p>Hello from <i>@otter18</i></p>";
        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}