using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string Email { get; set; }
        public int ISDCode { get; set; }
        public long PhoneNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Comments { get; set; }
    }
}
