using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IApiHelper _apiHelper;

        public BranchService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Branch>>> GetAllAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Branch>>("branch");
        }

        public async Task<ResponseBase<object>> CreateAsync(Branch model)
        {
            return await _apiHelper.PostTypedAsync<object>("branch", model);
        }

        public async Task<ResponseBase<object>> UpdateAsync(long id, Branch model)
        {
            return await _apiHelper.PutTypedAsync<object>($"branch/{id}", model);
        }

        public async Task<ResponseBase<bool>> DeleteAsync(long id)
        {
            return await _apiHelper.DeleteTypedAsync($"branch/{id}");
        }
    }
}
