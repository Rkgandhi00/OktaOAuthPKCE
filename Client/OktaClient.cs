using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Server;
using OktaOAuthPOC.Models;

namespace OktaOAuthPOC.Client
{
    public class OktaClient
    {
        private static BrowserOptions _browserOptions;
        private static CryptoHelper _helper;
        private static TokenRetriever _tokenRetriever;

        public OktaClient()
        {
            _browserOptions = new BrowserOptions();
            _helper = new CryptoHelper();
            _tokenRetriever = new TokenRetriever();
        }

        public async Task<string> GetBearerToken()
        {
            // Fetching values for OAuth Params which are configured in App.Config
            var oAuthParams = GetOAuthParams();

            // Initialize the WebListener to listen to the redirect_url.
            // Because once the finalAuthUrl is opened in the browser and if the user is authenticated successfully
            // then Okta server will redirect to mentioned redirect_url with the Authorization Code.
            var settings = new WebListenerSettings();
            settings.UrlPrefixes.Add(oAuthParams.RedirectUrl);

            var webListener = new WebListener(settings);
            webListener.Start();

            // Fetch code-challenge and code-verifier
            var pkce = _helper.GetPkceData();

            // setting up the required parameters for url to get the Authorization code.
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(Constants.RESPONSE_TYPE, Constants.RESPONSE_TYPE_CODE),
                new KeyValuePair<string, string>(Constants.STATE, _helper.CreateState()),
                new KeyValuePair<string, string>(Constants.CODE_CHALLENGE, pkce.CodeChallenge),
                new KeyValuePair<string, string>(Constants.CODE_CHALLENGE_METHOD,
                    Constants.CODE_CHALLENGE_METHOD_TYPE_S256),
                new KeyValuePair<string, string>(Constants.CLIENT_ID, oAuthParams.ClientId),
                new KeyValuePair<string, string>(Constants.SCOPE, oAuthParams.Scope),
                new KeyValuePair<string, string>(Constants.REDIRECT_URI, oAuthParams.RedirectUrl)
            };

            var param = parameters.ToDictionary(x => x.Key, x => x.Value);
            var finalAuthUrl = new Uri(QueryHelpers.AddQueryString(oAuthParams.AuthUrl, param)).AbsoluteUri;

            // finalAuthUrl will be opened in the browser.
            var searchEngine = _browserOptions.OpenBrowser(finalAuthUrl);

            // if the current logged in user is authenticated then it will redirect to mentioned redirect_url and webListener will return the context.
            var context = await webListener.AcceptAsync();

            var authorizationCode = HttpUtility.ParseQueryString(context.Request.QueryString)
                .Get(Constants.RESPONSE_TYPE_CODE);

            // Once the AuthorizationCode is received, it will be passed to the GetAccessToken method along with the codeVerifier to fetch the accessToken.
            var token = await _tokenRetriever.GetAccessToken(authorizationCode, pkce.CodeVerifier, oAuthParams);

            // After fetching the token for the current user this below method will close the browser.
            _browserOptions.CloseBrowser(searchEngine);

            return token;
        }

        private OAuthParams GetOAuthParams()
            => new OAuthParams
            {
                AuthUrl = ConfigurationManager.AppSettings[Constants.AUTH_URL],
                ClientId = ConfigurationManager.AppSettings[Constants.CLIENTID],
                RedirectUrl = ConfigurationManager.AppSettings[Constants.REDIRECT_URL],
                Scope = ConfigurationManager.AppSettings[Constants.SCOPE],
                TokenUrl = ConfigurationManager.AppSettings[Constants.TOKEN_URL]
            };
    }
}