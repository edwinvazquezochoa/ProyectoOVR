using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface ILaboratoryService
    {
        Task<ResponseBase<Laboratory>> Add(Laboratory laboratory);
        Task<ResponseBase<Laboratory>> Update(Laboratory laboratory);
        Task<bool> Delete(int laboratoryId);
        Task<Laboratory?> GetById(int laboratoryId);
        Task<List<Laboratory>> GetAll();
    }
}
