using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.Controllers.Commands
{
    public interface ICommand
    {
        public TelegramBotClient Client { get; }
        public string Name { get; }
        public async Task Execute(Update update) { }
    }
}
