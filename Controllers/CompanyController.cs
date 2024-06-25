using Job_Portal.Models;
using Job_Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Job_Portal.Controllers
{
    public class CompanyController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CompanyController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }

        // GET: Company/List
        public ActionResult List()
        {
            string url = "companydata/listcompanies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CompanyDto> companies = response.Content.ReadAsAsync<IEnumerable<CompanyDto>>().Result;

            return View(companies);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            DetailsCompany ViewModel = new DetailsCompany();
            string url = "companydata/findcompany/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CompanyDto SelectedCompany = response.Content.ReadAsAsync<CompanyDto>().Result;

            ViewModel.SelectedCompany = SelectedCompany;
            url = "jobdata/listjobsforcompany" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<JobDto> RelatedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;

            ViewModel.RelatedJobs = RelatedJobs;

            return View(ViewModel);
            //return View(SelectedCompany);

        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Company/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();//get token credentials
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Company company)
        {
            GetApplicationCookie();//get token credentials
            string url = "companydata/addcompany";

            string jsonpayload = jss.Serialize(company);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Company/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "companydata/findcompany/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyDto selectedCompany = response.Content.ReadAsAsync<CompanyDto>().Result;
                return View(selectedCompany);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Company/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Company company)
        {
            GetApplicationCookie();//get token credentials
            string url = "companydata/updatecompany/" + id;

            string jsonpayload = jss.Serialize(company);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Company/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "companydata/findcompany/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyDto selectedCompany = response.Content.ReadAsAsync<CompanyDto>().Result;
                return View(selectedCompany);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Company/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "companydata/deletecompany/" + id;
            HttpResponseMessage response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}