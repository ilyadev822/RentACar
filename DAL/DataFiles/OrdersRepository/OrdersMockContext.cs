using DAL.DataFiles.CarsRepository;
using DAL.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.OrdersRepository
{
    public class OrdersMockContext : IOrdersRepository
    {
        DataContext _dbContext = new DataContext();



        public async Task<List<Order>> GetAllOrders()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderID == id);
        }
        public async Task<ResponseModel> CreateNewOrder(Order NewOrder)
        {
            ResponseModel response = new ResponseModel();
            var CarInOrder = await _dbContext.Cars.FirstOrDefaultAsync(car => car.CarLicense == NewOrder.CarLicense);
            if (CarInOrder.IsProperForRent && CarInOrder.IsAvailbleForRent && (NewOrder.EndRentDay >= NewOrder.StartDayRent))
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    CarInOrder.IsAvailbleForRent = false;
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Orders.AddAsync(NewOrder);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    response.IsSuccess = true;
                }

                catch (DbUpdateException dbEx)
                {

                    response.IsSuccess = false;
                    response.ErrMessage = ((SqlException)dbEx.InnerException).Message;
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.ErrMessage = ex.Message;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.ErrMessage = "The car is not Availble or proper for rent,or dates are not valid";
            }
            return response;
        }

        //return orders by license for worker menu
      

        public async Task<ResponseModel> CloseOrder(string license)
        {
            ResponseModel response = new ResponseModel();
            Order OrderToClose = await _dbContext.Orders.FirstOrDefaultAsync(order => order.CarLicense == license 
                                                                                 && order.IsOrderClosed == false);
            if(OrderToClose != null)
            {
                var CarInOrder = await _dbContext.Cars.FirstOrDefaultAsync(car => car.CarLicense == OrderToClose.CarLicense);
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    OrderToClose.FinalyEndRentDay = DateTime.Now;
                    OrderToClose.TotalSumOfRent = TotalOrderSumCalculator(OrderToClose);
                    OrderToClose.IsOrderClosed = true;
                    await _dbContext.SaveChangesAsync();
                    CarInOrder.IsAvailbleForRent = true;
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    response.IsSuccess = true;
                    response.ResponseObject = OrderToClose;
                }
                catch (DbUpdateException dbEx)
                {
                    response.IsSuccess = false;
                    response.ErrMessage = ((SqlException)dbEx.InnerException).Message;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.ErrMessage = ex.Message;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.ErrMessage = "No opened orders with this car license";
            }         
            return response;
        }
        private int TotalOrderSumCalculator(Order order)
        {
            var CarModel = (from carModel in _dbContext.CarModels
                            join car in _dbContext.Cars
                            on carModel.CarModelID equals car.CarModelID
                            where car.CarLicense == order.CarLicense
                            select carModel
                            ).FirstOrDefault();
            int TotalSum = 0;

            if (order.FinalyEndRentDay.Value.Date > order.EndRentDay.Date)//+penalty calculate
            {
                TotalSum = ((order.EndRentDay - order.StartDayRent).Days * CarModel.PriceForDay) +
                          ((Convert.ToDateTime(order.FinalyEndRentDay) - order.EndRentDay).Days * CarModel.PriceForDayLate);


            }
            else if (order.FinalyEndRentDay.Value.Date < order.StartDayRent.Date)//הוחזר ללא שימוש
            {
                TotalSum = 0;
            }
            else if (order.FinalyEndRentDay.Value.Date <= order.EndRentDay.Date && order.StartDayRent.Date != order.FinalyEndRentDay.Value.Date)//מצב רגיל
            {
                TotalSum = (Convert.ToDateTime(order.FinalyEndRentDay) - order.StartDayRent).Days * CarModel.PriceForDay;
            }
           
            else
            {
                // the last option when StartDay == FinalyDay, need to pay for 1 day 
                TotalSum = CarModel.PriceForDay;
            }
            return TotalSum;
        }

        public async Task<ResponseModel> UpdateOrder(Order ExistingOrder, Order OrderToUpdate)
        {
            ResponseModel response = new ResponseModel();
            
            if (OrderToUpdate.StartDayRent > OrderToUpdate.EndRentDay)
            {
                response.IsSuccess = false;
                response.ErrMessage="wrong start/end  dates ";
            }
            else
            {
                try
                {
                    ExistingOrder.UserID = OrderToUpdate.UserID;
                    ExistingOrder.StartDayRent = OrderToUpdate.StartDayRent;
                    ExistingOrder.EndRentDay = OrderToUpdate.EndRentDay;
                   
                    await _dbContext.SaveChangesAsync();
                    response.IsSuccess = true;

                }
                catch (DbUpdateException dbEx)
                {

                    response.IsSuccess = false;
                    response.ErrMessage = ((SqlException)dbEx.InnerException).Message;
                }
                catch (Exception ex)
                {

                    response.IsSuccess = false;
                    response.ErrMessage = ex.Message;
                }
            }
            return response;
        }

        public async  Task<ResponseModel> DeleteOrder(Order OrderToDelete)
        {
            ResponseModel response = new ResponseModel();
            var CarInOrder = await _dbContext.Cars.FirstOrDefaultAsync(car => car.CarLicense == OrderToDelete.CarLicense);
            using var transaction = _dbContext.Database.BeginTransaction();
            try 
            { 
                if(!OrderToDelete.IsOrderClosed)
                {
                    CarInOrder.IsAvailbleForRent = true;
                    await _dbContext.SaveChangesAsync();
                }
                _dbContext.Remove(OrderToDelete);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                response.IsSuccess = true;
            }
            catch (DbUpdateException dbEx)
            {

                response.IsSuccess = false;
                response.ErrMessage = ((SqlException)dbEx.InnerException).Message;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.ErrMessage = ex.Message;
            }
            return response;
        }
    }
}
