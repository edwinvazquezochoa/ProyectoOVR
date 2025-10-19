using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IPersonsServices
    {
        Task<ResponseBase<List<Person>>> GetPersonsAsync();
        Task<ResponseBase<Person>> CreatePersonAsync(Person person);
        Task<ResponseBase<Person>> UpdatePersonAsync(long personId, Person person);
        Task<ResponseBase<Person>> GetByIdAsync(long personId);
    }
}
