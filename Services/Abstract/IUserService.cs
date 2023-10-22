using Telegram.Bot.Types;
using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IUserService
    {
        Task<EAUser> GetUserByChatId(long chatId);
        Task<bool> CreateUser(EAUser user);

    }
}
