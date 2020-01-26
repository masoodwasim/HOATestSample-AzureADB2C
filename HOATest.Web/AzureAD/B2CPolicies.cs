using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.Web.AzureAD
{
    public class B2CPolicies
    {
        public string SignInOrSignUpPolicy { get; set; }
        public string EditProfilePolicy { get; set; }
        public string ResetPasswordPolicy { get; set; }
    }
}
