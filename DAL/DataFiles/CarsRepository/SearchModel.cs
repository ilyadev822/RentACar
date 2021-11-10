using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataFiles.CarsRepository
{
    public class SearchModel
    {
        public int? Carid { get; set; }
        public string CarVendor { get; set; }
        public string CarModel { get; set; }
        public Gear? Gear { get; set; }
        public string BranchName { get; set; }
        public DateTime? Year { get; set; }
        public int? Kilometrag { get; set; }
        public string Picture { get; set; }

    }
}
