using Telegram.Bot.Types;
using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IUserService
    {
        Task<EAUser> GetOrCreate(Update update);
    }
}
