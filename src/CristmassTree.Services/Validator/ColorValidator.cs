using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class ColorValidator : LightValidator
    {
        private static readonly HashSet<string> ValidColors = new(StringComparer.OrdinalIgnoreCase)
        {
            new string(Color.BlueLt),
            new string(Color.BlueDk),
            new string(Color.Red),
            new string(Color.GoldLt),
            new string(Color.GoldDk),
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