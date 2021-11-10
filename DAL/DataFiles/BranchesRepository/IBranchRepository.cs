using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.BranchesRepository
{
    public interface IBranchRepository
    {
        public Task<List<Branch>> GetAllBranches();
        public Task<Branch> GetBranchById(int id);
        public Task<ResponseModel> CreateNewBranch(Branch newBranch);
        public Task<ResponseModel> UpdateBranch(Branch ExistingBranch, Branch BranchForUpdate);
        public Task<ResponseModel> DeleteBranch(Branch branchToDelete);





    }
}
