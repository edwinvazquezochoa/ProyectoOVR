using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.DaoServices.Interfaces
{
    public interface IPersonService
    {
        Task<ResponseBase<Person>> Add(Person person);
        Task<ResponseBase<Person>> Update(Person person);
        Task<Person?> GetById(long personId);
        Task<List<Person>> GetAll();
    }
}
