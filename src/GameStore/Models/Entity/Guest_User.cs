using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Guest_User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Cart_Id { get; set; }
        public long OTP { get; set; }
    }
}
