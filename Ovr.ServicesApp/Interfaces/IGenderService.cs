using Ovr.Domain.Models;

namespace Ovr.DaoServices.Interfaces
{
    public interface IGenderService
    {
        Task<long> Add(Gender model);
        Task<bool> Update(Gender model);
        Task<bool> Delete(int id);
        Task<Gender?> GetById(int id);
        Task<List<Gender?>?> GetAll();
    }
}
