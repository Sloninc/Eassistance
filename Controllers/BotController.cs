using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        //private TelegramBotClient bot = Bot.GetTelegramBot();
        private UpdateDistributor<CommandExecutor> updateDistributor = new UpdateDistributor<CommandExecutor>();

        [HttpPost]
        public async void Post(Update update)
        {
            if (update.Message == null) 
                return;
            await updateDistributor.GetUpdate(update);
        }
        [HttpGet]
        public string Get()
        {
            return "Telegram bot was started";
        }
    }
}
