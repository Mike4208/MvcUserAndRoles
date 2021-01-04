using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using MvcUserAndRoles.Models;
using MvcUserAndRoles.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcUserAndRoles.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext context;
        // GET: Users
        public UsersController()
        {
            this.context = new ApplicationDbContext();
        }
       
        public bool IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>
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
            
            var users = context.Users.ToList();
            var roles = context.Roles.ToList();
            List<UserRoles> viewModel = new List<UserRoles>();
            foreach (var user in users)
            {
                UserRoles u = new UserRoles();
                u.User = user;
                u.UserRole = user.Roles.Where(ur => ur.UserId == user.Id).FirstOrDefault();
                if (u.UserRole != null)
                {
                    u.Role = roles.Where(r => r.Id == u.UserRole.RoleId).FirstOrDefault();
                }
                viewModel.Add(u);
            }                 
            return View(viewModel);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
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

            var userDetailes = context.Users.FirstOrDefault(x => x.Id == id);
            var roles = context.Roles.ToList();
            if (userDetailes == null)
            {
                return HttpNotFound();
            }

            UserRoles userRole = new UserRoles();
            
            userRole.User = userDetailes;
            userRole.UserRole = userDetailes.Roles.Where(ur => ur.UserId == userDetailes.Id).FirstOrDefault();
            if (userRole.UserRole != null)
            {
                userRole.Role = roles.Where(r => r.Id == userRole.UserRole.RoleId).FirstOrDefault();
            }

            return View(userRole);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

            ApplicationUser editUser = context.Users.Find(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            return View(editUser);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);
            if (TryUpdateModel(user, "", new string[] { "Email", "UserName" }))
            {
                try
                {
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(user);
        }
        public ActionResult Delete(string id)
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
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser deleteUser = context.Users.Find(id);
            if (deleteUser == null)
            {
                return HttpNotFound();
            }
            return View(deleteUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(string id)
        {
            try
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Remove(user);
                context.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }


        public ActionResult DeleteAll()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.DisplayMenu = "No";

                if (IsAdminUser())
                    ViewBag.DisplayMenu = "Yes";
                else
                    ViewBag.Name = "Not Logged IN";
            }

            IEnumerable<ApplicationUser> allUsers = context.Users;
            if (allUsers == null)
            {
                return HttpNotFound();
            }
            return View(allUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.DisplayMenu = "No";

                if (IsAdminUser())
                    ViewBag.DisplayMenu = "Yes";
                else
                    ViewBag.Name = "Not Logged IN";
            }
            IEnumerable<ApplicationUser> allUsers = context.Users;
            foreach (var user in allUsers)
            {
                if (!IsAdminUser()) context.Users.Remove(user);
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}