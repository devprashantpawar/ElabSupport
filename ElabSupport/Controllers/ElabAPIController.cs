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
                string jsonData = JsonConvert.SerializeObject(Data);
                jsonData = jsonData.Replace("\\", "");
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

        }
        
        [System.Web.Http.HttpGet]
        public async Task<ActionResult> GetSupportData(string UserName)
        {
            UserDAC uc = new UserDAC();
            List<SupportData> data = uc.GetSupportData();
            
                var Data = await ec.GetUserData(userid);
                string jsonData = JsonConvert.SerializeObject(Data);
                jsonData = jsonData.Replace("\\", "");
                return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }

}