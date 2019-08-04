using System;

namespace GameStore.Models.Entity
{
    public class User 
    {
        public long Id { get; set; }

        public bool Active { get; set; }
        public int ISDCode { get; set; }
        public long PhoneNumber { get; set; }
        public bool PhoneVefified { get; set; }
        public bool EmailVerified { get; set; }
        
        public int ReferalCount { get; set; }
        public int ProfileScore { get; set; }
        public string ProfileRemainingOptions { get; set; } = string.Empty;
        public decimal CreditAmount { get; set; }

        public string Email { get; set; } = string.Empty;
        public string NameUrl { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string UserKey { get; set; } = string.Empty;
        public string SecurityCode { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;        
        public string Password { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;

        public DateTime? DOB { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        
        public long? ReferalKey { get; set; }
        public long Referal_Id { get; set; }

        public string Contact_Number { get { return string.Format("+{0}-{1}", ISDCode, PhoneNumber); } }
        public string Full_Name { get { return string.Format("{0} {1}", FirstName, LastName); } }


    }

    public class User_Credential
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class UserLogin
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string LoginProvider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
    }

    
}
