using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Eassistance.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        [HttpPost]
        public void Post(Update update) //Сюда будут приходить апдейты
        {
            Console.WriteLine(update.Message.Text);
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
