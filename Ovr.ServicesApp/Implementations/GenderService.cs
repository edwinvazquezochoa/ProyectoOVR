using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.DaoServices.Interfaces;

namespace Ovr.DaoServices.Implementations
{
    public class GenderService : IGenderService
    {
        public async Task<long> Add(Gender model)
        {
            return await GenderDAO.Insert(model);
        }

        public async Task<bool> Update(Gender model)
        {
            return await GenderDAO.Update(model);
        }

        public async Task<bool> Delete(int id)
        {
            return await GenderDAO.Delete(id);
        }

        public async Task<Gender?> GetById(int id)
        {
            return await GenderDAO.GetById(id);
        }

        public async Task<List<Gender?>?> GetAll()
        {
            return await GenderDAO.GetAll();
        }
    }
}
