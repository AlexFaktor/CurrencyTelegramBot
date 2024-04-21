using CurrencyTelegramBot;
using CurrencyTelegramBot.Enums;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    static readonly ITelegramBotClient botClient = new TelegramBotClient("6745328129:AAFoVIuIV1V8r7N5PpoW-yfoiTBr4b6CTqA");
    static readonly HttpPrivatBankCurrency httpPrivatBankCurrency = new(new HttpClient());

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        var botName = GetBot(botClient).Result;
        Console.WriteLine($"Bot username: {botName.Username}");

        // Get the latest update
        var updates = botClient.GetUpdatesAsync().Result;
        var lastUpdateId = updates.Length != 0 ? updates.Select(u => u.Id).Max() : 0;

        // Start receiving from the next update
        botClient.StartReceiving(new DefaultUpdateHandler(Update, Error),
            new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                Offset = lastUpdateId + 1
            });
        Console.ReadKey();
    }

    private static async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var message = update.Message;
        Console.WriteLine($"{message?.From?.Username} | {message?.Text}");

        if (message?.Text != null)
        {
            var text = message.Text.Trim().Split(' ');
            using CancellationTokenSource cts = new();

            if (message.Text[0] == '/')
            {
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Привіт, тут ти можеш отримати курс валюти залежно від дати.\n" +
                          $"Формат який має бути:\n" +
                          $"<Код валюти> <Дата в форматі \"dd.MM.yyyy\">\n" +
                          $"Приклад: USD 01.01.2024",
                    cancellationToken: cts.Token
                    );
                }

                if (message.Text == "/currency_list")
                {
                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Валюти які обслуговуються:\n" +
                          $"UAH - Ukrainian hryvnia\n" +
                          $"USD - US dollar\n" +
                          $"EUR - Euro\n" +
                          $"CHF - Swiss franc\n" +
                          $"GBP - British pound\n" +
                          $"PLZ - Polish zloty\n" +
                          $"SEK - Swedish krona\n" +
                          $"XAU - Gold\n" +
                          $"CAD - Canadian dollar",
                    cancellationToken: cts.Token
                    );
                }
            }
            else if (text.Length == 2)
            {
                string currency = text[0].ToUpper();
                string dateText = text[1];
                var isCorrectDate = DateTime.TryParseExact(dateText, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                if (СurrencyСode.GetCode(currency) == null)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Невідома валюта - {currency}",
                        cancellationToken: cts.Token);
                    return;
                }
                if (!isCorrectDate)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Невірний формат дати - {dateText}",
                        cancellationToken: cts.Token);
                    return;
                }

                var response = httpPrivatBankCurrency.GetForeinceCurrencyToUAH(date, СurrencyСode.GetCode(currency));
                if (response.Result == null)
                {
                    await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"По заданій інформації немає результатів",
                            cancellationToken: cts.Token);
                    await botClient.SendStickerAsync(message.Chat.Id, InputFile.FromString(@"CAACAgIAAxkBAAEE4C9mJNEjxWH55WhqhE8rXa_2aY-jXAAC-QADVp29CpVlbqsqKxs2NAQ"));
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: $"Валюта: {currency}\n" +
                                                $"Час: {dateText}\n" +
                                                $"Курс продажу НБУ: {response!.Result}",
                                                cancellationToken: cts.Token);
                    cts.Cancel();
                }
                
            }
            else
            {

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Не зрозумів. \nФормат який має бути:\n" +
                          $"<Код валюти> <Дата в форматі \"dd.MM.yyyy\">\n" +
                          $"Приклад: USD 01.01.2024",
                    cancellationToken: cts.Token
                    );

                cts.Cancel();
            }
        }
    }

    private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        try
        {
            // Handle the error
            Console.WriteLine($"An error occurred: {exception.Message}");
        }
        catch (Exception ex)
        {
            // Log the error handling exception
            Console.WriteLine($"An error occurred while handling an error: {ex.Message}");
        }
    }

    private static async Task<User> GetBot(ITelegramBotClient client)
    {
        return await client.GetMeAsync();
    }
}
