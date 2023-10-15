using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Eassistance.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        //private readonly ICommandExecutor _commandExecutor;

        //public TelegramBotController(ICommandExecutor commandExecutor)
        //{
        //    _commandExecutor = commandExecutor;
        //}
        //BaseCommand _command;
        //public BotController(BaseCommand command)
        //{
        //    _command= command;
        //}
        [HttpPost]
        public async Task<IActionResult> Update(Update update)
        {
            // /start => register user
          
            //var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

            if (update.Message == null && update.CallbackQuery == null)
            {
                return Ok();
            }

            try
            {
                //await _commandExecutor.Execute(update);
                if (update.Message.Text == "/start")
                {
                    //await _command.ExecuteAsync(update);
                }
            }
            catch (Exception e)
            {
                return Ok();
            }

            return Ok();
        }
    }
}
