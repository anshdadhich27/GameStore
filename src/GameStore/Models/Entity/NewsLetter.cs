using System;

namespace GameStore.Models.Entity
{
    public class NewsLetter
    {
        public long Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Subscribe { get; set; }
        public bool Email_Verified { get; set; }
        public string Security_Key { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}
