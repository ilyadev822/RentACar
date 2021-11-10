using DAL.DataFiles.CarsRepository;
using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.BranchesRepository
{
  public  interface ICarsRepository
    {
        public Task<List<Car>> GetAllCars();
        public Task<Car> GetCarById(int id);
        public Task<ResponseModel> CreateNewCar(Car newCar);

        public Task<ResponseModel> UpdateCar(Car ExistingCar, Car CarForUpdate);
        public Task<ResponseModel> DeleteCar(Car CarToDelete);
        public Task<List<SearchModel>> SearchAvailableCarsForRent(SearchModel searchModel);

    }
}
