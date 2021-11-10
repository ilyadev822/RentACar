using DAL.DataFiles.OrdersRepository;
using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.UsersRepository
{
    public interface IUsersRepository
    {
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByUsername(string userName);
        public Task<List<User>> GetAllUsers();
        public   Task<List<OrderSummaryForUser>> GetOrdersByUserId(int id);
        public Task<string> UpdateUser(User ExistingUser, User UserDataToUpdate);
        public Task<string> Register(User newUser);
        public Task<string> DeleteUser(User UserTodelete);

    }
}
