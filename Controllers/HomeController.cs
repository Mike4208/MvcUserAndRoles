using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcUserAndRoles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcUserAndRoles.Controllers
{
    public class HomeController : Controller
    {
        public bool IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>
                    (new UserStore<ApplicationUser>(context));
                var s = userManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                    return true;
                else
                    return false;
            }
            return false;
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                // ApplicationDbContext context = new ApplicationDbContext();
                // var userManager = new UserManager<ApplicationUser>
                //(new UserStore<ApplicationUser>(context));

                //var s = userManager.GetRoles(user.GetUserId());
                ViewBag.DisplayMenu = "No";

                if (IsAdminUser())
                    ViewBag.DisplayMenu = "Yes";
                else
                    ViewBag.Name = "Not Logged IN";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}