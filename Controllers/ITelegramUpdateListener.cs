using Telegram.Bot.Types;

namespace Eassistance.Controllers
{
    public interface ITelegramUpdateListener
    {
        public Task GetUpdate(Update update);
    }
}
