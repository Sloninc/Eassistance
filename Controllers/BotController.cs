using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        private TelegramBotClient bot = Bot.GetTelegramBot();
        [HttpPost]
        public async Task Post(Update update) //Сюда будут приходить апдейты
        {
            long chatId = update.Message.Chat.Id; //получаем айди чата, куда нам сказать привет
            await bot.SendTextMessageAsync(chatId, "Привет!");
        }
        [HttpGet]
        public string Get()
        {
            //Здесь мы пишем, что будет видно если зайти на адрес,
            //указаную в ngrok и launchSettings
            return "Telegram bot was started";
        }
    }
}
