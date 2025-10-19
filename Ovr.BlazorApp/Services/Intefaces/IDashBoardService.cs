using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IDashBoardService
    {
        Task<ResponseBase<DashBoardDTO>> Resumen();
    }
}
