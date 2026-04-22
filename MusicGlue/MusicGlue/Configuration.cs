using Microsoft.Extensions.Configuration;

namespace MusicGlue
{
    internal static class Configuration
    {
        private static readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        public static string ConnectionString { get; } = configuration.GetConnectionString("MyDBConnection") ?? "";
    }
}
