using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PARCIAL_CASTRO.Services
{
    public class ConversionService
    {
        private readonly HttpClient _httpClient;

        public ConversionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertUsdToBtc(decimal amountInUsd)
        {
            // Llamar a la API de CoinGecko para obtener el precio de BTC en USD
            var response = await _httpClient.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd");
            var data = JObject.Parse(response);

            // Extraer el precio de BTC en USD
            var priceInUsd = data["bitcoin"]["usd"].Value<decimal>();

            // Convertir el monto en USD a BTC
            return amountInUsd / priceInUsd;
        }

        public async Task<decimal> ConvertBtcToUsd(decimal amountInBtc)
        {
            var response = await _httpClient.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd");
            var data = JObject.Parse(response);

            // Extraer el precio de BTC en USD
            var priceInUsd = data["bitcoin"]["usd"].Value<decimal>();

            // Convertir el monto en BTC a USD
            return amountInBtc * priceInUsd;
        }

        public async Task<decimal> GetBtcToUsdRateAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd");
            var data = JObject.Parse(response); // Parsear la respuesta JSON
            var tasaBtc = (decimal)data["bitcoin"]["usd"]; // Obtener la tasa de BTC a USD
            return tasaBtc;
        }

        
   
    }
}