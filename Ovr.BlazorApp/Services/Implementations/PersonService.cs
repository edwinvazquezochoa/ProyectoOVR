using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;

namespace Ovr.BlazorApp.Services.Implementations
{
    public class PersonsServices : IPersonsServices
    {
        private readonly IApiHelper _apiHelper;

        public PersonsServices(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseBase<List<Person>>> GetPersonsAsync()
        {
            return await _apiHelper.GetTypedAsync<List<Person>>("person");
        }

        public async Task<ResponseBase<Person>> GetByIdAsync(long personId)
        {
            return await _apiHelper.GetTypedAsync<Person>($"person/{personId}");
        }

        public async Task<ResponseBase<Person>> CreatePersonAsync(Person person)
        {
            return await _apiHelper.PostTypedAsync<Person>("person", person);
        }

        public async Task<ResponseBase<Person>> UpdatePersonAsync(long personId, Person person)
        {
            return await _apiHelper.PutTypedAsync<Person>($"person/{personId}", person);
        }
    }
}
