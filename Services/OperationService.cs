using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eassistance.Services
{
    public class OperationService : IOperationService
    {
        private readonly DataContext _context;

        public OperationService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateOperation(Operation operation)
        {
            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteOperation(Operation operation)
        {
            if (operation != null)
            {
                _context.Operations.Remove(operation);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Operation>> GetAllOperations(Equipment equipment)
        {
            return await _context.Operations
                .Where(x => x.EquipmentId == equipment.Id)
                .ToListAsync();
        }
        public async Task<Operation> GetOperationById(Guid id)
        {
            return await _context.Operations.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
