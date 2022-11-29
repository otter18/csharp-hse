using System.Net;
using Microsoft.AspNetCore.Mvc;

using Utils = sem_2022_11_09.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/gen_random_name", Utils.GenRandomName);
app.MapGet("/get_links", Utils.ExtractUrls);

app.Run();

