using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Implementations
{
    public class LaboratoryService : ILaboratoryService
    {
        public async Task<ResponseBase<Laboratory>> Add(Laboratory model)
        {
            var (id, error) = await LaboratoriesDAO.Insert(model);

            if (error == "DUPLICATE")
                return new ResponseBase<Laboratory>(409, "Ya existe un laboratorio con ese nombre.", null);
            if (error == "ERROR")
                return new ResponseBase<Laboratory>(500, "Error al insertar laboratorio.", null);

            model.LaboratoryId = id;
            return new ResponseBase<Laboratory>(200, "Laboratorio agregado correctamente.", model);
        }

        public async Task<ResponseBase<Laboratory>> Update(Laboratory model)
        {
            return await LaboratoriesDAO.Update(model);
        }

        public async Task<bool> Delete(int laboratoryId)
        {
            // Si deseas agregar lógica de borrado lógico más adelante, aquí puedes hacerlo.
            return false;
        }

        public async Task<Laboratory?> GetById(int id)
        {
            return await LaboratoriesDAO.GetById(id);
        }

        public async Task<List<Laboratory>> GetAll()
        {
            return await LaboratoriesDAO.GetAll();
        }
    }
}