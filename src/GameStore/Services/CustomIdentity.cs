using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace GameStore.Services
{
    public class CustomIdentity : IIdentity
    {
        public IEnumerable<Claim> Claims { get; }
        public Claim FindFirst(string type) { return Claims.First(x => x.Type == type); }
        public Claim HasClaim(string type, string value) { return Claims.First(x => x.Type == type && x.Value == value); }

        public string Name { get; private set; }
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        public string AuthenticationType { get; private set; }

        public CustomIdentity(string name, string type)
        {
            Name = name;
            AuthenticationType = type;
        }
    }
}
