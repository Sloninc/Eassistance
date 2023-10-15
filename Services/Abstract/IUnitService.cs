using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IUnitService
    {
        Task<List<Unit>> GetAllUnits();
        Task<bool> DeleteUnit(Unit unit);
        Task<Unit> GetUnitById(Guid id);
        Task<bool> CreateUnit(Unit unit);
    }
}