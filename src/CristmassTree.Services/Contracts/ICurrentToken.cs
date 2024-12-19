namespace CristmassTree.Services.Contracts;

public interface ICurrentToken
{
    string GetToken();
    void SetToken(string token);
}