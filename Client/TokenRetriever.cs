using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OktaOAuthPOC.Models;

namespace OktaOAuthPOC.Client
{
    public class TokenRetriever
    {
        private static WebClient _webClient;

        public TokenRetriever()
        {
            _webClient = new WebClient();
        }

        public async Task<string> GetAccessToken(string authorizationCode, string codeVerifier, OAuthParams oAuthParams)
        {
            _webClient.Headers.Add("Accept", "application/json");
            _webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string query;
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {Constants.CLIENT_ID, oAuthParams.ClientId},
                {Constants.REDIRECT_URI, oAuthParams.RedirectUrl},
                {Constants.GRANT_TYPE, Constants.GRANT_TYPE_AUTHORIZATION_CODE},
                {Constants.CODE_VERIFIER, codeVerifier},
                {Constants.RESPONSE_TYPE_CODE, authorizationCode}
            }))
            {
                query = await content.ReadAsStringAsync();
            }

            // The token url with the query is posted by the client to get the Access Token.
            var json = _webClient.UploadString(oAuthParams.TokenUrl, query);

            var response = JsonConvert.DeserializeObject<TokenResponse>(json);
            return response?.access_token;
        }
    }
}