using Ovr.DAO;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Implementations
{
    public class FrameService : IFrameService
    {
        public async Task<ResponseBase<Frame>> Add(Frame model)
        {
            var (id, errorCode) = await FramesDAO.Insert(model);

            if (errorCode == "DUPLICATE")
            {
                return new ResponseBase<Frame>
                {
                    Code = 409,
                    Message = "Ya existe un armazón con ese nombre.",
                    Data = null
                };
            }
            else if (errorCode == "ERROR")
            {
                return new ResponseBase<Frame>
                {
                    Code = 500,
                    Message = "Error inesperado al insertar el frame.",
                    Data = null
                };
            }

            model.FrameId = (int)id;
            return new ResponseBase<Frame>
            {
                Code = 200,
                Message = "Frame creado exitosamente.",
                Data = model
            };
        }


        public async Task<ResponseBase<Frame>> Update(Frame model)
        {
            return await FramesDAO.Update(model);
        }

        public async Task<bool> Delete(int id)
        {
            return await FramesDAO.Delete(id);
        }

        public async Task<Frame?> GetById(int id)
        {
            return await FramesDAO.GetById(id);
        }

        public async Task<List<Frame?>?> GetAll()
        {
            return await FramesDAO.GetAll();
        }
    }
}
