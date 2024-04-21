using CurrencyTelegramBot.Enums;
using CurrencyTelegramBot.Interfaces;
using Newtonsoft.Json.Linq;

namespace CurrencyTelegramBot
{
    internal class HttpPrivatBankCurrency(HttpClient client) : IHttpCurrencyJson
    {
        private readonly HttpClient _client = client;

        private const string _privatBankAPI = @"https://api.privatbank.ua/p24api/exchange_rates?json&date="; // format date=01.12.2020 

        public async Task<decimal?> GetForeinceCurrencyToUAH(DateTime date, EСurrencyСode? code)
        {
            string dateLine = date.ToString("dd.MM.yyyy");
            string link = _privatBankAPI + dateLine;

            var response = await _client.GetStringAsync( link );
            var data = JObject.Parse(response);

            var saleRateNB = data.SelectToken($"$.exchangeRate[?(@.currency=='{СurrencyСode.GetCode(code)}')].saleRateNB");

            if (saleRateNB == null)
                return null;
            else
                return (decimal)saleRateNB;
        }
    }
}
