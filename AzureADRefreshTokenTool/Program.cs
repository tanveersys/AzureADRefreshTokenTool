
using Microsoft.Extensions.Configuration;

namespace AzureADRefreshTokenTool
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Build the configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            var configuration = configurationBuilder.Build();

            var azureADRefreshToken = new AzureADRefreshToken(configuration);

            try
            {
                var refreshTokenResponse = await azureADRefreshToken.GetRefreshTokenAsync();
                Console.WriteLine("Access Token:");
                Console.WriteLine(refreshTokenResponse.access_token);
                Console.WriteLine("Refresh Token:");
                Console.WriteLine(refreshTokenResponse.refresh_token);
                Console.WriteLine("Expires In (seconds):");
                Console.WriteLine(refreshTokenResponse.expires_in);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
