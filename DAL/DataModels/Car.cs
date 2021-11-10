using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModels
{
  public  class Car
    {
        public int CarID  { get; set; }

        [Required]
        public int BranchID { get; set; }

        [Required]
        public int CarModelID { get; set; }
       
        public int? Kilometrag { get; set; } = 0;

        [Required]
        public string CarPicture { get; set; }
        public bool IsProperForRent { get; set; } = true;
        public bool IsAvailbleForRent { get; set; } = true;
        [Required, MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string CarLicense { get; set; }
       
        public virtual Branch BranchNavigation { get; set; }
        public virtual CarModel CarModelNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }


    }
}
