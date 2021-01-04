using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcUserAndRoles.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcUserAndRoles.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        ApplicationDbContext context;
        public RoleController()
        {
            context = new ApplicationDbContext();
        }
        public bool IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
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

        // GET All Roles
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            ViewBag.DisplayMenu = "Yes";
            var Roles = context.Roles.ToList();
            return View(Roles);
        }
       
        public ActionResult Create() 
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");

            ViewBag.DisplayMenu = "Yes";
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            
            ViewBag.DisplayMenu = "Yes";
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
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

            IdentityRole role = context.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = context.Roles.Find(id);
            if (TryUpdateModel(role, "", new string[] { "Name" }))
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
            return View(role);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");

            ViewBag.DisplayMenu = "Yes";
            var Role = context.Roles.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(string id)
        {
            try
            {
                IdentityRole role = context.Roles.Find(id);
                context.Roles.Remove(role);
                context.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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