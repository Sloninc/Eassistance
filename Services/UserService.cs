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
            //var newUser = update.Type switch
            //{
            //    UpdateType.CallbackQuery => new EAUser
            //    {
            //        Username = update.CallbackQuery.From.Username,
            //        ChatId = update.CallbackQuery.Message.Chat.Id,
            //        FirstName = update.CallbackQuery.Message.From.FirstName,
            //        LastName = update.CallbackQuery.Message.From.LastName
            //    },
            //    UpdateType.Message => new EAUser
            //    {
            //        Username = update.Message.Chat.Username,
            //        ChatId = update.Message.Chat.Id,
            //        FirstName = update.Message.Chat.FirstName,
            //        LastName = update.Message.Chat.LastName
            //    }
            //};
            //EAUser user = null;
            //using (var _context = _contextFactory.CreateDbContext())
            //{
            //    user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);
            //    if (user != null) return user;
            //    var result = await _context.Users.AddAsync(newUser);
            //    await _context.SaveChangesAsync();
            //    return result.Entity;
            //}
        }
    }
}
