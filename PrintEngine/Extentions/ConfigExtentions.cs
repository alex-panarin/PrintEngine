namespace PrintEngine.Extentions
{
    internal static class ConfigExtentions
    {
        private static readonly string[] _environments = new[]
            {
                "Development",
                "Test",
                "Stage"
            };
        internal static bool IsTestEnvironment(this IConfiguration configuration)
        {
            var environment = configuration["Logger:EnvironmentName"];
            if (environment == null) return true;

            return _environments
                .Any(e => e.Equals(environment, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
