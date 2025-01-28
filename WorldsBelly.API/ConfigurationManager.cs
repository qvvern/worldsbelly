namespace WorldsBelly.API
{
    using Microsoft.Extensions.Configuration;

    internal static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; set; }
    }
}