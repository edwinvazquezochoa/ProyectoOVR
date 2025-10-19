using Ovr.ClientServices.Intefaces;
using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Implementations
{
    public class BranchService: IBranchService
    {
        public async Task<ResponseBase<Branch>> Create(Branch model)
        {
            return await BranchDAO.Create(model);
        }

        public async Task<ResponseBase<Branch>> Update(Branch model)
        {
            return await BranchDAO.Update(model);
        }

        public async Task<bool> Delete(long id)
        {
            return await BranchDAO.Delete(id);
        }

        public async Task<Branch?> GetById(int id)
        {
            return await BranchDAO.GetById(id);
        }

        public async Task<List<Branch?>> GetAll()
        {
            return await BranchDAO.GetAll();
        }

    }
}
