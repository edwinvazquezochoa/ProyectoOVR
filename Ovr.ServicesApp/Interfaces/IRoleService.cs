using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseBase<Role>> Add(Role model);
        Task<ResponseBase<Role>> Update(Role model);  
        Task<bool> Delete(int id);
        Task<Role?> GetById(int id);
        Task<List<Role?>?> GetAll();
    }
}
