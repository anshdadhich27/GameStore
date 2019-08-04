using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace GameStore.Services
{
    public class CustomPrincipal : IPrincipal
    {
        public IEnumerable<ClaimsIdentity> Identities { get; }
        public IEnumerable<Claim> Claims { get; }

        public Claim FindFirst(string type) { return null; }
        public Claim HasClaim(string type, string value) { return null; }

        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return true; }

        public CustomPrincipal(CustomIdentity identity)
        {
            Identity = identity;
        }
    }
}
