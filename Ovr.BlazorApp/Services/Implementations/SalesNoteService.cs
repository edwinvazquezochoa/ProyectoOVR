using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Interfaces;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class SalesNoteService : ISalesNoteService
    {
        private readonly IApiHelper _apiHelper;

        public SalesNoteService(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<SaleNoteDetailDto>> GetSaleNoteByIdAsync(long saleNoteId)
        {
            return await _apiHelper.GetTypedAsync<SaleNoteDetailDto>($"/SalesNote/{saleNoteId}");
        }

        public async Task<ResponseBase<long>> CreateSaleNoteAsync(SaleNoteDetailDto saleNote)
        {
            return await _apiHelper.PostTypedAsync<long>("/SalesNote", saleNote);
        }

        public async Task<ResponseBase<SaleNoteDetailDto>> UpdateSaleNoteAsync(long saleNoteId, SaleNoteDetailDto saleNote)
        {
            return await _apiHelper.PutTypedAsync<SaleNoteDetailDto>($"/SalesNote/{saleNoteId}", saleNote);
        }
    }
}
