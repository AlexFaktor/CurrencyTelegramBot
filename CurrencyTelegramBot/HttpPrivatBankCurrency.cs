using CurrencyTelegramBot.Enums;
using CurrencyTelegramBot.Interfaces;
using Newtonsoft.Json.Linq;

namespace CurrencyTelegramBot
{
    internal class HttpPrivatBankCurrency(HttpClient client) : IHttpCurrencyJson
    {
        private readonly HttpClient _client = client;

        public async Task<decimal?> GetForeinceCurrencyToUAH(DateTime date, EСurrencyСode? code)
        {
            var response = await _client.GetStringAsync(@"https://api.privatbank.ua/p24api/exchange_rates?json&date=" + $"{date:dd.MM.yyyy}");
            var data = JObject.Parse(response);

            var saleRateNB = data.SelectToken($"$.exchangeRate[?(@.currency=='{СurrencyСode.GetCode(code)}')].saleRateNB");

            if (saleRateNB == null)
                return null;
            else
                return (decimal)saleRateNB;
        }
    }
}
