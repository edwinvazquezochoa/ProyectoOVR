using Ovr.DAO;
using Ovr.Domain.DTOs;
using Ovr.Domain.Models;

namespace Ovr.ServicesApp.Mappings
{
    public static class UserMaper
    {

        public static User? MapToModel(UserDto? dto)
        {
            User? userMap = null;
            if (dto != null)
            {
                userMap = new User();
                userMap.UserId = dto.UserId;
                userMap.PersonId = dto.PersonId;
                userMap.Email = dto.Email;
                userMap.RoleId = dto.RoleId;
                userMap.IsActive = dto.IsActive;
            }
            return userMap;
        }

        // Mapea de Entity a DTO (User -> UserDto)
        public static async Task<UserDto?> MapToDto(long userId)
        {
            if (userId == 0) return null;

            UserDto userMap = new UserDto();
            User? userExist = await UserDao.GetById(userId);
            if (userExist != null)
            {
                userMap.UserId = userExist.UserId;
                userMap.PersonId = userExist.PersonId;
                userMap.Email = userExist.Email;
                userMap.RoleId = userExist.RoleId;
                userMap.IsActive = userExist.IsActive;

                // Get person data 
                Person? person = await PersonDao.GetById(userMap.PersonId);
                if (person != null)
                {
                    userMap.ShortName = person.ShortName;
                    userMap.FullName = person.FullName;
                }
                // Get role data 
                Role? role = await RoleDAO.GetById(userMap.RoleId);
                if (role != null)
                {
                    userMap.RoleName = role.RoleName;
                }
            }
            return userMap;
        }

        // Métodos para listas
        public static async Task<IEnumerable<UserDto?>?> MapToDtoList()
        {
            var persons = await PersonDao.GetAll();
            var roles = await RoleDAO.GetAll();
            var users = await UserDao.GetAll();
            var usersDto = new List<UserDto>();
            foreach (var user in users) 
            { 
                var userDto = new UserDto();
                userDto.UserId = user.UserId;
                userDto.PersonId = user.PersonId;
                userDto.Email = user.Email;
                userDto.RoleId = user.RoleId;
                userDto.IsActive = user.IsActive;

                // Get person data 
                Person? person = persons.Where(u=> u.PersonId== user.PersonId).FirstOrDefault();
                if (person != null)
                {
                    userDto.ShortName = person.ShortName;
                    userDto.FullName = person.FullName;
                }
                // Get role data 
                Role? role = roles.Where(u => u.RoleId == user.RoleId).FirstOrDefault();
                if (role != null)
                {
                    userDto.RoleName = role.RoleName;
                }
                usersDto.Add(userDto);
            }
            return usersDto;
        }

        public static Task<IEnumerable<User?>?> MapToModelList(List<UserDto?> userDtos)
        {
            var users = new List<User?>();
            foreach (var userDto in userDtos)
            {
                var user = new User();
                user.UserId = userDto.UserId;
                user.PersonId = userDto.PersonId;
                user.Email = userDto.Email;
                user.RoleId = userDto.RoleId;
                user.IsActive = userDto.IsActive;
                users.Add(user);
            }
            return Task.FromResult<IEnumerable<User?>?>(users);
        }
    }
}
