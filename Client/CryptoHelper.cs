using System.Security.Cryptography;
using System.Text;
using IdentityModel;
using OktaOAuthPOC.Models;

namespace OktaOAuthPOC.Client
{
    public class CryptoHelper
    {
        public string CreateState() => CryptoRandom.CreateUniqueId(Constants.DEFAULT_LENGTH_OF_STATE);

        public Pkce GetPkceData()
        {
            var pkce = new Pkce
            {
                CodeVerifier = CryptoRandom.CreateUniqueId()
            };

            using var sha256 = SHA256.Create();
            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pkce.CodeVerifier));
            pkce.CodeChallenge = Base64Url.Encode(challengeBytes);

            return pkce;
        }
    }
}