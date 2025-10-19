using Ovr.DAO;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using Ovr.DaoServices.Interfaces;

namespace Ovr.DaoServices.Implementations
{
    public class PersonService : IPersonService
    {
        public async Task<ResponseBase<Person>> Add(Person person)
        {
            var (id, errorCode) = await PersonsDAO.Insert(person);

            if (errorCode == "DUPLICATE")
            {
                return new ResponseBase<Person>
                {
                    Code = 409,
                    Message = "Ya existe una persona con esos datos.",
                    Data = null
                };
            }

            if (errorCode == "ERROR")
            {
                return new ResponseBase<Person>
                {
                    Code = 500,
                    Message = "Error inesperado al insertar la persona.",
                    Data = null
                };
            }

            person.PersonId = id;

            return new ResponseBase<Person>
            {
                Code = 200,
                Message = "Persona registrada correctamente.",
                Data = person
            };
        }

        public async Task<ResponseBase<Person>> Update(Person person)
        {
            return await PersonsDAO.Update(person);
        }

        public async Task<Person?> GetById(long personId)
        {
            return await PersonsDAO.GetById(personId);
        }

        public async Task<List<Person>> GetAll()
        {
            return await PersonsDAO.GetAll();
        }
    }
}
