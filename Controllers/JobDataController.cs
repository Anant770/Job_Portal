using Job_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;

namespace Job_Portal.Controllers
{
    public class JobDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/JobData/ListJobs
        [Route("api/JobData/ListJobs")]
        [HttpGet]
        public IEnumerable<JobDto> ListJobs()
        {
            List<Job> jobs = db.Jobs.Include(j => j.Company).Include(j => j.JobCategory).ToList();
            List<JobDto> jobDtos = new List<JobDto>();

            jobs.ForEach(j => jobDtos.Add(new JobDto()
            {
                JobId = j.JobId,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                PostedDate = j.PostedDate,
                CompanyId = j.CompanyId,
                CompanyName = j.Company.Name,
                JobCategoryId = j.JobCategoryId,
                JobCategoryName = j.JobCategory.Name
            }));

            return jobDtos;
        }

        // GET: api/JobData/FindJob/5
        [Route("api/JobData/FindJob/{id}")]
        [ResponseType(typeof(Job))]
        [HttpGet]
        public IHttpActionResult FindJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            JobDto jobDto = new JobDto()
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                PostedDate = job.PostedDate,
                CompanyId = job.CompanyId,
                CompanyName = job.Company.Name,
                JobCategoryId = job.JobCategoryId,
                JobCategoryName = job.JobCategory.Name
            };

            return Ok(jobDto);
        }

        // POST: api/JobData/UpdateJob/5
        [Route("api/JobData/UpdateJob/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.JobId)
            {
                return BadRequest();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/JobData/AddJob
        [Route("api/JobData/AddJob")]
        [ResponseType(typeof(Job))]
        [HttpPost]
        public IHttpActionResult AddJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jobs.Add(job);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/JobData/DeleteJob/5
        [Route("api/JobData/DeleteJob/{id}")]
        [ResponseType(typeof(Job))]
        [HttpPost]
        public IHttpActionResult DeleteJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.JobId == id) > 0;
        }
    }
}
