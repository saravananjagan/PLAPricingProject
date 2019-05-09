using AuthenticationLearning_WithoutWebApi.Models;
using LinqToExcel;
using PMSProxy.Pricing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationLearning_WithoutWebApi.Controllers
{
    public class ManagePricingController : Controller
    {
        // GET: ManagePricing
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public JsonResult UploadPricing(PricingList pricingList, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            if (FileUpload != null)
            { 
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var pricingLists = from a in excelFile.Worksheet<PricingList>(sheetName) select a;
                    StringBuilder ImportValues = new StringBuilder();

                    foreach (var a in pricingLists)
                    {
                        try
                        {
                            if (a.ProductId!="")
                            {
                                PricingList PL = new PricingList();
                                PL.ProductId = a.ProductId;
                                PL.ProductName = a.ProductName;
                                PL.BuyPrice = a.BuyPrice;
                                PL.SellPrice = a.SellPrice;
                                PL.Profit = a.Profit;
                                PL.MRP = a.MRP;
                                string value = "('"+Guid.NewGuid()+"','"+PL.ProductId+"','"+PL.ProductName+"','"+PL.BuyPrice+"','"+PL.SellPrice+"','"+PL.Profit+"','"+PL.MRP+"',1,getdate(),suser_sname(),null,null),";
                                ImportValues.Append(value);
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    string ImportData = ImportValues.ToString();
                    ImportData=ImportData.Substring(0, ImportData.Length - 1);
                    PricingDetailsProxy.InsertBulkPricingDetails(ImportData);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult ImportPricingDetails(PricingList pricingList, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var pricingLists = from a in excelFile.Worksheet<PricingList>(sheetName) select a;
                    StringBuilder ImportValues = new StringBuilder();

                    foreach (var a in pricingLists)
                    {
                        try
                        {
                            //if (a.ProductName != "" && a.ProductPrice != "")
                            //{
                            //    PricingList PL = new PricingList();
                            //    PL.ProductName = a.ProductName;
                            //    PL.ProductPrice = a.ProductPrice;
                            //    PL.ProductDescription = a.ProductDescription;
                            //    string value = "('"+PL.ProductName+"','"+PL.ProductPrice+"','"+PL.ProductDescription+"'),";
                            //    ImportValues.Append(value);
                            //}
                            //else
                            //{
                            //    data.Add("<ul>");
                            //    if (a.ProductName == "" || a.ProductName == null) data.Add("<li> name is required</li>");
                            //    if (a.ProductPrice == "" || a.ProductPrice == null) data.Add("<li> ProductPrice is required</li>");                                

                            //    data.Add("</ul>");
                            //    data.ToArray();
                            //    return Json(data, JsonRequestBehavior.AllowGet);
                            //}

                        }

                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    string ImportData = ImportValues.ToString();
                    ImportData = ImportData.Substring(0, ImportData.Length - 1);
                    PricingDetailsProxy.InsertBulkPricingDetails(ImportData);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
    }       
}
