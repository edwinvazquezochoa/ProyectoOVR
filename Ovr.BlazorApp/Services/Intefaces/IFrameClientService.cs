using System.Collections.Generic;
using System.Threading.Tasks;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IFrameClientService
    {
        Task<ResponseBase<List<Frame>>> GetFramesAsync();
        Task<ResponseBase<Frame>> CreateFrameAsync(Frame frame);
        Task<ResponseBase<Frame>> UpdateFrameAsync(int frameId, Frame frame);
        Task<ResponseBase<bool>> DeleteFrameAsync(int frameId);
    }
}
