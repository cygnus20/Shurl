using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shurl.Core;
using Shurl.Data;
using Shurl.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ShurlDbContext>(options => options.UseInMemoryDatabase("shurl"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGetBaseUrl, GetBaseUrl>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();


if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/{**surl}", (string surl, [FromServices] ShurlDbContext context) =>
{
    var url = context.Urls.FirstOrDefault(u => u.ShortUrl == surl);

    if (url != null)
    {
        return Results.Redirect(url.LongUrl);
    }
    return Results.NotFound();
}).WithName("GotoUrl");

app.MapGet("/urls", async (ShurlDbContext context) =>
{
    var urls = await context.Urls.ToListAsync();

    return Results.Ok(urls);
}).WithName("GetUrls");

app.MapPost("/shorten", async (string url, ShurlDbContext context, IGetBaseUrl baseUrl) =>
{
    UrlService service = new UrlService(new HashService());
    var shortUrl =  $"{baseUrl.Url}{service.Shorten(url)}";
    Urls urls = new Urls { LongUrl = url, ShortUrl = shortUrl };
    await context.Urls.AddAsync(urls);
    await context.SaveChangesAsync();

    return Results.Ok(new { shortUrl = shortUrl });
})
.WithName("ShortenUrl");

app.MapDelete("/urls/{id}", async (int id, ShurlDbContext context) =>
{
    var url = context.Urls.FirstOrDefault(u  => u.Id == id);
    if (url != null)
    {
        context.Urls.Remove(url);
        await context.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
    

}).WithName("DeleteUrls");

app.Run();

