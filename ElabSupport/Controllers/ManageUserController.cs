using ElabSupport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElabSupport.Controllers
{
    public class ManageUserController : Controller
    {
        // GET: ManageUser
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                UserDAC dc = new UserDAC();
                DataTable dt = null;
                dt = dc.GetUserRoles(0);
                // Check if DataTable is not null and has rows
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Convert DataTable to a list of UserRoles
                    List<UseRoleModel> userRoles = new List<UseRoleModel>();
                    foreach (DataRow row in dt.Rows)
                    {
                        userRoles.Add(new UseRoleModel
                        {
                            UserRoleId = row["UserRoleId"] != DBNull.Value ? Convert.ToInt32(row["UserRoleId"]) : 0,
                            UserRole = row["UserRole"] != DBNull.Value ? row["UserRole"].ToString() : string.Empty,
                            UserRoleDescription = row["UserRoleDescription"] != DBNull.Value ? row["UserRoleDescription"].ToString() : string.Empty,
                            Rates = row["Rates"] != DBNull.Value ? Convert.ToDecimal(row["Rates"]) : 0
                        });
                    }

                    return View(userRoles);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the user role by id from your data source (e.g., database)
            UserDAC dc = new UserDAC();
            bool deleted = dc.DeleteUserRole(id.Value);

            if (deleted == false)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddUserRole(string userRole, string userRoleDescription, string rates, int userRoleId=0)
        {
            try
            {
                UseRoleModel userRole1 = new UseRoleModel();
                userRole1.UserRoleId = userRoleId;
                userRole1.UserRole = userRole;
                userRole1.UserRoleDescription = userRoleDescription;
                userRole1.Rates = Convert.ToDecimal(rates);
                // Perform logic to add the new user role to your data source (e.g., database)
                UserDAC dc = new UserDAC();
                int newRoleId = dc.AddUserRole(userRole1);
                if (userRoleId > 0)
                {
                    TempData["ErrorMessage"] = "UserRole Update!...";
                }
                else
                {
                    TempData["ErrorMessage"] = "New UserRole Added!...";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error message
                return View();
            }
        }
    }
}