using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Job_Portal.Models;
using System.Web.Script.Serialization;

namespace Job_Portal.Controllers
{
    /// <summary>
    /// The JobController class handles the web-based interactions with job data, including CRUD operations.
    /// </summary>
    public class JobController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JobController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44320/api/jobdata/");
        }

        /// <summary>
        /// Displays a list of all jobs.
        /// </summary>
        /// <returns>A view containing a list of JobDto objects.</returns>
        /// <example>
        /// GET: Job/List
        /// </example>
        public ActionResult List()
        {
            string url = "listjobs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<JobDto> jobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;

            return View(jobs);
        }

        /// <summary>
        /// Displays details of a specific job.
        /// </summary>
        /// <param name="id">The ID of the job to display.</param>
        /// <returns>A view containing the details of the specified job.</returns>
        /// <example>
        /// GET: Job/Details/5
        /// </example>
        public ActionResult Details(int id)
        {
            string url = "findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                JobDto selectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                return View(selectedJob);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays an error page.
        /// </summary>
        /// <returns>An error view.</returns>
        public ActionResult Error()
        {
            return View();
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

        /// <summary>
        /// Displays a form to create a new job.
        /// </summary>
        /// <returns>A view containing the form to create a new job.</returns>
        /// <example>
        /// GET: Job/New
        /// </example>

        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult New()
        {
            ViewBag.Companies = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.JobCategories = new SelectList(db.JobCategories, "JobCategoryId", "Name");

            return View();
        }

        /// <summary>
        /// Creates a new job in the database.
        /// </summary>
        /// <param name="job">The job object to create.</param>
        /// <returns>Redirects to the list of jobs if successful, otherwise redirects to the error page.</returns>
        /// <example>
        /// POST: Job/Create
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Create(Job job)
        {
            GetApplicationCookie();//get token credentials
            string url = "addjob";
            string jsonpayload = jss.Serialize(job);

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

        /// <summary>
        /// Displays a form to edit an existing job.
        /// </summary>
        /// <param name="id">The ID of the job to edit.</param>
        /// <returns>A view containing the form to edit the specified job.</returns>
        /// <example>
        /// GET: Job/Edit/5
        /// </example>
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                JobDto selectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                ViewBag.Companies = new SelectList(db.Companies, "CompanyId", "Name", selectedJob.CompanyId);
                ViewBag.JobCategories = new SelectList(db.JobCategories, "JobCategoryId", "Name", selectedJob.JobCategoryId);
                return View(selectedJob);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Updates an existing job in the database.
        /// </summary>
        /// <param name="id">The ID of the job to update.</param>
        /// <param name="job">The job object containing updated information.</param>
        /// <returns>Redirects to the job details if successful, otherwise redirects to the error page.</returns>
        /// <example>
        /// POST: Job/Update/5
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Job job)
        {
            GetApplicationCookie();//get token credentials
            try
            {
                string url = "UpdateJob/" + id;
                string jsonpayload = jss.Serialize(job);

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
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Displays a confirmation page for deleting a job.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>A view containing the confirmation page for deleting the specified job.</returns>
        /// <example>
        /// GET: Job/DeleteConfirm/5
        /// </example>
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                JobDto selectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                return View(selectedJob);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Deletes a job from the database.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>Redirects to the list of jobs if successful, otherwise redirects to the error page.</returns>
        /// <example>
        /// POST: Job/Delete/5
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "deletejob/" + id;
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
    }
}
