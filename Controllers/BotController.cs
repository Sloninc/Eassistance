using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Eassistance.BuisnessLogic.FSM;

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
        FSMContext _fsmContext;
        BaseState _state;
        public BotController(BaseState state, FSMContext fsmContext)
        {
            _state = state;
            _fsmContext = fsmContext;
        }
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
                await _fsmContext.Request(update);
            }
            catch (Exception e)
            {
                return Ok();
            }

            return Ok();
        }
    }
}
