using DAL.DataFiles;
using DAL.DataFiles.BranchesRepository;
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
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarsRepository _carsContext;

        public CarController(ICarsRepository carsContext)
        {
            _carsContext = carsContext;
        }

        


        // GET all cars (for manager menu)
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllCars()
        {
           
            var cars = await _carsContext.GetAllCars();


            return Ok(cars);
        }

        // GET car by id
        [HttpGet]
        [Route("[Action]/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carsContext.GetCarById(id);
            if (car != null)
            {
                return Ok(car);
            }
            else
            {
                return NotFound();
            }
        }

        // POST create new car
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewCar([FromBody] Car NewCar)
        {
            var result = await _carsContext.CreateNewCar(NewCar);
            if (result.IsSuccess)
            {
                return Created("", NewCar);
            }
            else
            {
                return BadRequest(result.ErrMessage);
            }
        }

        //PUT Update car
        [HttpPut]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car CarForUpdate)
        {

            var ExistingCar = await _carsContext.GetCarById(CarForUpdate.CarID);
            if(ExistingCar != null)
            {
                var result = await _carsContext.UpdateCar(ExistingCar, CarForUpdate);
                if (result.IsSuccess)
                {
                    return Ok(ExistingCar);
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

        // DELETE car
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCar(int id)
        {

            var car = await _carsContext.GetCarById(id);
            if (car != null)
            {
                var result = await _carsContext.DeleteCar(car);
                if (result.IsSuccess)
                {
                    return Ok();
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
    }
}
