using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IOperationService
    {
        Task<List<Operation>> GetAllOperations(Equipment equipment);
        Task<bool> CreateOperation(Operation operation);
        Task<Operation> GetOperationByName(string Name);
        Task<bool> DeleteOperation(Operation operation);
    }
}
