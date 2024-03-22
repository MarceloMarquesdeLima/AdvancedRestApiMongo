using AdvancedRestApiMongo.Data;
using AdvancedRestApiMongo.DTOs;
using AdvancedRestApiMongo.Interfaces;
using AdvancedRestApiMongo.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdvancedRestApiMongo.Services
{
    public class UserService : IUser
    {
        private UserDbContext _dbContext;
        private IMapper _mapper;

        public UserService(UserDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddUser(UserDTO userdto)
        {
            if(userdto != null)
            {
                var user = _mapper.Map<User>(userdto);
                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            return(false, "Forneça os dados do usuário!");
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            return (false, "Usuário não encontrado.");
        }

        public async Task<(bool IsSuccess, List<UserDTO> User, string ErrorMessage)> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if(users != null)
            {
                var result = _mapper.Map<List<UserDTO>>(users);
                return (true, result, null);
            }
            return (false,null,"Usuários não encontrados");
        }

        public async Task<(bool IsSuccess, UserDTO User, string ErrorMessage)> GetUserById(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user != null)
            {
                var result = _mapper.Map<UserDTO>(user);
                return (true, result, null);
            }
            return (false, null, "Usuário não encontrado" );
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO userdto)
        {
            var userObj = await _dbContext.Users.FindAsync(id);
            if(userObj != null)
            {
                var user = _mapper.Map<User>(userdto);
                userObj.Name = user.Name;
                userObj.Address = user.Address;
                userObj.Phone = user.Phone;
                userObj.BloodGroup = user.BloodGroup;
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            return (false, "Usuário não existe");
        }
    }
}
