using System;
using System.Configuration;
using System.Text;
using System.Web;
using GoFish.Library;
using System.Web.Mvc;
using System.Web.Security;
using static System.String;

namespace GoFish.Controllers
{
    public class SearchController : Controller
    {
        private const string CookieName = "FlowdockToken";

        private string _apiKey;

        public ActionResult Index()
        {
            _apiKey = GetCookie();

            if (IsNullOrEmpty(_apiKey))
            {
                _apiKey = ConfigurationManager.AppSettings["FlowdockAPIKey"];
            }

            ViewBag.ApiKey = ObfuscateString(_apiKey);

            return View();
        }

        [HttpPost]
        public ActionResult SearchFlows(string apiKey, string searchText)
        {
            apiKey = GetApiKey(apiKey);

            var flowdock = new Flowdock(apiKey);

            var results = flowdock.Search(IsNullOrEmpty(searchText) ? "fish" : searchText);

            SetCookie(apiKey);

            return Json(results);
        }

        private string GetApiKey(string apiKey)
        {
            var suppliedApiKey = apiKey;
            var cookiedApiKey = GetCookie();
            var configApiKey = ConfigurationManager.AppSettings["FlowdockAPIKey"];

            if (!IsNullOrEmpty(apiKey))
            {
                apiKey = suppliedApiKey;
            }
            else if (!IsNullOrEmpty(cookiedApiKey))
            {
                apiKey = cookiedApiKey;
            }
            else
            {
                apiKey = configApiKey;
            }
            return apiKey;
        }

        private string GetCookie()
        {
            return Unprotect(ControllerContext.HttpContext.Request.Cookies[CookieName]?.Value);
        }

        private void SetCookie(string apiKey)
        {
            if (!IsNullOrEmpty(apiKey))
            {
                var cookie = new HttpCookie("FlowdockToken") { Value = Protect(apiKey) };

                ControllerContext.HttpContext.Response.SetCookie(cookie);
            }
        }

        public static string Protect(string text)
        {
            if (IsNullOrEmpty(text))
                return null;

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(MachineKey.Protect(bytes, CookieName));
        }

        public static string Unprotect(string text)
        {
            if (IsNullOrEmpty(text))
                return null;

            byte[] bytes = Convert.FromBase64String(text);
            var output = MachineKey.Unprotect(bytes, CookieName);
            return Encoding.UTF8.GetString(output);
        }

        private string ObfuscateString(string input)
        {
            if (IsNullOrEmpty(input))
                return input;

            string startChars = input.Substring(0, 2);
            string middlePart = Empty.PadLeft(input.Length - 4, '•');
            string endChars = input.Substring(input.Length - 2, 2);

            return Concat(startChars, middlePart, endChars);
        }
    }
}