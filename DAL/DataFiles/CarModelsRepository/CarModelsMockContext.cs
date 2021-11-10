using DAL.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.CarModelsRepository
{
    public class CarModelsMockContext : ICarModelsRepository
    {
        DataContext _dbContext = new DataContext();
        public async Task<ResponseModel> CreateNewCarModel(CarModel newCarModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                _dbContext.Add(newCarModel);
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
            return response;
        }

        public async Task<ResponseModel> DeleteCarModel(CarModel CarModelToDelete)
        {
            ResponseModel response = new ResponseModel();
            var CarsWithThisModel = (from car in _dbContext.Cars
                                     where car.CarModelID == CarModelToDelete.CarModelID
                                     select car
                                          ).FirstOrDefault();
            if (CarsWithThisModel != null)
            {
                response.IsSuccess = false;
                response.ErrMessage = "Cant Delete this  car model,there car/s created from this car model";
            }
            else
            {
                try
                {
                    _dbContext.Remove(CarModelToDelete);
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

        public async Task<List<CarModel>> GetAllCarModels()
        {
            return await _dbContext.CarModels.ToListAsync();
        }

        public async Task<CarModel> GetCarModelById(int id)
        {
            return await _dbContext.CarModels.FirstOrDefaultAsync(carModel => carModel.CarModelID == id);
        }
        public async Task<List<CarModelName>> GetCarModelsByCarVendor(string carVendor)

        {           
            var DataList = (await (from car in _dbContext.Cars
                                   join carModel in _dbContext.CarModels
                                  on car.CarModelID equals carModel.CarModelID
                                   where carModel.CarVendor == carVendor
                                   select carModel.CarModelName).ToListAsync()).Distinct();
            List<CarModelName> returnList = new List<CarModelName>();
            foreach (var item in DataList)
            {
                CarModelName modelName = new CarModelName();
                modelName.CarModel = item;
                returnList.Add(modelName);
            }
            return returnList;
        }

        

        public async Task<List<CarVendorModel>> GetCarVendors()
        {
            var DataList = (await (from car in _dbContext.Cars
                                   join carModel in _dbContext.CarModels
                                  on car.CarModelID equals carModel.CarModelID
                                   select carModel.CarVendor).ToListAsync()).Distinct();
            List<CarVendorModel> returnList = new List<CarVendorModel>();
            foreach (var item in DataList)
            {
                CarVendorModel vendor = new CarVendorModel();
                vendor.CarVendor = item;
                returnList.Add(vendor);
            }
            return returnList;

        }

        public async Task<List<YearOfManufacture>> GetManufactureYearByCarModel(string carModelName)
        {
            var DataList = (await (from car in _dbContext.Cars
                                   join carModel in _dbContext.CarModels
                                  on car.CarModelID equals carModel.CarModelID
                                   where carModel.CarModelName == carModelName
                                   select carModel.YearOfManufacture).ToListAsync()).Distinct();

            List<YearOfManufacture> returnList = new List<YearOfManufacture>();
            foreach (var item in DataList)

            {
                YearOfManufacture year = new YearOfManufacture();
                year.Year = item;
                returnList.Add(year);

            }
            return returnList;
        }

        public async Task<ResponseModel> UpdateCarModel(CarModel ExistingCarModel, CarModel CarModelForUpdate)
        {
            ResponseModel response = new ResponseModel();
            var ExistingCar = await _dbContext.Cars.Where(car => car.CarModelID == ExistingCarModel.CarModelID).FirstOrDefaultAsync();
            if (ExistingCar != null && (ExistingCarModel.CarModelName != CarModelForUpdate.CarModelName || ExistingCarModel.CarVendor != CarModelForUpdate.CarVendor))
            {
                response.IsSuccess = false;
                response.ErrMessage = "Cant change car model name or/and car model vendor because there existing cars with this model";
            }
            else
            {
                try
                {
                    ExistingCarModel.CarModelName = CarModelForUpdate.CarModelName;
                    ExistingCarModel.CarVendor = CarModelForUpdate.CarVendor;
                    ExistingCarModel.Gear = CarModelForUpdate.Gear;
                    ExistingCarModel.PriceForDay = CarModelForUpdate.PriceForDay;
                    ExistingCarModel.PriceForDayLate = CarModelForUpdate.PriceForDayLate;
                    ExistingCarModel.YearOfManufacture = CarModelForUpdate.YearOfManufacture;
                    await _dbContext.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ResponseObject = ExistingCarModel;

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

    }
}
