using DAL.CustomValidationAttributes;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UpdateUserModel
    {


        [Required]
        public string FullName { get; set; }
        [Required]
        [TzValidator(ErrorMessage ="Tz not valid!")]
        public string Tz { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        [Required]
        public Gender Gender { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Picture { get; set; }
        [Required]
        public Roles Roles { get; set; }
        public string UpdatedPassword { get; set; } = null;

       

    }
}
