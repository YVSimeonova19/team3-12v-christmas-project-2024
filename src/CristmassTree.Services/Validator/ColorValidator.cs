using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class ColorValidator : LightValidator
    {
        private static readonly HashSet<string> ValidColors = new(StringComparer.OrdinalIgnoreCase)
        {
            "blue-lt",
            "blue-dk",
            "red",
            "gold-lt",
            "gold-dk",
        };

        public override async Task<bool> ValidateLightAsync(Light light)
        {
            if (light.Color != null && !ValidColors.Contains(light.Color))
            {
                return false;
            }

            return await this.ValidateNext(light);
        }
    }
}