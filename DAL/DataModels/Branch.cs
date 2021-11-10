using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModels
{
    public class Branch
    {
        public int BranchID { get; set; }

        [Required, MaxLength(50)]
        public string BranchName { get; set; }

        [Required, MaxLength(50)]
        public string BranchAdress { get; set; }

        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }           
        public virtual ICollection<Car> Cars { get; set; }

    }
}
