namespace api.Authentication.Helper
{
    public class TokenGenerationRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Dictionary<string, object> CustomClaims { get; set; }
    }
}
