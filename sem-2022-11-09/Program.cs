using Utils = sem_2022_11_09.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello =)");
app.MapGet("/gen_random_name", Utils.GenRandomName);
app.MapGet("/get_links", Utils.ExtractUrls);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
