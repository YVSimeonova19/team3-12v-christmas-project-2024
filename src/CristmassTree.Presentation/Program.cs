using CristmassTree.Data;
using CristmassTree.Data.Data;
using CristmassTree.Presentation;
using CristmassTree.Presentation.Controllers;
using CristmassTree.Presentation.Middleware;
using CristmassTree.Services;
using CristmassTree.Services.Contracts;
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
            policy.WithOrigins(
                    "https://codingburgas.karagogov.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Modify the dependency injection setup
builder.Services.AddControllers();
builder.Services.AddScoped<LightFactory>();
builder.Services.AddScoped<LightService>();
builder.Services.AddScoped<ITokenTracker, TokenTrackerService>();
builder.Services.AddScoped<ColorValidator>();
builder.Services.AddScoped<EffectValidator>();
builder.Services.AddScoped<ExternalApiValidator>();

// Configure the validation chain
builder.Services.AddScoped<ILightValidator>(sp =>
{
    var validationChain = new TrianglePositionValidator();
    validationChain.SetNext(new ColorValidator())
        .SetNext(new EffectValidator())
        .SetNext(new ExternalApiValidator());
    return validationChain;
});

builder.Services.AddScoped<TrianglePositionValidator>();

var app = builder.Build();

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