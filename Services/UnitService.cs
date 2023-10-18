using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eassistance.Services
{
    public class UnitService:IUnitService
    {
        protected readonly IDbContextFactory<DataContext> _contextFactory;

        public UnitService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> CreateUnit(Unit unit)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Units.AddAsync(unit);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteUnit(Unit unit)
        {
            if (unit != null)
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    _context.Units.Remove(unit);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        public async Task<List<Unit>> GetAllUnits()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Units.ToListAsync();
            }
        }
        public async Task<Unit> GetUnitByName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Units.FirstOrDefaultAsync(x => x.Name == name);
            }
        }
    }
}
