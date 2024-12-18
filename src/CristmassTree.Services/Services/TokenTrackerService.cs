using CristmassTree.Services.Contracts;

namespace CristmassTree.Services.Services;

public class TokenTrackerService : ITokenTracker
{
    private readonly LightService lightService;
    private string lastToken = string.Empty;

    public TokenTrackerService(LightService lightService)
    {
        this.lightService = lightService;
    }

    public async Task TrackTokenAsync(string token)
    {
        if (lastToken != token)
        {
            await lightService.DeleteOldAsync(token);
            lastToken = token;
        }

        await Task.CompletedTask;
    }
}