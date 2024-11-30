using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shurl.Core;
using Shurl.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ShurlDbContext>(options => options.UseInMemoryDatabase("shurl"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();


if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/shorten", (string url) =>
{
    UrlService service = new UrlService(new HashService());
    return service.Shorten(url);
})
.WithName("ShortenUrl");

app.Run();

