using DAL.DataFiles.BranchesRepository;
using DAL.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.CarsRepository
{
    public class CarsMockContext : ICarsRepository
    {
        DataContext _dbContext = new DataContext();

      

        public async Task<List<Car>> GetAllCars()
        {

            return await _dbContext.Cars.ToListAsync();
        }

        public async Task<Car> GetCarById(int id)
        {
            return await _dbContext.Cars.FirstOrDefaultAsync(car => car.CarID == id);
        }

        public async Task<ResponseModel> CreateNewCar(Car newCar)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                _dbContext.Add(newCar);
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
            return response ;
        }

        public async Task<ResponseModel> UpdateCar(Car ExistingCar, Car CarForUpdate)
        {
            ResponseModel response = new ResponseModel();
            var OpenedOrderWithThisCar = (from order in _dbContext.Orders                                          
                                          where order.IsOrderClosed == false && order.CarLicense == ExistingCar.CarLicense
                                          select order
                                          ).FirstOrDefault();
            if (CarForUpdate.IsAvailbleForRent == true && OpenedOrderWithThisCar != null)
            {
                response.IsSuccess = false;
                response.ErrMessage = "cant change status availble for rent of this car.\\nThere are opened order with this car ";
            }
            else
            {
                try
                {
                    ExistingCar.BranchID = CarForUpdate.BranchID;
                    ExistingCar.CarLicense = CarForUpdate.CarLicense;
                    ExistingCar.CarPicture = CarForUpdate.CarPicture;
                    ExistingCar.IsProperForRent = CarForUpdate.IsProperForRent;
                    ExistingCar.Kilometrag = (CarForUpdate.Kilometrag != null) ? CarForUpdate.Kilometrag : ExistingCar.Kilometrag;
                    ExistingCar.IsAvailbleForRent = CarForUpdate.IsAvailbleForRent;
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

        public async Task<ResponseModel> DeleteCar(Car CarToDelete)
        {
            ResponseModel response = new ResponseModel();
            var OrderWithThisCar = (from order in _dbContext.Orders
                                          where order.CarLicense == CarToDelete.CarLicense
                                          select order
                                         ).FirstOrDefault();
            if(OrderWithThisCar != null)
            {
                response.IsSuccess = false;
                response.ErrMessage = "Cant Delete this car,there are orders with this car";
            }
            else
            {
                try
                {
                    _dbContext.Remove(CarToDelete);
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

        public async Task<List<SearchModel>> SearchAvailableCarsForRent(SearchModel searchModel)
        {
            return  await (from car in _dbContext.Cars
                                  join carModel in _dbContext.CarModels
                                  on car.CarModelID equals carModel.CarModelID
                                  join branch in _dbContext.Branches
                                   on car.BranchID equals branch.BranchID
                                  where car.IsAvailbleForRent == true && car.IsProperForRent == true &&
                                 (((searchModel.Gear != null) ? carModel.Gear == searchModel.Gear : true)) &&
                                  ((searchModel.Year != null) ? carModel.YearOfManufacture == searchModel.Year : true) &&
                                  ((searchModel.CarModel != null) ? carModel.CarModelName == searchModel.CarModel : true) &&
                                  ((searchModel.CarVendor != null) ? carModel.CarVendor == searchModel.CarVendor : true)
                                  select new SearchModel
                                  {
                                      Carid = car.CarID,
                                      CarVendor = carModel.CarVendor,
                                      CarModel = carModel.CarModelName,
                                      Gear = carModel.Gear,
                                      BranchName = branch.BranchName,
                                      Year = carModel.YearOfManufacture,
                                      Kilometrag = car.Kilometrag,
                                      Picture = car.CarPicture
                             
                                  }).ToListAsync();
        }
    }
}
