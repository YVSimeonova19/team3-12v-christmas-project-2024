﻿using CristmassTree.Data.Models;
using CristmassTree.Services.Contracts;
using CristmassTree.Services.Validator;
using Microsoft.Extensions.Caching.Memory;

namespace CristmassTree.Services.Factory
{
    public class LightFactory : ILightFactory
    {
        private static readonly Random Random = new();
        private readonly ILightValidator validationChain;
        private readonly IMemoryCache memoryCache;
        private Light? lastLight;

        private static readonly List<string> Colors = new()
        {
            "blue-lt",
            "blue-dk",
            "red",
            "gold-lt",
            "gold-dk",
        };

        private static readonly List<string> Effects = new()
        {
            "g1",
            "g2",
            "g3",
        };

        public LightFactory(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.validationChain = new TrianglePositionValidator(httpClientFactory);
            this.validationChain.SetNext(new ColorValidator())
                            .SetNext(new EffectValidator())
                            .SetNext(new ExternalApiValidator());
            this.lastLight = this.memoryCache.TryGetValue("LastLight", out Light? l) ? l : null;
        }

        public async Task<Light> CreateLight(string description, string ct)
        {
            Light light;

            do
            {
                double x = (Random.NextDouble() * (125.80 - 0.00)) + 0.00;
                double y = (Random.NextDouble() * (170.30 - 14.90)) + 14.90;
                double radius = 3 + (Random.NextDouble() * (6 - 3));

                string color;
                string effect;

                do
                {
                    color = Colors[Random.Next(Colors.Count)];
                    effect = Effects[Random.Next(Effects.Count)];
                    if (lastLight == null)
                    {
                        Console.WriteLine("First | Last Light was null");
                        break;
                    }
                }
                while (color == lastLight.Color || effect == lastLight.Effect);

                light = new Light
                {
                    X = Math.Round(x, 2),
                    Y = Math.Round(y, 2),
                    Radius = Math.Round(radius, 2),
                    Color = color,
                    Effect = effect,
                    Description = description,
                    CT = ct,
                };
                if (await validationChain.ValidateLightAsync(light))
                {
                    break;
                }
            }
            while (true);

            lastLight = light;
            memoryCache.Set("LastLight", light);
            return light;
        }
    }
}