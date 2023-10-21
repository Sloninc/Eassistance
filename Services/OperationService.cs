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
        protected readonly IDbContextFactory<DataContext> _contextFactory;

        public OperationService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> CreateOperation(Operation operation)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Operations.AddAsync(operation);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteOperation(Operation operation)
        {
            if (operation != null)
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    _context.Operations.Remove(operation);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            else
                return false;
        }
        public async Task<List<Operation>> GetAllOperations(Equipment equipment)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Operations
                .Where(x => x.EquipmentId == equipment.Id)
                .ToListAsync();
            }
        }
        public async Task<Operation> GetOperationByName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Operations.FirstOrDefaultAsync(x => x.Name == name);
            }
        }
    }
}
