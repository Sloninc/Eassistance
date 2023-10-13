using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class BotController : ControllerBase
    {
        //private readonly ICommandExecutor _commandExecutor;

        //public TelegramBotController(ICommandExecutor commandExecutor)
        //{
        //    _commandExecutor = commandExecutor;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update([FromBody] object update)
        //{
        //    // /start => register user

        //    var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

        //    if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
        //    {
        //        return Ok();
        //    }

        //    try
        //    {
        //        await _commandExecutor.Execute(upd);
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok();
        //    }

        //    return Ok();
    }

}
