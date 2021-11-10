using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.OrdersRepository
{
    public interface IOrdersRepository
    {
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(int id);
      
        public Task<ResponseModel> CreateNewOrder(Order NewOrder);
        public Task<ResponseModel> CloseOrder(string license);
        public Task<ResponseModel> UpdateOrder(Order ExistingOrder, Order OrderToUpdate);
        public Task<ResponseModel> DeleteOrder(Order OrderToDelete);

    }
}
