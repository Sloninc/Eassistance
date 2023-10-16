using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Eassistance.BuisnessLogic.FSM
{
    public class FSMContextGen
    {
        //public FSMContextGen()
        //{

        //}
        //private readonly IConfiguration _configuration;
        //private TelegramBotClient _botClient;

        //public TelegramBot(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public async Task<TelegramBotClient> GetBot()
        //{
        //    if (_botClient != null)
        //    {
        //        return _botClient;
        //    }

        //    _botClient = new TelegramBotClient(_configuration["Token"]);

        //    var hook = $"{_configuration["Url"]}";
        //    await _botClient.SetWebhookAsync(hook);

        //    return _botClient;
        //}
    }
}
