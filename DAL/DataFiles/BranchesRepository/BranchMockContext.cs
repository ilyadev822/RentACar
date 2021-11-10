using DAL.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.BranchesRepository
{
    public class BranchMockContext : IBranchRepository
    {
        DataContext _dbContext = new DataContext();

        public async Task<ResponseModel> CreateNewBranch(Branch newBranch)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                _dbContext.Add(newBranch);
                await _dbContext.SaveChangesAsync();
                response.IsSuccess = true;

            }
            catch (DbUpdateException dbEx)
            {
                response.IsSuccess = false;
                response.ErrMessage= ((SqlException)dbEx.InnerException).Message;
               
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrMessage= ex.Message;
            }
            return response;
        }

      

        public async  Task<List<Branch>> GetAllBranches()
        {
            return await _dbContext.Branches.ToListAsync();
        }

        public async Task<Branch> GetBranchById(int id)
        {
            return await _dbContext.Branches.FirstOrDefaultAsync(branch => branch.BranchID == id);
        }

        public async Task<ResponseModel> UpdateBranch(Branch ExistingBranch, Branch BranchForUpdate)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ExistingBranch.BranchAdress = BranchForUpdate.BranchAdress;
                ExistingBranch.BranchName = BranchForUpdate.BranchName;
                ExistingBranch.Latitude = BranchForUpdate.Latitude;
                ExistingBranch.Longitude = BranchForUpdate.Longitude;
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

        public async Task<ResponseModel> DeleteBranch(Branch branchToDelete)
        {
            ResponseModel response = new ResponseModel();
            var ExistingCarInBranch = await _dbContext.Cars.FirstOrDefaultAsync(car => car.BranchID == branchToDelete.BranchID);
            if(ExistingCarInBranch != null)
            {
                response.IsSuccess = false;
                response.ErrMessage= "Cant remove branche with cars,please remove cars from this branch first";
            }
            else
            {
                try
                {
                    _dbContext.Remove(branchToDelete);
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
    }
}
