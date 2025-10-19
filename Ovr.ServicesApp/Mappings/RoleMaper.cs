using Ovr.Domain.DTOs;
using Ovr.Domain.Models;

namespace Ovr.ServicesApp.Mappings
{
    public static class RoleMaper
    {

        public static User? MapToModel(UserDto? dto)
        {
            if (dto == null) return null;

            return new User
            {
                UserId = dto.UserId,
                PersonId = dto.PersonId,
                Email = dto.Email,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive
            };
        }

        // Mapea de Entity a DTO (User -> UserDto)
        public static UserDto? MapToDto(User? entity)
        {
            if (entity == null) return null;

            return new UserDto
            {
                UserId = entity.UserId,
                PersonId = entity.PersonId,
                Email = entity.Email,
                RoleId = entity.RoleId,
                IsActive = entity.IsActive
            };
        }

        // Métodos para listas
        public static IEnumerable<UserDto?>? MapToDtoList(IEnumerable<User>? entities)
        {
            return entities?.Select(MapToDto).ToList();
        }

        public static IEnumerable<User?>? MapToModelList(IEnumerable<UserDto>? dtos)
        {
            return dtos?.Select(MapToModel).ToList();
        }
    }
}