using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GuideMVC_.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public virtual Person Person { get; set; }
    }
}
