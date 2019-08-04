using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Career
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone_No { get; set; }
        public string Applied_For { get; set; }
        public string Message { get; set; }
        public string CV_Path { get; set; }
        public DateTime Added_On { get; set; }
        public bool EmailConfirmed { get; set; }
        public int OTP { get; set; }
    }
}
