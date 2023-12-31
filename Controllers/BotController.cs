﻿using Microsoft.AspNetCore.Mvc;
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
        public BotController(BaseState state, FSMContext fsmContext)
        {
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
                var context = FSMContextStorage.Get(update.Message.Chat.Id);// вынимаем контекст машины состояний из класса хранения контекстов пользователей
                if(context == null)
                    context = _fsmContext;// или получаем новый контекст
                await context.Request(update);// обработка update текущим состоянием пользователя
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }
    }
}
