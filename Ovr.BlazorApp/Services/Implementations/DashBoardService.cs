using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IApiHelper _apiHelper;

        public DashBoardService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<DashBoardDTO>> Resumen()
        {
            return await _apiHelper.GetAsync<ResponseBase<DashBoardDTO>>("dashboard/Resumen")
                   ?? new ResponseBase<DashBoardDTO> { Code = 500, Message = "Error al obtener el resumen" };
        }
    }
}
