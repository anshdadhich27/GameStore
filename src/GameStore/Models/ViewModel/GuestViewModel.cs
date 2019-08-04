using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.ViewModel
{
    public class GuestViewModel
    {
        [Required]
        [EmailAddress]
        [Remote("CheckEmail", "Account", ErrorMessage = "This email is already registered", HttpMethod = "POST")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Cart Id")]
        public string CartId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "OTP")]
        public long OTP { get; set; }
    }
}
