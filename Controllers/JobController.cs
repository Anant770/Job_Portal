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
    public class JobController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JobController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/jobdata/");
        }

        // GET: Job/List
        public ActionResult List()
        {
            //objective: communicate with our job data api to retrieve a list of jobs
            //curl https://localhost:44324/api/jobdata/listjobs


            string url = "listjobs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<JobDto> jobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
            //Debug.WriteLine("Number of jobs received : ");
            //Debug.WriteLine(jobs.Count());


            return View(jobs);
        }

        // GET: Job/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our job data api to retrieve one job
            //curl https://localhost:44324/api/jobdata/findjob/{id}

            string url = "findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            JobDto selectedjob = response.Content.ReadAsAsync<JobDto>().Result;
            Debug.WriteLine("job received : ");
            Debug.WriteLine(selectedjob.Description);


            return View(selectedjob);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Job/New
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult New()
        {
            ViewBag.Companies = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.JobCategories = new SelectList(db.JobCategories, "JobCategoryId", "Name");

            return View();
        }

        // POST: Job/Create
        [HttpPost]
        public ActionResult Create(Job job)
        {
            Debug.WriteLine("The JSON payload is:");
            string url = "addjob";

            string jsonpayload = jss.Serialize(job);

            Debug.WriteLine(jsonpayload);

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

        // GET: Job/Edit/5
        public ActionResult Edit(int id)
        {
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

        // POST: Job/Update/5
        [HttpPost]
        public ActionResult Update(int id, Job job)
        {
            try
            {
                Debug.WriteLine("The new job info is:");
                Debug.WriteLine(job.JobId);
                Debug.WriteLine(job.Description);

                string url = "UpdateJob/" + id;

                string jsonpayload = jss.Serialize(job);
                Debug.WriteLine(jsonpayload);

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


        // GET: Job/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
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
        // POST: Job/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
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