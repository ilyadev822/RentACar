using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserDetailsModel
    {

        public int UserID { get; set; }       
        public string FullName { get; set; }   
        public string Tz { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        public Gender Gender { get; set; }     
        public string Email { get; set; }
        public string UserName { get; set; }            
        public string Picture { get; set; }
        public Roles Roles { get; set; }
      
    }
}
