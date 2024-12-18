using CristmassTree.Presentation.Controllers;
using CristmassTree.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_corsPolicy",
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