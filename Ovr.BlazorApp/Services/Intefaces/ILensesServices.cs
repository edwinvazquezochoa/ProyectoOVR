using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface ILensesServices
    {
        Task<ResponseBase<List<Len>>> GetLensesAsync();
        Task<ResponseBase<Len>> CreateLensAsync(Len model);
        Task<ResponseBase<Len>> UpdateLensAsync(int id, Len model);
        Task<ResponseBase<bool>> DeleteLensAsync(int id);
    }
}
