using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Intefaces
{
    public interface IBranchService
    {
        Task<ResponseBase<List<Branch>>> GetAllAsync();
        Task<ResponseBase<object>> CreateAsync(Branch model);
        Task<ResponseBase<object>> UpdateAsync(long id, Branch model);
        Task<ResponseBase<bool>> DeleteAsync(long id);
    }
}
