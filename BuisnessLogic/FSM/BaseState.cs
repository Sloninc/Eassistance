using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot.Types.ReplyMarkups;

namespace Eassistance.BuisnessLogic.FSM
{
    public abstract class BaseState
    {
        protected readonly IDbContextFactory<DataContext> _contextFactory;
        protected FSMContext _fsmcontext;
        protected readonly IUserService _userService;
        protected readonly TelegramBot _botClient;
        protected readonly IUnitService _unitService;
        protected readonly IOperationService _operationService;
        protected readonly IEquipmentService _equipmentService;
        protected readonly IStepService _stepService;
        public BaseState(IDbContextFactory<DataContext> contextFactory, IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService)
        {
            _contextFactory = contextFactory;
            _userService = userService;
            _botClient = telegramBot; ;
            _unitService = unitService;
            _operationService = operationService;
            _equipmentService = equipmentService;
            _stepService = stepService;
        }
        public void SetContext(FSMContext fsmcontext)
        {
            _fsmcontext = fsmcontext;
        }
        public abstract string Name { get; }
        public FSMContext fSM { get; }
        public abstract Task Handle(Update update);

        protected ReplyKeyboardMarkup GetTrheeKeyboard(string btn1, string btn2, string btn3)
        {
            KeyboardButton[][] keyboardButtons = new KeyboardButton[3][];
            keyboardButtons[0] = new KeyboardButton[1] { new KeyboardButton(btn1) };
            keyboardButtons[1] = new KeyboardButton[1] { new KeyboardButton(btn2) };
            keyboardButtons[2] = new KeyboardButton[1] { new KeyboardButton(btn3) };
            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
            inlineKeyboard.ResizeKeyboard = true;
            return inlineKeyboard;
        }
    }
}
