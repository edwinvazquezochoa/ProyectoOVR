using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class LensesServices : ILensesServices
    {
        private readonly IApiHelper _apiHelper;

        public LensesServices(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Len>>> GetLensesAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Len>>("lenses");
        }

        public async Task<ResponseBase<Len>> CreateLensAsync(Len lens)
        {
            return await _apiHelper.PostTypedAsync<Len>("lenses", lens);
        }

        public async Task<ResponseBase<Len>> UpdateLensAsync(int lensId, Len lens)
        {
            return await _apiHelper.PutTypedAsync<Len>($"lenses/{lensId}", lens);
        }

        public async Task<ResponseBase<bool>> DeleteLensAsync(int lensId)
        {
            return await _apiHelper.DeleteTypedAsync($"lenses/{lensId}");
        }
    }
}
