using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.OrdersRepository
{
   public class OrderSummaryForUser
    {
        public int OrderID { get; set; }  
        
        public string CarLicense { get; set; }     
        public DateTime StartDayRent { get; set; }     
        public DateTime EndRentDay { get; set; }
        public DateTime? FinalyEndRentDay { get; set; } = null;
        public bool IsOrderClosed { get; set; }
        public int? TotalSumOfRent { get; set; } = null;
        public string Car { get; set; }
        public int PriceForDay { get; set; }
       
        public int PenaltyPrice { get; set; }

    }
}
