using ConcretAssignment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ConcretAssignment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GitCallBack()
        {

            var appUrl = (Request.Url.GetLeftPart(UriPartial.Authority) + (Request.ApplicationPath == "/" ? Request.ApplicationPath : Request.ApplicationPath + "/") + "home/gitcallback").ToLower();

            GitTokenRequest gitTokenRequest = new GitTokenRequest
            {
                Code = Request.QueryString["code"],
                RedirectUri = appUrl,
                ClientId = ConfigurationManager.AppSettings["ClientID"],
                ClientSecret = ConfigurationManager.AppSettings["ClientSecret"]
            };
            var postdata = JsonConvert.SerializeObject(gitTokenRequest);

            var response = ApiPost("https://github.com/login/oauth/access_token", postdata);

            if (response != null)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                HttpCookie accessTokenCookie = new HttpCookie("git_token");
                accessTokenCookie.Value = loginResponse.AccessToken;
                accessTokenCookie.Expires = DateTime.Now.AddHours(5);
                HttpContext.Response.Cookies.Add(accessTokenCookie);
                return RedirectToAction("Gists");
            }
            ViewBag.message = "Unable to Login.";
            return RedirectToAction("Index");
        }

        public ActionResult Gists()
        {
            var getToken = Request.Cookies.Get("git_token");
            List<UserGist> userGists = new List<UserGist>();
            try
            {
                var token = Request.Cookies.Get("git_token").Value;
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "bearer " + token);
                var getdata = ApiGet("https://api.github.com/gists", headers);
                userGists= JsonConvert.DeserializeObject<List<UserGist>>(getdata);
                return View(userGists);
            }
            catch {

            }
            return View(new List<UserGist>());
        }

        public string ApiGet(string url, Dictionary<string, string> headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = ConfigurationManager.AppSettings["AppName"];
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.ContentType = "application/json";
            request.Accept = "application/json";
            var response = request.GetResponse();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }


        private string ApiPost(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            if (data != null)
            {
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(data);
                    sw.Flush();
                }
            }
            var response = request.GetResponse();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }

    }
}