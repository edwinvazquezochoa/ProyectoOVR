using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Implementations
{
    public class LensesService: ILensesService
    {
        public async Task<ResponseBase<Len>> Add(Len model)
        {
            var (id, errorCode) = await LensesDAO.Insert(model);

            if (errorCode == "DUPLICATE")
            {
                return new ResponseBase<Len>
                {
                    Code = 409,
                    Message = "Ya existe un lente con ese nombre.",
                    Data = null
                };
            }
            else if (errorCode == "ERROR")
            {
                return new ResponseBase<Len>
                {
                    Code = 500,
                    Message = "Error inesperado al insertar el lente.",
                    Data = null
                };
            }

            model.LensId = (int)id;
            return new ResponseBase<Len>
            {
                Code = 200,
                Message = "Lente creado exitosamente.",
                Data = model
            };
        }


        public async Task<ResponseBase<Len>> Update(Len model)
        {
            return await LensesDAO.Update(model);
        }

        public async Task<bool> Delete(int id)
        {
            return await LensesDAO.Delete(id);
        }

        public async Task<Len?> GetById(int id)
        {
            return await LensesDAO.GetById(id);
        }

        public async Task<List<Len?>?> GetAll()
        {
            return await LensesDAO.GetAll();
        }
    }
}
