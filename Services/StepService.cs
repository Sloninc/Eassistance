using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eassistance.Services
{
    public class StepService:IStepService
    {
        protected readonly IDbContextFactory<DataContext> _contextFactory;

        public StepService(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> CreateStep(Step step)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.Steps.AddAsync(step);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteStep(Step step)
        {
            if (step != null)
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    _context.Steps.Remove(step);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            else
                return false;
        }
        public async Task<List<Step>> GetAllSteps(Operation operation)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Steps
                .Where(x => x.OperationId == operation.Id)
                .ToListAsync();
            }
        }
        public async Task<Step> GetStepByName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Steps.FirstOrDefaultAsync(x => x.Name == name);
            }
        }
    }
}
