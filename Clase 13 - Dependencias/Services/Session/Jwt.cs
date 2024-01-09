namespace Logic.Session
{
    public class Jwt
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public TimeSpan ValidityTime { get; set; } = TimeSpan.FromSeconds(300);
    }
}
