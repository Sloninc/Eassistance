using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class OperationChoiceState:BaseState
    {
        public OperationChoiceState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService) : base(contextFactory, userService, telegramBot, unitService, operationService, equipmentService, stepService)
        {
            Name = "EqipmentChoice";
        }
        public override string Name { get; }

        public async override Task Handle(Update update)
        {
            try
            {
                switch (update.Message.Text)
                {
                    case "/start":
                        _fsmcontext.TransitionTo(new StartState(_contextFactory, _userService, _botClient, _unitService, _operationService, _equipmentService, _stepService));
                        FSMContextStorage.Set(update.Message.Chat.Id, _fsmcontext);
                        _fsmcontext.Request(update);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
