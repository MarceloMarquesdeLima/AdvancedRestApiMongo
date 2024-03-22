using AdvancedRestApiMongo.DTOs;
using AdvancedRestApiMongo.Interfaces;
using AdvancedRestApiMongo.Models;
using AutoMapper;
using MongoDB.Driver;
using System.Collections.Generic;

namespace AdvancedRestApiMongo.Services
{
    public class UserService : IUser
    {
        private IMongoCollection<User> usersCollection;
        private IMapper _mapper;

        public UserService(IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            var mongoClient = new MongoClient(config.GetConnectionString("UserConnection"));
            var mongoDatabase = mongoClient.GetDatabase("UsersDb");
            usersCollection = mongoDatabase.GetCollection<User>("Users");         
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddUser(UserDTO userdto)
        {
            if(userdto != null)
            {
                var user = _mapper.Map<User>(userdto);
                await usersCollection.InsertOneAsync(user);
                return (true, null);
            }
            return(false, "Forneça os dados do usuário!");
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id)
        {
            var user = await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                await usersCollection.DeleteOneAsync(u => u.Id == id);
                return (true, null);
            }
            return (false, "Usuário não encontrado.");
        }

        public async Task<(bool IsSuccess, List<UserDTO> User, string ErrorMessage)> GetAllUsers()
        {
            var users = await usersCollection.Find(u => true).ToListAsync();
            if(users != null)
            {
                var result = _mapper.Map<List<UserDTO>>(users);
                return (true, result, null);
            }
            return (false,null,"Usuários não encontrados");
        }

        public async Task<(bool IsSuccess, UserDTO User, string ErrorMessage)> GetUserById(Guid id)
        {
            var user = await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
            {
                var result = _mapper.Map<UserDTO>(user);
                return (true, result, null);
            }
            return (false, null, "Usuário não encontrado" );
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO userdto)
        {
            var userObj = await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if(userObj != null)
            {
                var user = _mapper.Map<User>(userdto);
                userObj.Name = user.Name;
                userObj.Address = user.Address;
                userObj.Phone = user.Phone;
                userObj.BloodGroup = user.BloodGroup;
                await usersCollection.ReplaceOneAsync(u => u.Id == id, user);
                return (true, null);
            }
            return (false, "Usuário não existe");
        }
    }
}
