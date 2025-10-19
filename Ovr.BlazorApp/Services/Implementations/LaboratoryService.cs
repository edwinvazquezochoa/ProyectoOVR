using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly IApiHelper _apiHelper;

        public LaboratoryService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Laboratory>>> GetLaboratoriesAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Laboratory>>("laboratory");
        }

        public async Task<ResponseBase<Laboratory>> CreateLaboratoryAsync(Laboratory laboratory)
        {
            return await _apiHelper.PostTypedAsync<Laboratory>("laboratory", laboratory);
        }

        public async Task<ResponseBase<Laboratory>> UpdateLaboratoryAsync(int laboratoryId, Laboratory laboratory)
        {
            return await _apiHelper.PutTypedAsync<Laboratory>($"laboratory/{laboratoryId}", laboratory);
        }

        public async Task<ResponseBase<bool>> DeleteLaboratoryAsync(int laboratoryId)
        {
            return await _apiHelper.DeleteTypedAsync($"laboratory/{laboratoryId}");
        }
    }
}
