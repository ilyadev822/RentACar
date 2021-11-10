using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModels
{
   public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string ErrMessage { get; set; }
        public object ResponseObject { get; set; }

    }
}
