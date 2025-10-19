using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Intefaces
{
    public interface IDashBoardService
    {
        Task<ResponseDTO<DashBoardDTO>> Resumen();
    }
}
