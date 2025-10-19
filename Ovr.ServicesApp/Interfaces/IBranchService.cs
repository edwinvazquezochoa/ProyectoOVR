using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Intefaces
{
    public interface IBranchService
    {
        Task<ResponseBase<Branch>> Create(Branch model);
        Task<ResponseBase<Branch>> Update(Branch model);
        Task<bool> Delete(long id);
        Task<List<Branch?>> GetAll();
        Task<Branch?> GetById(int id);
    }
}
