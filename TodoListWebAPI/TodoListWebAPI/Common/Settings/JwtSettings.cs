namespace TodoListWebAPI.Common.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string SignKey { get; set; }

        public int ExpireMinutes { get; set; }
    }
}
