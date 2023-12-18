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
                            Rates = row["Rates"] != DBNull.Value ? Convert.ToDecimal(row["Rates"]) : 0,
                            Shift1 = row["Shift1"] != DBNull.Value ? Convert.ToInt32(row["Shift1"]) : 0,
                            Shiftboth = row["Shiftboth"] != DBNull.Value ? Convert.ToInt32(row["Shiftboth"]) : 0
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
        public ActionResult AddUserRole(string userRole, string userRoleDescription, string rates, string shift1,string Shiftboth, int userRoleId=0)
        {
            try
            {
                UseRoleModel userRole1 = new UseRoleModel();
                userRole1.UserRoleId = userRoleId;
                userRole1.UserRole = userRole;
                userRole1.UserRoleDescription = userRoleDescription;
                userRole1.Rates = Convert.ToDecimal(rates);
                userRole1.Shift1 = Convert.ToInt32(shift1);
                userRole1.Shiftboth = Convert.ToInt32(Shiftboth);
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

        public ActionResult AddUser()
        {
            // Your logic for the AddUser action goes here
            // You can return a view or perform any other action
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
                    return View();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult AddUser(UserModel newUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDAC dc = new UserDAC();
                    int newuserId = dc.AddUser(newUser);
                    if (newuserId == 1)
                    {
                        TempData["ErrorMessage"] = "User Added!...";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "New User Updated!...";
                    }
                    return RedirectToAction("AddUser");
                }
                catch (Exception ex)
                {
                    // Handle exceptions and return an error message
                    return View();
                }
                return RedirectToAction("AddUser");
            }

            // If the model is not valid, return to the same view with validation errors
            return View(newUser);
        }

        public ActionResult Account()
        {
            // Your logic for the AddUser action goes here
            // You can return a view or perform any other action
            if (Session["UserID"] != null)
            {
                UserDAC dc = new UserDAC();
                DateTime currentDate = DateTime.Now;
                DateTime fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime toDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                ViewData["fromDate"] = fromDate.ToString("yyyy-MM-dd");
                ViewData["toDate"] = toDate.ToString("yyyy-MM-dd");

                List<AccountSupportData> AccountSupportData = dc.GetAccountSupportData(fromDate,toDate);

                return View(AccountSupportData);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
       public ActionResult FilterByDate(DateTime? fromDate, DateTime? toDate)
        {
            UserDAC dc = new UserDAC();

            // Use fromDate and toDate parameters to filter data
            List<AccountSupportData> filteredData = dc.GetAccountSupportData(fromDate, toDate);

            // Pass the filtered data to the view
            return View(filteredData);
        }
    }
}