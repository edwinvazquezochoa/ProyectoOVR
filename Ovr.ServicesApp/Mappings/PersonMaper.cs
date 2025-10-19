using Ovr.Domain.DTOs;
using Ovr.Domain.Models;

namespace Ovr.ServicesApp.Mappings
{
    public static class PersonMaper
    {
        public static Person? MapToModel(PersonDto? dto)
        {
            if (dto == null) return null;

            return new Person
            {
                PersonId = dto.PersonId,
                pe = dto.Email,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive
            };
        }

        // Mapea de Entity a DTO (User -> UserDto)
        public static PersonDto? MapToDto(Person? entity)
        {
            if (entity == null) return null;

            return new PersonDto
            {
                UserId = entity.UserId,
                PersonId = entity.PersonId,
                Email = entity.Email,
                RoleId = entity.RoleId,
                IsActive = entity.IsActive
            };
        }

        // Métodos para listas
        public static IEnumerable<PersonDto?>? MapToDtoList(IEnumerable<Person>? entities)
        {
            return entities?.Select(MapToDto).ToList();
        }

        public static IEnumerable<Person?>? MapToModelList(IEnumerable<PersonDto>? dtos)
        {
            return dtos?.Select(MapToModel).ToList();
        }
    }
}
