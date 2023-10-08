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
        private static UpdateDistributor<CommandExecutor> updateDistributor { get; set; }
        public static UpdateDistributor<CommandExecutor> GetUpdateDistributor()
        {
            if (updateDistributor != null)
            {
                return updateDistributor;
            }
            updateDistributor = new UpdateDistributor<CommandExecutor>();
            return updateDistributor;
        }

        [HttpPost]
        public async void Post(Update update)
        {
            if (update.Message == null) 
                return;
            await GetUpdateDistributor().GetUpdate(update);
        }
        [HttpGet]
        public string Get()
        {
            return "Telegram bot was started";
        }
    }
}
