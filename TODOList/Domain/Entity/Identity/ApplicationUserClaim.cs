using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TODOList.Domain.Entity.Identity {
    public class ApplicationUserClaim : IdentityUserClaim<long> {
    }
}