using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DataModels
{
    public class Order
    {
   

        public int OrderID { get; set; }
      
        [Required]
        public int UserID { get; set; }
        [Required]
        public int CarID { get; set; }

        [Required, MaxLength(50) ,Column(TypeName = "nvarchar(50)")]
        public string CarLicense { get; set; }

        [Required]
        public DateTime StartDayRent { get; set; }
        [Required]
        public DateTime EndRentDay { get; set; }
        public DateTime? FinalyEndRentDay { get; set; } = null;
        public bool IsOrderClosed { get; set; } = false;
        public int? TotalSumOfRent { get; set; } = null;

        [JsonIgnore]
        public virtual User UserNavigation { get; set; }
        [JsonIgnore]
        public virtual Car CarNavigation { get; set; }



    }
}
