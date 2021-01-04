using Microsoft.AspNet.Identity.EntityFramework;
using MvcUserAndRoles.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcUserAndRoles.ViewModels
{
    public class UserRoles
    {
        public ApplicationUser  User { get; set; }
        public IdentityRole Role { get; set; }
        public IdentityUserRole UserRole { get; set; }
    }
}