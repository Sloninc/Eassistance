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
        private readonly DataContext _context;

        public UnitService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateUnit(Unit unit)
        {
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUnit(Unit unit)
        {
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Unit>> GetAllUnits()
        {
            return await _context.Units.ToListAsync();
        }
        public async Task<Unit> GetUnitById(Guid id)
        {
            return await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
