using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IStepServise
    {
        Task<List<Step>> GetAllSteps(Operation operation);
        Task<bool> CreateStep(Step step);
        Task<Step> GetStepById(Guid id);
        Task<bool> DeleteStep(Step step);
    }
}