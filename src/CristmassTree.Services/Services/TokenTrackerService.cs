using CristmassTree.Services.Contracts;

namespace CristmassTree.Services.Services
{
    public class TokenTrackerService : ITokenTracker
    {
        private readonly LightService lightService;
        private readonly ICurrentToken currentToken;

        public TokenTrackerService(LightService lightService, ICurrentToken currentToken)
        {
            this.lightService = lightService;
            this.currentToken = currentToken;
        }

        public async Task TrackTokenAsync(string token)
        {
            var lastToken = currentToken.GetToken();

            if (lastToken != token)
            {
                await lightService.DeleteOldAsync(token);
                currentToken.SetToken(token);
            }

            await Task.CompletedTask;
        }
    }
}