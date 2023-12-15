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
    [Route("api/[controller]")]
    public class APIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("GetLogin")]
        public async Task<ActionResult> GetLogin([FromBody] LoginRequest model)
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
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}