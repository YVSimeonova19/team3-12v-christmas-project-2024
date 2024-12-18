using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CristmassTree.Data.Models;
using CristmassTree.Services.Contracts;
using CristmassTree.Services.Validator;

namespace CristmassTree.Services
{
    public class LightFactory : ILightFactory
    {
        private static readonly Random Random = new();
        private readonly ILightValidator validationChain;

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

        public LightFactory(IHttpClientFactory httpClientFactory)
        {
            this.validationChain = new TrianglePositionValidator(httpClientFactory);
            this.validationChain.SetNext(new ColorValidator())
                            .SetNext(new EffectValidator())
                            .SetNext(new ExternalApiValidator());
        }

        public async Task<Light> CreateLight(string description, string ct)
        {
            double x = (Random.NextDouble() * (125.80 - 0.00)) + 0.00;
            double y = (Random.NextDouble() * (170.30 - 14.90)) + 14.90;
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

            if (await this.validationChain.ValidateLightAsync(light))
            {
                return light;
            }

            return await this.CreateLight(description, ct);
        }
    }
}