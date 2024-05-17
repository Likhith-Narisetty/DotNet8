using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Mirabel.MM.Configurations;
using Mirabel.MM.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.DataProtection;

namespace Mirabel.MM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        //public HomeController(IDataProtectionProvider dataProtectionProvider)
        //{
        //    _dataProtectionProvider = dataProtectionProvider;
        //}
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            _logger.LogInformation("Testing logging in Program.cs");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region "GetCaptchaImage"
        [Route("GetCaptchaImage")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            string JSON = JsonConvert.SerializeObject(result);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            //return new FileStreamResult(s, "image/png");
            return Ok(JSON);
        }
        #endregion


        #region LoginApi
        [HttpPost]
        public IActionResult LoginApi([FromBody] LoginIP ip)
        {
            string OpjsonString = "";
            ControllerResult FinalResult = new ControllerResult();
            ControllerResponse FinalResponse = new ControllerResponse();

            //var tokenString = JWTtoken.GenerateJSONWebToken();
            try
            {
                var IPjson = JsonConvert.SerializeObject(ip);
                //string OpString = LoginDBCall(IPjson); 
                //var OPjson = JsonConvert.DeserializeObject(OpString); 

                HttpContext.Session.SetString("IsLoggedIn", true.ToString());


                FinalResult.ErrorCode = "200";
                FinalResult.ErrorMsg = "Success";
                FinalResult.ResultSet = null;
                //FinalResponse.Response = OPjson.Errrorcode.tostring();

            }
            catch (Exception ex)
            {
                FinalResult.ErrorCode = "205";
                FinalResult.ErrorMsg = ex.Message;
                FinalResult.ResultSet = null;
            }

            OpjsonString = JsonConvert.SerializeObject(FinalResult);

            return Ok(OpjsonString);
        }

        public string LoginDBCall(string Json)
        {
            string OpjsonString = "";
            try
            {
                DataSet Bdset = new DataSet();
                SqlParameter[] parameters = new SqlParameter[]
               {
                    new SqlParameter("@InputString", Json)
               };
                Bdset = SQLDBHelper.ExecuteDataset_CS1("USP_SampleSP", CommandType.StoredProcedure, parameters);
                if (Bdset.Tables[0].Rows.Count > 0)
                {
                    OpjsonString = Bdset.Tables[0].Rows[0]["Result"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Log.LogService("EmailApi Method:: EmailApi catch Exception: " + Convert.ToString(ex.Message), "Email");
            }
            return OpjsonString;
        }


        #endregion

        #region Dashboard Table Load
        [HttpPost]
        public IActionResult LoadDashboard([FromBody] DashboardIP ip)
        {
            string OpjsonString = "";
            TableResult TableResult = new TableResult();
            try
            {
                var IPjson = JsonConvert.SerializeObject(ip);
                string OpString = LoadDashboardDB(IPjson);

                TableResult.ErrorCode = "200";
                TableResult.ErrorMsg = "Success";
                TableResult.ResultSet = OpString;

            }
            catch (Exception ex)
            {
                TableResult.ErrorCode = "205";
                TableResult.ErrorMsg = ex.Message;
                TableResult.ResultSet = null;
            }

            OpjsonString = JsonConvert.SerializeObject(TableResult);

            return Ok(OpjsonString);
        }

        public string LoadDashboardDB(string Json)
        {
            string OpjsonString = "";
            try
            {
                DataSet Bdset = new DataSet();
                SqlParameter[] parameters = new SqlParameter[]
               {
                    new SqlParameter("@DBName", Json),
                    new SqlParameter("@TableName", Json),
               };
                Bdset = SQLDBHelper.ExecuteDataset_CS1("USP_DbDetailsLoad", CommandType.StoredProcedure, parameters);
                if (Bdset.Tables[0].Rows.Count > 0)
                {
                    OpjsonString = JsonConvert.SerializeObject(Bdset.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                //Log.LogService("EmailApi Method:: EmailApi catch Exception: " + Convert.ToString(ex.Message), "Email");
            }
            return OpjsonString;
        }


        #endregion




        #region DropDown Load
        [HttpPost]
        public IActionResult LoadDropDown()
        {
            string OpjsonString = "{}";
            TableResult TableResult = new TableResult();
            try
            {
                OpjsonString = @"{
                    ""DropDownItems"": [
                        {
                            ""Value"": ""01"",
                            ""label"": ""Option 1""
                        },
                        {
                            ""Value"": ""02"",
                            ""label"": ""Option 2""
                        },
                        {
                            ""Value"": ""03"",
                            ""label"": ""Option 3""
                        }
                    ]
                }";

                TableResult.ErrorCode = "200";
                TableResult.ErrorMsg = "Success";
                TableResult.ResultSet = OpjsonString;

            }
            catch (Exception ex)
            {
                TableResult.ErrorCode = "205";
                TableResult.ErrorMsg = ex.Message;
                TableResult.ResultSet = null;
            }
            return Ok(TableResult);
        }
        #endregion
    }



}







