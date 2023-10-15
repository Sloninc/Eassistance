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
        private readonly DataContext _context;

        public EquipmentService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateEquipment(Equipment equipment)
        {
            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteEquipment(Equipment equipment)
        {
            if (equipment != null)
            {
                _context.Equipments.Remove(equipment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Equipment>> GetAllEquipments(Unit unit)
        {
            return await _context.Equipments
                .Where(x => x.UnitId == unit.Id)
                .ToListAsync();
        }
        public async Task<Equipment> GetEquipmentById(Guid id)
        {
            return await _context.Equipments.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
