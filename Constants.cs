namespace OktaOAuthPOC
{
    public class Constants
    {
        #region Params

        public const string CODE_CHALLENGE = "code_challenge";
        public const string CODE_CHALLENGE_METHOD = "code_challenge_method";
        public const string CODE_CHALLENGE_METHOD_TYPE_S256 = "S256";
        public const string CODE_VERIFIER = "code_verifier";
        public const string CLIENT_ID = "client_id";
        public const int DEFAULT_LENGTH_OF_STATE = 16;
        public const string GRANT_TYPE = "grant_type";
        public const string GRANT_TYPE_AUTHORIZATION_CODE = "authorization_code";
        public const string REDIRECT_URI = "redirect_uri";
        public const string RESPONSE_TYPE = "response_type";
        public const string RESPONSE_TYPE_CODE = "code";
        public const string SCOPE = "scope";
        public const string STATE = "state";

        #endregion Params

        #region Config-related

        public const string AUTH_URL = "authUrl";
        public const string CLIENTID = "clientId";
        public const string REDIRECT_URL = "redirectUrl";
        public const string TOKEN_URL = "tokenUrl";

        #endregion Config-related
    }
}