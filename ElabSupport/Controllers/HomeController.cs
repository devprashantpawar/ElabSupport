using ElabSupport.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElabSupport.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                UserDAC dc = new UserDAC();
                DateTime currentDate = DateTime.Now;
                DateTime fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime toDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                ViewData["fromDate"] = fromDate.ToString("yyyy-MM-dd");
                ViewData["toDate"] = toDate.ToString("yyyy-MM-dd");

                List<SupportData> supportPersons = dc.GetSupportData(fromDate,toDate);

                return View(supportPersons);
               
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
        }
        
        public ActionResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if (file != null)   
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                if(file!=null && file.ContentLength>0)
                { 
                    try
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            var colCount = worksheet.Dimension.Columns;

                            var excelDataList = new List<ExcelData>();

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var dateCellValue = worksheet.Cells[row, 1].Value?.ToString();
                                if (!string.IsNullOrEmpty(dateCellValue))
                                {
                                    var supportPerson = new List<string>();
                                    for (int col = 2; col <= colCount; col++)
                                    {
                                        var supportPersonCellValue = worksheet.Cells[row, col].Value?.ToString();
                                        supportPerson.Add(supportPersonCellValue);
                                    }

                                    var excelData = new ExcelData
                                    {
                                        Date = DateTime.Parse(dateCellValue),
                                        SupportPersons = supportPerson
                                    };

                                    excelDataList.Add(excelData);
                                }
                            }

                            UserDAC dc = new UserDAC();
                             int uploaded = dc.UploadExcelData(excelDataList);

                            TempData["ErrorMessage"] = "File uploaded successfully!";
                            return View("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Error: " + ex.Message;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please choose a file.";
                }

            }
            return View("Index");
        }

        
        public ActionResult FilterOOSData(DateTime fromDate, DateTime toDate)
        {
            UserDAC dc = new UserDAC();

            // Use fromDate and toDate parameters to filter data
            List<SupportData> supportPersons = dc.GetSupportData(fromDate,toDate);
            ViewData["fromDate"] = fromDate.ToString("yyyy-MM-dd");
            ViewData["toDate"] = toDate.ToString("yyyy-MM-dd");

            // Pass the filtered data to the view
            return View("_Home",supportPersons);
        }

    }
}
