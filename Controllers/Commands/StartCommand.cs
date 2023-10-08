using Telegram.Bot;
using Telegram.Bot.Types;

namespace Eassistance.Controllers.Commands
{
    public class StartCommand: ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();

        public string Name => "/start";

        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            await Client.SendTextMessageAsync(chatId, "Привет!");
        }
    }
}
