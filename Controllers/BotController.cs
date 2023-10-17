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
            if (update.Message == null && update.CallbackQuery == null)
            {
                return NotFound();
            }
            try
            {
                await _fsmContext.Request(update);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }
    }
}
