using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace AzureADRefreshTokenTool
{
    class AzureADRefreshToken
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;

        public AzureADRefreshToken(IConfiguration configuration)
        {
            _tenantId = configuration["AzureAd:TenantId"];
            _clientId = configuration["AzureAd:ClientId"];
            _clientSecret = configuration["AzureAd:ClientSecret"];
            _tokenEndpoint = configuration["AzureAd:TokenEndpoint"];
        }

        public async Task<RefreshTokenResponse> GetRefreshTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var requestContent = new StringContent(
                    $"grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}&scope=https://graph.microsoft.com/.default",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(_tokenEndpoint, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RefreshTokenResponse>(responseContent);
                }
                else
                {
                    throw new Exception($"Failed to retrieve refresh token: {response.StatusCode}");
                }
            }
        }
    }
}
