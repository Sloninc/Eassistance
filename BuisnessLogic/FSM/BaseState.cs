using Eassistance.Infrastructure;
using Telegram.Bot.Types;
using Eassistance.Services;
using Eassistance.Services.Abstract;
using Telegram.Bot.Types.ReplyMarkups;

namespace Eassistance.BuisnessLogic.FSM
{
    //базовый абстрактный класс состояния
    public abstract class BaseState
    {
        protected FSMContext _fsmcontext;
        protected readonly IUserService _userService;
        protected readonly TelegramBot _botClient;
        protected readonly IUnitService _unitService;
        protected readonly IOperationService _operationService;
        protected readonly IEquipmentService _equipmentService;
        protected readonly IStepService _stepService;
        public BaseState(IUserService userService, TelegramBot telegramBot, IUnitService unitService, IOperationService operationService, IEquipmentService equipmentService, IStepService stepService)
        {
            _userService = userService;
            _botClient = telegramBot; ;
            _unitService = unitService;
            _operationService = operationService;
            _equipmentService = equipmentService;
            _stepService = stepService;
        }
        //при смене состояния передаем ему контекст
        public void SetContext(FSMContext fsmcontext)
        {
            _fsmcontext = fsmcontext;
        }
        public abstract string Name { get; }
        // метод для обработки update текущим состоянием пользователя
        public abstract Task Handle(Update update);
        //для вывода меню трехкнопочного
        protected ReplyKeyboardMarkup GetTrheeKeyboard(string btn1, string btn2, string btn3)
        {
            KeyboardButton[][] keyboardButtons = new KeyboardButton[3][];
            keyboardButtons[0] = new KeyboardButton[1] { new KeyboardButton(btn1) };
            keyboardButtons[1] = new KeyboardButton[1] { new KeyboardButton(btn2) };
            keyboardButtons[2] = new KeyboardButton[1] { new KeyboardButton(btn3) };
            var inlineKeyboard = new ReplyKeyboardMarkup(keyboardButtons);
            inlineKeyboard.ResizeKeyboard = false;
            return inlineKeyboard;
        }
    }
}
