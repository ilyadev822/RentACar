using DAL.DataFiles;
using DAL.DataFiles.CarModelsRepository;
using DAL.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
      
        private readonly ICarModelsRepository _carModelsRepository;

        public CarModelController(ICarModelsRepository carModelsRepository)
        {
          
            _carModelsRepository = carModelsRepository;
        }
        // GET all car models
        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> GetAllCarModels()
        {


            var CarModels = await _carModelsRepository.GetAllCarModels();
            return Ok(CarModels);
        }

        // GET Car model by CarModelID
        [HttpGet]
        [Route("[Action]/{id}")]
        public async Task<IActionResult> GetCarModelById(int id)
        {
            var CarModel = await _carModelsRepository.GetCarModelById(id);
            if (CarModel != null)
            {
                return Ok(CarModel);
            }
            else
            {
                return NotFound();
            }
        }

        // POST Create New Car Model
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewCarModel([FromBody] CarModel NewCarModel)
        {
            var result = await _carModelsRepository.CreateNewCarModel(NewCarModel);
            if (result.IsSuccess)
            {
                return Created("", NewCarModel);
            }
            else
            {
                return BadRequest(result.ErrMessage);
            }
        }

        // PUT Update Car Model
        [HttpPut]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCarModel(int id, [FromBody] CarModel CarModelForUpdate)
        {
            var ExistingCarModel = await _carModelsRepository.GetCarModelById(id);
            if (ExistingCarModel != null)
            {
                var result = await _carModelsRepository.UpdateCarModel(ExistingCarModel, CarModelForUpdate);
                if (result.IsSuccess)
                {


                    return Ok(new CarModel
                    {
                        CarModelID = ((CarModel)result.ResponseObject).CarModelID,
                        CarModelName = ((CarModel)result.ResponseObject).CarModelName,
                        CarVendor= ((CarModel)result.ResponseObject).CarVendor,
                        Gear= ((CarModel)result.ResponseObject).Gear,
                        YearOfManufacture= ((CarModel)result.ResponseObject).YearOfManufacture,
                        PriceForDay= ((CarModel)result.ResponseObject).PriceForDay,
                        PriceForDayLate= ((CarModel)result.ResponseObject).PriceForDayLate
                    }); ;
                }
                else
                {
                    return BadRequest(result.ErrMessage);
                }
            }
            else
            {
                return NotFound();
            }
        }
        // DELETE Car model
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            var CarModelToDelete = await _carModelsRepository.GetCarModelById(id);
            if (CarModelToDelete != null)
            {
                var result = await _carModelsRepository.DeleteCarModel(CarModelToDelete);
                if (result.IsSuccess)
                {
                    return Ok();
                }
                else return BadRequest(result.ErrMessage);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
