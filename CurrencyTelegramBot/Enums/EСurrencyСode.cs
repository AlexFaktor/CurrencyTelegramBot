namespace CurrencyTelegramBot.Enums
{
    public enum EСurrencyСode
    {
        UAH, // Ukrainian hryvnia
        USD, // US dollar
        EUR, // Euro
        CHF, // Swiss franc
        GBP, // British pound
        PLZ, // Polish zloty
        SEK, // Swedish krona
        XAU, // Gold
        CAD, // Canadian dollar
    }

    public class СurrencyСode
    {
        public static string GetCodeDescription()
        {
            return "UAH - Ukrainian hryvnia\n" +
                  "USD - US dollar\n" +
                  "EUR - Euro\n" +
                  "CHF - Swiss franc\n" +
                  "GBP - British pound\n" +
                  "PLZ - Polish zloty\n" +
                  "SEK - Swedish krona\n" +
                  "XAU - Gold\n" +
                  "CAD - Canadian dollar";
        }

        public static EСurrencyСode? GetCode(string code)
        {
            return code switch
            {
                "UAH" => EСurrencyСode.UAH,
                "USD" => EСurrencyСode.USD,
                "EUR" => EСurrencyСode.EUR,
                "CHF" => EСurrencyСode.CHF,
                "GBP" => EСurrencyСode.GBP,
                "PLZ" => EСurrencyСode.PLZ,
                "SEK" => EСurrencyСode.SEK,
                "XAU" => EСurrencyСode.XAU,
                "CAD" => EСurrencyСode.CAD,
                _ => null,
            };
        }

        public static string? GetCode(EСurrencyСode? code)
        {
            return code switch
            {
                EСurrencyСode.UAH => "UAH",
                EСurrencyСode.USD => "USD",
                EСurrencyСode.EUR => "EUR",
                EСurrencyСode.CHF => "CHF",
                EСurrencyСode.GBP => "GBP",
                EСurrencyСode.PLZ => "PLZ",
                EСurrencyСode.SEK => "SEK",
                EСurrencyСode.XAU => "XAU",
                EСurrencyСode.CAD => "CAD",
                _ => null,
            };
        }
    }
}
