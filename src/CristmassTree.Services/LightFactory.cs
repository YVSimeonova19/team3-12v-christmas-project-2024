using CristmassTree.Services.Contracts;
using CristmassTree.Services.Models;

namespace CristmassTree.Services;

public class LightFactory : ILightFactory
{
    private static readonly Random Random = new();
    private static readonly List<string> Colors = new() { "blue-lt", "blue-dk", "red", "gold-lt", "gold-dk" };
    private static readonly List<string> Effects = new() { "g1", "g2", "g3" };

    public Light CreateLight(string description, string ct)
    {
        var x = Random.NextDouble() * (125.80 - 0.00) + 0.00;
        var y = Random.NextDouble() * (170.30 - 14.90) + 14.90;
        var radius = 3 + (Random.NextDouble() * (6 - 3));
        var color = Colors[Random.Next(Colors.Count)];
        var effects = Effects[Random.Next(Effects.Count)];
        
        return new Light
        {
            X = x,
            Y = y,
            Radius = radius,
            Color = color,
            Effects = effects,
            Description = description,
            ChristmasToken = ct,
        };
    }
}