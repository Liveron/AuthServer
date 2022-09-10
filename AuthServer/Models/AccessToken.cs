namespace AuthServer.Models
{
    public class AccessToken
    {
        public Guid UserId { get; set; }
        public string Value { get; set; }
    }
}
