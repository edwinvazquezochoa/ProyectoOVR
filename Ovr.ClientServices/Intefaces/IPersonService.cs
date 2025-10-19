using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.ClientServices.Intefaces
{
    public interface IPersonService
    {
        Task<ResponseBase<List<Person>>> GetAllAsync();
        Task<ResponseBase<Person>> CreateAsync(Person model);
        Task<ResponseBase<Person>> UpdateAsync(int id, Person model);
        Task<ResponseBase<bool>> DeleteAsync(int id);
    }
}
