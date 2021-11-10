using DAL.DataFiles;
using DAL.DataFiles.BranchesRepository;
using DAL.DataFiles.CarModelsRepository;
using DAL.DataFiles.CarsRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
       
        private readonly ICarModelsRepository _carModelRepository;
        private readonly ICarsRepository _carsRepository;

        public SearchController(ICarModelsRepository carModelRepository, ICarsRepository carsRepository)
        {
            _carModelRepository = carModelRepository;
            _carsRepository = carsRepository;
        }

        // GET car vendors
        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> GetCarVendors()
        {
            var CarVendors = await _carModelRepository.GetCarVendors();         
            return Ok(CarVendors);
        }

        //get CarModels by CarVendor
        [HttpGet]
        [Route("[Action]/{CarVendor}")]
        public async Task<IActionResult> GetCarModelsByCarVendor(string carVendor)
        {
           
            var CarModels = await _carModelRepository.GetCarModelsByCarVendor(carVendor);
            return Ok(CarModels);
        }


        //get ManufactureYears by CarModelName
        [HttpGet]
        [Route("[Action]/{carModelName}")]
        public async Task<IActionResult> GetManufactureYearByCarModel(string carModelName)
        {
            var Years = await _carModelRepository.GetManufactureYearByCarModel(carModelName);
            return Ok(Years);
        }
        // POST: return  List of  cars by search values
        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> SearchAvailableCarsForRent([FromBody] SearchModel searchModel)
        {
            var cars = await _carsRepository.SearchAvailableCarsForRent(searchModel);

            return Ok(cars);
        }

      
    }
}
