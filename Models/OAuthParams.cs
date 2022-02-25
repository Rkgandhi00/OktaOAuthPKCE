namespace OktaOAuthPOC.Models
{
    public class OAuthParams
    {
        public string AuthUrl { get; set; }
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
        public string Scope { get; set; }
        public string TokenUrl { get; set; }
    }
}