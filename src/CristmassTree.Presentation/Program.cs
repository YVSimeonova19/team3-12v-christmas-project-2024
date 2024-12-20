using CristmassTree.Data.Data;
using CristmassTree.Presentation.Middleware;
using CristmassTree.Services.Contracts;
using CristmassTree.Services.Factory;
using CristmassTree.Services.Services;
using CristmassTree.Services.Validator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")!, o =>
    {
        o.MigrationsAssembly(typeof(Program).Assembly.FullName);
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_corsPolicy",
        policy =>
        {
            policy.WithOrigins("https://codingburgas.karagogov.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Register HttpClient
builder.Services.AddHttpClient();

// Register services
builder.Services.AddControllers();
builder.Services.AddTransient<LightFactory>();
builder.Services.AddSingleton<ICurrentToken, CurrentToken>();
builder.Services.AddScoped<ITokenTracker, TokenTrackerService>();
builder.Services.AddScoped<LightService>();
builder.Services.AddScoped<ColorValidator>();
builder.Services.AddScoped<EffectValidator>();
builder.Services.AddScoped<ExternalApiValidator>();
builder.Services.AddMemoryCache();

// Configure the validation chain
builder.Services.AddScoped<ILightValidator>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var validationChain = new TrianglePositionValidator(httpClientFactory);
    validationChain.SetNext(sp.GetRequiredService<ColorValidator>())
        .SetNext(sp.GetRequiredService<EffectValidator>())
        .SetNext(sp.GetRequiredService<ExternalApiValidator>());
    return validationChain;
});

builder.Services.AddScoped<TrianglePositionValidator>();

var app = builder.Build();

// header bypass for scripts? (last resort solution)
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "script-src 'self' ");
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("_corsPolicy");

app.UseMiddleware<TokenCaptureMiddleware>();

app.MapControllers();

app.Run();
