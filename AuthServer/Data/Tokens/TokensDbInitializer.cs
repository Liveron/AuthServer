namespace AuthServer.Data.Tokens
{
    public class TokensDbInitializer
    {
        public static void Initialize(TokensDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
