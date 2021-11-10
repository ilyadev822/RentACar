using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DAL.DataFiles.OrdersRepository;

namespace DAL.DataFiles.UsersRepository
{
    public class UsersMockContext : IUsersRepository
    {
        DataContext _dbContext = new DataContext();



        public async Task<List<OrderSummaryForUser>> GetOrdersByUserId(int id)
        {
     
       return  await (from order in _dbContext.Orders
                              join car in _dbContext.Cars
                              on order.CarID equals car.CarID
                              join carModel in _dbContext.CarModels
                              on car.CarModelID equals carModel.CarModelID
                              where order.UserID == id
                              select new OrderSummaryForUser
                              {
                                  OrderID = order.OrderID,
                                  CarLicense=order.CarLicense,
                                  StartDayRent=order.StartDayRent,
                                  EndRentDay = order.EndRentDay,
                                  FinalyEndRentDay=order.FinalyEndRentDay,
                                  TotalSumOfRent=order.TotalSumOfRent,
                                  Car = carModel.CarVendor + " "+carModel.CarModelName,
                                  PriceForDay=carModel.PriceForDay,
                                  PenaltyPrice=carModel.PriceForDayLate,
                                  IsOrderClosed=order.IsOrderClosed
                              }
                              ).ToListAsync();


        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserID == id);
        }

        public async Task<string> Register(User newUser)
        {
            try 
            {
                _dbContext.Users.Add(newUser);
              await  _dbContext.SaveChangesAsync();
                return "success";

            }
            catch (DbUpdateException dbEx)
            {
                var DuplicateKey = (((SqlException)dbEx.InnerException).Number == 2601);
                return   (DuplicateKey) ? "This User Already exist" : ((SqlException)dbEx.InnerException).Message;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public async Task<User> GetUserByUsername(string userName)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        public async  Task<string> UpdateUser(User ExistingUser, User UserDataToUpdate)
        {
            try
            {
                ExistingUser.FullName = UserDataToUpdate.FullName;
                ExistingUser.UserName = UserDataToUpdate.UserName;
                ExistingUser.Picture = (UserDataToUpdate.Picture != null) ? UserDataToUpdate.Picture : ExistingUser.Picture;
                ExistingUser.Tz = UserDataToUpdate.Tz;
                ExistingUser.Roles = UserDataToUpdate.Roles;
                ExistingUser.Gender = UserDataToUpdate.Gender;
                ExistingUser.BirthDate = (UserDataToUpdate.BirthDate != null) ? UserDataToUpdate.BirthDate : ExistingUser.BirthDate;
                ExistingUser.Email = UserDataToUpdate.Email;
                ExistingUser.Password = (UserDataToUpdate.Password != null) ? UserDataToUpdate.Password : ExistingUser.Password;
           await     _dbContext.SaveChangesAsync();

                return "success";

            }
            catch (DbUpdateException dbEx)
            {
               
                return ((SqlException)dbEx.InnerException).Message;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public async  Task<string> DeleteUser(User UserToDelete)
        {
            try
            {
                 _dbContext.Remove(UserToDelete);
                await _dbContext.SaveChangesAsync();
                return "success";

            }
            catch (DbUpdateException dbEx)
            {
              
                return ((SqlException)dbEx.InnerException).Message;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
