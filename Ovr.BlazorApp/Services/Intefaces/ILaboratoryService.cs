using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface ILaboratoryService
    {
        Task<ResponseBase<List<Laboratory>>> GetLaboratoriesAsync();
        Task<ResponseBase<Laboratory>> CreateLaboratoryAsync(Laboratory laboratory);
        Task<ResponseBase<Laboratory>> UpdateLaboratoryAsync(int laboratoryId, Laboratory laboratory);
        Task<ResponseBase<bool>> DeleteLaboratoryAsync(int laboratoryId);
    }
}
