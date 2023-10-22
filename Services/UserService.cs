using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;


namespace Eassistance.Services
{
    public class UserService : IUserService
    {
        protected readonly IDbContextFactory<DataContext> _contextFactory;

        public UserService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<EAUser> GetUserByChatId(long chatId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.ChatId == chatId);
            }
        }
        public async Task<bool> CreateUser(EAUser user)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
