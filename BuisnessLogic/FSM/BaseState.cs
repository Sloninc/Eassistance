using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot;
using Eassistance.Services;
using Eassistance.Services.Abstract;

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
        public abstract Task Handle(Update update);
    }
}
