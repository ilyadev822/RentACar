using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.CarModelsRepository
{
   public  interface ICarModelsRepository
    {
        public Task<List<CarModel>> GetAllCarModels();
        public Task<CarModel> GetCarModelById(int id);
        public Task<ResponseModel> CreateNewCarModel(CarModel newCarModel);
        public Task<ResponseModel> UpdateCarModel(CarModel ExistingCarModel, CarModel CarModelForUpdate);
        public Task<ResponseModel> DeleteCarModel(CarModel CarModelToDelete);
        public Task<List<CarVendorModel>> GetCarVendors();
      
        public Task<List<CarModelName>> GetCarModelsByCarVendor(string varVendor);
        public Task<List<YearOfManufacture>> GetManufactureYearByCarModel(string carModelName);

     






    }
}
