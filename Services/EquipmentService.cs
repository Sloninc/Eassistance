using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eassistance.Services
{
    public class EquipmentService:IEquipmentService
    {
        protected readonly IDbContextFactory<DataContext> _contextFactory;

        public EquipmentService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> CreateEquipment(Equipment equipment)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Equipments.AddAsync(equipment);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteEquipment(Equipment equipment)
        {
            if (equipment != null)
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    _context.Equipments.Remove(equipment);
                    await _context.SaveChangesAsync();
                }
            }
            return false;
        }
        public async Task<List<Equipment>> GetAllEquipments(Unit unit)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Equipments
                .Where(x => x.UnitId == unit.Id)
                .ToListAsync();
            }
        }
        public async Task<Equipment> GetEquipmentByName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Equipments.FirstOrDefaultAsync(x => x.Name == name);
            }
        }
    }
}
