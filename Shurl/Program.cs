using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shurl.Core;
using Shurl.Data;
using Shurl.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ShurlDbContext>(
    options => options.UseInMemoryDatabase("shurl"));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ShurlDbContext>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGetBaseUrl, GetBaseUrl>();
builder.Services.AddScoped<IGetUserClaims, GetUserClaims>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapIdentityApi<IdentityUser>();
app.MapOpenApi();


if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapPost("/logout", async (SignInManager<IdentityUser> signInManger,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManger.SignOutAsync();
        return Results.Ok();
    }

    return Results.Unauthorized();
}).RequireAuthorization();


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

app.MapPost("/shorturl", async (string url, ShurlDbContext context, IGetBaseUrl baseUrl, IGetUserClaims claims) =>
{
    int nextId = 0;
    try
    {
        nextId = (int)context.Urls.Max(u => u.Id);
    } 
    catch (InvalidOperationException)
    {
        nextId = 0;
    }
    finally
    {
        nextId += 1;
    }
    UrlService service = new UrlService(new HashService());
    var urlKey = service.Shorten(nextId, url);
    var shortUrl =  $"{baseUrl.Url}{urlKey}";
    Urls urls = new Urls { UserId = claims.UserId, LongUrl = url, ShortUrl = urlKey };
    await context.Urls.AddAsync(urls);
    await context.SaveChangesAsync();

    return Results.Ok(new { shortUrl });
})
.WithName("ShortUrl");

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

