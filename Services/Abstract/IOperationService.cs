using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IOperationService
    {
        Task<List<Operation>> GetAllOperations(Equipment equipment);
        Task<bool> CreateOperation(Operation operation);
        Task<Operation> GetOperationById(Guid id);
        Task<bool> DeleteOperation(Operation operation);
    }
}
