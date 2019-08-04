using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Address
    {
        public long Id { get; set; }
        public string IP { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address_Line { get; set; } = string.Empty;
        public string Address_Line2 { get; set; } = string.Empty;
        public string Address_Line3 { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Address_Type { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public long UserId { get; set; }
        public int ISDCode { get; set; }
        public long PhoneNumber { get; set; }
        public string Full_Name { get { return string.Format("{0} {1}", First_Name, Last_Name); } }
        public string Complete_Address { get { return string.Format("{0}<br/>{1}<br/>{2}, {3}<br/>{4}, {5}<br/>Mobile: {6}", Address_Line, Area, City, State, Country, ZipCode, Mobile); } }
    }

    public class Address_DTO
    {
        public string TrackingId { get; set; }

        public Address Billing { get; set; }
        public Address Shipping { get; set; }

        public Address_DTO()
        {
            Billing = new Address();
            Shipping = new Address();
        }
    }
}
