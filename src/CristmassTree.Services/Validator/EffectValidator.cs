using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class EffectValidator : LightValidator
    {
        private static readonly HashSet<string> ValidEffects = new(StringComparer.OrdinalIgnoreCase)
        {
            new string(Effect.G1),
            new string(Effect.G2),
            new string(Effect.G3),
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