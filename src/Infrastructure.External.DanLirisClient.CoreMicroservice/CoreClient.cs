using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.CoreMicroservice
{
    public class CoreClient : ICoreClient
    {
        private readonly HttpClient _http;
        private readonly MasterDataSettings _settings;

        public CoreClient(HttpClient http, MasterDataSettings settings)
        {
            _http = http;
            _settings = settings;
        }

        public Task<dynamic> RetrieveUnitDepartments()
        {
            throw new System.NotImplementedException();
        }

        protected async Task<string> GetTokenAsync()
        {
            var response = _http.PostAsync(MasterDataSettings.TokenEndpoint,
                new StringContent(JsonConvert.SerializeObject(new { username = MasterDataSettings.Username, password = MasterDataSettings.Password }), Encoding.UTF8, "application/json"));

            dynamic tokenResult = new { };

            if (response.IsCompletedSuccessfully)
            {
                tokenResult = JsonConvert.DeserializeObject<dynamic>(await response.Result.Content.ReadAsStringAsync());
            }

            return tokenResult.accessToken;
        }
    }
}