using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface IFrameService
    {
        Task<ResponseBase<Frame>> Add(Frame model);
        Task<ResponseBase<Frame>> Update(Frame model);
        Task<bool> Delete(int id);
        Task<Frame?> GetById(int id);
        Task<List<Frame?>?> GetAll();
    }
}
