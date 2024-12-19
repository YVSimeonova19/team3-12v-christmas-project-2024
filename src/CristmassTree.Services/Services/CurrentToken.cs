using CristmassTree.Services.Contracts;

namespace CristmassTree.Services.Services
{
    public class CurrentToken : ICurrentToken
    {
        private string token;

        public CurrentToken()
        {
            this.token = string.Empty;
        }

        public string GetToken()
        {
            return token;
        }

        public void SetToken(string token)
        {
            this.token = token;
        }
    }
}