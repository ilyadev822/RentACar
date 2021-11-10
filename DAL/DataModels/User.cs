using DAL.CustomValidationAttributes;
using DAL.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace DAL.DataModels
{
  public  class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string FullName { get; set; }

        [Required]
      //  [TzValidator(ErrorMessage = "Tz not valid!")]     
        public string Tz { get; set; }
        public DateTime? BirthDate { get; set; } =null;
        [Required]
    
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Picture { get; set; }
        [Required]     
        public Roles Roles { get; set; } = Roles.Customer;
      
        public virtual ICollection<Order> Orders { get; set; }
    }
}
