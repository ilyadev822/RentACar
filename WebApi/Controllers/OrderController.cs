using DAL.DataFiles;
using DAL.DataFiles.OrdersRepository;
using DAL.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        // GET all orders
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Manager,Worker")]
        public async Task<IActionResult> GetAllOrders()
        {
            var OrderList = await _ordersRepository.GetAllOrders();
            return Ok(OrderList);
        }

        // GET order by id
        [HttpGet]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager,Worker,Customer")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _ordersRepository.GetOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound();
            }
        }
        
        
        //Create new order
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "Manager,Worker,Customer")]
        public async Task<IActionResult> CreateNewOrder([FromBody] Order NewOrder)
        {
            var result = await _ordersRepository.CreateNewOrder(NewOrder);
          
            if (result.IsSuccess)
            {
                return Ok(NewOrder);
            }
            else
            {
                return BadRequest(result.ErrMessage);
            }
        }
        //close order for worker menu
        [HttpPost]
        [Route("[Action]/{license}")]
       [Authorize(Roles = "Manager,Worker")]
        public async Task<IActionResult> CloseOrder(string license)
        {                          
                var result = await _ordersRepository.CloseOrder(license);
                if (result.IsSuccess)
                {
                    return Ok(result.ResponseObject);
                }
                else
                {
                    return BadRequest(result.ErrMessage);
                }           
        }

        //  PUT Update Order(for manager menu)

        [HttpPut]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order OrderToUpdate)
        {

            var ExistingOrder = await _ordersRepository.GetOrderById(id);
            if (ExistingOrder != null)
            {
                var result = await _ordersRepository.UpdateOrder(ExistingOrder, OrderToUpdate);
                if (result.IsSuccess)
                {
                    return Ok(ExistingOrder);
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
        // DELETE order
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var OrderToDelete = await _ordersRepository.GetOrderById(id);
            if (OrderToDelete != null)
            {
                var result = await _ordersRepository.DeleteOrder(OrderToDelete);
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
