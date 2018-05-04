using Steeltoe.Security.Authentication.CloudFoundry.Wcf;
using System.Security.Permissions;

namespace CloudFoundryWcf
{
    public class ValueService : IValueService
    {
        [ScopePermission(SecurityAction.Demand, Scope = "testgroup")]
        public string GetData()
        {
            return "Hello from the WCF Sample!";
        }
    }
}
