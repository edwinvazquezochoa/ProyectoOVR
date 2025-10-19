using Ovr.DAO;
using Ovr.Domain.DTOs;
using Ovr.Domain.Responses;
using Ovr.DaoServices.Interfaces;

namespace Ovr.DaoServices.Implementations
{
    public class SalesNoteService : ISalesNoteService
    {
        public async Task<long> Add(SaleNoteDetailDto model, long userId)
        {
            return await SalesNoteDAO.Insert(model, userId);
        }

        public async Task<ResponseBase<SaleNoteDetailDto>> Update(SaleNoteDetailDto model, long userId)
        {
            return await SalesNoteDAO.Update(model, userId);
        }

        public async Task<SaleNoteDetailDto?> GetById(long saleNoteId)
        {
            return await SalesNoteDAO.GetById(saleNoteId);
        }
    }
}
