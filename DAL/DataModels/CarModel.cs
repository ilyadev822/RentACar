using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModels
{
 public   class CarModel
    {
        public int CarModelID { get; set; }

        [Required, MaxLength(50)]
        public string CarVendor { get; set; }

        [Required, MaxLength(50)]
        public string CarModelName { get; set; }

        [Required,Range(0, Int32.MaxValue)]
        public int PriceForDay { get; set; }

        [Required, Range(0, Int32.MaxValue)]
        public int PriceForDayLate  { get; set; }

        [Required]
        public DateTime YearOfManufacture { get; set; }

        [Required(ErrorMessage ="Gear not valid!")]
        public Gear Gear { get; set; }
      

        public virtual ICollection<Car> Cars { get; set; }
        

    }
}
