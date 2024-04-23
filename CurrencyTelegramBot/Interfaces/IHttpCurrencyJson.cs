using CurrencyTelegramBot.Enums;

namespace CurrencyTelegramBot.Interfaces
{
    internal interface IHttpCurrencyJson
    {
        public Task<decimal?> GetForeinceCurrencyToUAH(DateTime date, EСurrencyСode? сode);
    }
}
