using CristmassTree.Data.Models;
using CristmassTree.Services.Contracts;

namespace CristmassTree.Services.Validator
{
    public abstract class LightValidator : ILightValidator
    {
        protected ILightValidator? NextValidator { get; private set; }

        public ILightValidator SetNext(ILightValidator validator)
        {
            this.NextValidator = validator;
            return validator;
        }

        public abstract Task<bool> ValidateLightAsync(Light light);

        protected async Task<bool> ValidateNext(Light light)
        {
            if (this.NextValidator == null)
            {
                return true;
            }

            return await this.NextValidator.ValidateLightAsync(light);
        }
    }
}