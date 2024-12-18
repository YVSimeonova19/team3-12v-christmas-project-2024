namespace CristmassTree.Services.Contracts;

public interface ITokenTracker
{
    public Task TrackTokenAsync(string token);
}