using ElabSupport.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ElabSupport.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    public class ElabAPIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
            return View();
        }
        [System.Web.Http.HttpPost]
        public async Task<ActionResult> GetLogin([FromBody] LoginModel model)
        {
            UserDAC uc = new UserDAC();
            DataTable data = null;
            data = uc.GetLogin(model.UserName, model.Password);
            if (data != null && data.Rows.Count != 0)
            {
                string userid = data.Rows[0].Field<string>("UserId").ToString();
                ExotelController ec = new ExotelController();
                var Data = await ec.GetUserData(userid);
                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(Data);
                    string userData = JsonConvert.SerializeObject(data);
                    var combinedData = new
                    {
                        UserData = userData,
                        exotelData = jsonData
                    };
                    string combinedJsonData = JsonConvert.SerializeObject(combinedData);
                    combinedJsonData = combinedJsonData.Replace("\\", "");
                    return Json(combinedJsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string userData = JsonConvert.SerializeObject(data);
                    var combinedData = new
                    {
                        UserData = userData,
                        exotelData = ""
                    };
                    string combinedJsonData = JsonConvert.SerializeObject(combinedData);
                    combinedJsonData = combinedJsonData.Replace("\\", "");
                    return Json(combinedJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

        }
        
        [System.Web.Http.HttpGet]
        public ActionResult GetUserSupportData(string UserName)
        {
            UserDAC uc = new UserDAC();
            DataTable data = null;
            string jsonData = "";
            data = uc.GetUserSupportData();
            if (data!=null && data.Rows.Count > 0)
            {
                // Convert DataTable to JSON using Newtonsoft.Json
                jsonData = JsonConvert.SerializeObject(data);
                jsonData = jsonData.Replace("\\", "");
                // Return JSON response
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "No data available" }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPut]
        public async Task<ActionResult> UpdateUserStatus([FromBody] UpdateStatusRequest model)
        {
            model.DeviceId = "490038";
            model.UserId = "fa1d03deee38403c96917513a7affaf8";
            model.Status = true;
            ExotelController ec = new ExotelController();
            var Data = await ec.UpdateDeviceAvailability(model.UserId, model.Status, model.DeviceId);
            string jsonData = JsonConvert.SerializeObject(Data);
            jsonData = jsonData.Replace("\\", "");
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        
        [System.Web.Http.HttpPost]
        public ActionResult UpdateStatus([FromBody] UpdateuserStatus model)
        {
            
            UserDAC ec = new UserDAC();
            bool status = ec.UpdateUserStatus(model.UserId, model.Status);
            return Json(status, JsonRequestBehavior.AllowGet);

        }

    }

}

