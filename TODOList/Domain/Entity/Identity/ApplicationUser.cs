using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Security.Claims;
using TODOList.Domain.Concrete.Owin;

namespace TODOList.Domain.Entity.Identity {
    public class ApplicationUser : IdentityUser<long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<long> {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager) {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public virtual ICollection<TodoList> TodoLists { get; set; }
    }
}