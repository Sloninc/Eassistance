using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Eassistance.Services.Abstract;
using Eassistance.Infrastructure;
using Eassistance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eassistance.Services
{
    public class StepService:IStepServise
    {
        private readonly DataContext _context;

        public StepService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateStep(Step step)
        {
            await _context.Steps.AddAsync(step);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteStep(Step step)
        {
            if (step != null)
            {
                _context.Steps.Remove(step);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<Step>> GetAllSteps(Operation operation)
        {
            return await _context.Steps
                .Where(x => x.OperationId == operation.Id)
                .ToListAsync();
        }
        public async Task<Step> GetStepById(Guid id)
        {
            return await _context.Steps.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
