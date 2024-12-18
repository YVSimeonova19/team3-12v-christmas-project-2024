using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class EffectValidator : LightValidator
    {
        private static readonly HashSet<string> ValidEffects = new(StringComparer.OrdinalIgnoreCase)
        {
            "g1",
            "g2",
            "g3",
        };

        public override async Task<bool> ValidateLightAsync(Light light)
        {
            if (light.Effect != null && !ValidEffects.Contains(light.Effect))
            {
                return false;
            }

            return await this.ValidateNext(light);
        }
    }
}