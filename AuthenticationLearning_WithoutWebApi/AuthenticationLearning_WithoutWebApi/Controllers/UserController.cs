using AuthenticationLearning_WithoutWebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PMSProxy.FetchMetadata;
using System;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace AuthenticationLearning_WithoutWebApi.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [Authorize]
        public ActionResult Index()
        {            
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
        }

        [HttpPost]
        public string LoadMenuDetails()
        {
            StringBuilder MenuDOM = new StringBuilder();
            if (User.Identity.IsAuthenticated)
            {
                string TenantId = TenantDetailsProxy.FetchTenantId();
                DataSet ModuleDisplayDetails = ModuleDisplayDetailsProxy.FetchModuleDisplayDetails(TenantId, User.Identity.GetUserId());
                DataTable MenuDetailsTable = ModuleDisplayDetails.Tables[0].DefaultView.ToTable(); 
                if(MenuDetailsTable != null)
                {
                    foreach (DataRow menuRow in MenuDetailsTable.Rows)
                    {
                        MenuDOM.Append("<li><a href=\"" + menuRow["ModuleURL"] + "\">" + menuRow["ModuleDisplayName"] + "</a>");
                    }
                }
                return MenuDOM.ToString();
            }
            else
            {
                return MenuDOM.ToString();
            }
        }

        private Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {                
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

    }
}