using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class FrameClientService : IFrameClientService
    {
        private readonly IApiHelper _apiHelper;

        public FrameClientService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Frame>>> GetFramesAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Frame>>("frame");
        }

        public async Task<ResponseBase<Frame>> CreateFrameAsync(Frame frame)
        {
            return await _apiHelper.PostTypedAsync<Frame>("frame", frame);
        }

        public async Task<ResponseBase<Frame>> UpdateFrameAsync(int frameId, Frame frame)
        {
            return await _apiHelper.PutTypedAsync<Frame>($"frame/{frameId}", frame);
        }

        public async Task<ResponseBase<bool>> DeleteFrameAsync(int frameId)
        {
            return await _apiHelper.DeleteTypedAsync($"frame/{frameId}");
        }
    }
}
