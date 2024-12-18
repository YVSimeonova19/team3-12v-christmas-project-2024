using CristmassTree.Services.Contracts;
using CristmassTree.Data.Models;
using CristmassTree.Services.Validator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CristmassTree.Services
{
    public class LightFactory : ILightFactory
    {
        private static readonly Random Random = new();
        private readonly LightValidator _lightValidator;

        private static readonly List<Color> Colors = new()
        {
            new Color { Name = Color.BlueLt },
            new Color { Name = Color.BlueDk },
            new Color { Name = Color.Red },
            new Color { Name = Color.GoldLt },
            new Color { Name = Color.GoldDk },
        };

        private static readonly List<Effect> Effects = new()
        {
            new Effect { Name = Effect.G1 },
            new Effect { Name = Effect.G2 },
            new Effect { Name = Effect.G3 },
        };

        public LightFactory(LightValidator lightValidator)
        {
            _lightValidator = lightValidator;
        }

        public async Task<Light> CreateLight(string description, string ct)
        {
            double x = Random.NextDouble() * (125.80 - 0.00) + 0.00;
            double y = Random.NextDouble() * (170.30 - 14.90) + 14.90;
            double radius = 3 + (Random.NextDouble() * (6 - 3));

            var color = Colors[Random.Next(Colors.Count)];
            var effect = Effects[Random.Next(Effects.Count)];

            var light = new Light
            {
                X = x,
                Y = y,
                Radius = radius,
                Color = color,
                Effect = effect,
                Description = description,
                CT = ct,
            };

            if (await _lightValidator.ValidateLightAsync(light))
            {
                return light;
            }

            return await CreateLight(description, ct);
        }
    }
}