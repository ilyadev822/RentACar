using DAL.DataFiles;
using DAL.DataFiles.BranchesRepository;
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
    public class BranchController : ControllerBase
    {
        
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository)
        {
            
            _branchRepository = branchRepository;
        }
        // GET All Braches
        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> GetAllBranches()
        {

            var branches = await _branchRepository.GetAllBranches();
            return Ok(branches);
        }

        // GET Branch by Id
        [HttpGet]
        [Route("[Action]/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _branchRepository.GetBranchById(id);
            if(branch != null)
            {
                return Ok(branch);
            }
            else
            {
                return NotFound();
            }
        }

        // POST Create new branch
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewBranch([FromBody] Branch  NewBranch)
        {
            var result = await _branchRepository.CreateNewBranch(NewBranch);
            if (result.IsSuccess)
            {
                return Created("",NewBranch);
            }
            else
            {
                return BadRequest(result.ErrMessage);
            }
        }

        // PUT Update existing bracnh
        [HttpPut]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] Branch BranchForUpdate)
        {
            var ExistingBranch = await _branchRepository.GetBranchById(id);
            if(ExistingBranch != null)
            {
                var result = await _branchRepository.UpdateBranch(ExistingBranch, BranchForUpdate);
                if (result.IsSuccess)
                {
                    return Ok(ExistingBranch);
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
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var ExistingBranch = await _branchRepository.GetBranchById(id);
            if (ExistingBranch != null)
            {
                var result =await _branchRepository.DeleteBranch(ExistingBranch);
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
