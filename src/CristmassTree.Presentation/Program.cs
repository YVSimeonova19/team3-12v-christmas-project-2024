using CristmassTree.Data;
using CristmassTree.Data.Data;
using CristmassTree.Presentation;
using CristmassTree.Presentation.Controllers;
using CristmassTree.Services;
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

builder.Services.AddControllers();
builder.Services.AddSingleton<LightFactory>();
builder.Services.AddSingleton<LightValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("_corsPolicy");

app.MapControllers();

app.Run();