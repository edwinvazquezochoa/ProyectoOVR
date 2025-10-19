using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using Ovr.DaoServices.Interfaces;

namespace Ovr.DaoServices.Implementations
{
    public class RoleService: IRoleService
    {
        //public async Task<long> Add(Role model)
        //{
        //    return await RoleDAO.Insert(model);
        //}

        public async Task<ResponseBase<Role>> Add(Role model)
        {
            var (id, errorCode) = await RoleDAO.Insert(model);

            if (errorCode == "DUPLICATE")
            {
                return new ResponseBase<Role>
                {
                    Code = 409,
                    Message = "Ya existe un lente con ese nombre.",
                    Data = null
                };
            }
            else if (errorCode == "ERROR")
            {
                return new ResponseBase<Role>
                {
                    Code = 500,
                    Message = "Error inesperado al insertar.",
                    Data = null
                };
            }

            model.RoleId = (int)id;
            return new ResponseBase<Role>
            {
                Code = 200,
                Message = "Registro creado exitosamente.",
                Data = model
            };
        }

        public async Task<ResponseBase<Role>> Update(Role model)
        {
            return await RoleDAO.Update(model);
        }

        public async Task<bool> Delete(int id)
        {
            return await RoleDAO.Delete(id);
        }

        public async Task<Role?> GetById(int id)
        {
            return await RoleDAO.GetById(id);
        }

        public async Task<List<Role?>?> GetAll()
        {
            return await RoleDAO.GetAll();
        }
    }
}
