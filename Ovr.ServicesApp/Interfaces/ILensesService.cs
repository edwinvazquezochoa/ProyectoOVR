using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface ILensesService
    {
        Task<ResponseBase<Len>> Add(Len model);
        Task<ResponseBase<Len>> Update(Len model);
        Task<bool> Delete(int id);
        Task<Len?> GetById(int id);
        Task<List<Len?>?> GetAll();
    }
}
