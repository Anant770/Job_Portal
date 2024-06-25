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
    /// <summary>
    /// The JobDataController class handles the API endpoints for managing job data.
    /// </summary>
    public class JobDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of all jobs, including their associated companies and job categories.
        /// </summary>
        /// <returns>A list of JobDto objects representing all jobs.</returns>
        /// <example>
        /// GET: api/JobData/ListJobs
        /// </example>
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
                CompanyAddress = j.Company.Address,
                JobCategoryId = j.JobCategoryId,
                JobCategoryName = j.JobCategory.Name
            }));

            return jobDtos;
        }

        /// <summary>
        /// Finds a specific job by its ID.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>A JobDto object representing the job.</returns>
        /// <example>
        /// GET: api/JobData/FindJob/5
        /// </example>
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
                CompanyAddress = job.Company.Address,
                JobCategoryId = job.JobCategoryId,
                JobCategoryName = job.JobCategory.Name
            };

            return Ok(jobDto);
        }

        /// <summary>
        /// Gathers information about jobs related to a particular company.
        /// </summary>
        /// <param name="id">Company ID.</param>
        /// <returns>A list of JobDto objects representing jobs for the specified company.</returns>
        /// <example>
        /// GET: api/JobData/ListJobsForCompany/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(JobDto))]
        [Route("api/JobData/ListJobsForCompany/{id}")]
        public IHttpActionResult ListJobsForCompany(int id)
        {
            List<Job> jobs = db.Jobs.Include(j => j.Company).Include(j => j.JobCategory)
                                    .Where(j => j.CompanyId == id).ToList();
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
                CompanyAddress = j.Company.Address,
                JobCategoryId = j.JobCategoryId,
                JobCategoryName = j.JobCategory.Name
            }));

            return Ok(jobDtos);
        }

        /// <summary>
        /// Gathers information about jobs related to a particular job category.
        /// </summary>
        /// <param name="id">Job Category ID.</param>
        /// <returns>A list of JobDto objects representing jobs for the specified job category.</returns>
        /// <example>
        /// GET: api/JobData/ListJobsForCategory/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(JobDto))]
        [Route("api/JobData/ListJobsForCategory/{id}")]
        public IHttpActionResult ListJobsForCategory(int id)
        {
            List<Job> jobs = db.Jobs.Include(j => j.Company).Include(j => j.JobCategory)
                            .Where(j => j.JobCategoryId == id).ToList();
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

            return Ok(jobDtos);
        }

        /// <summary>
        /// Updates a specific job.
        /// </summary>
        /// <param name="id">The ID of the job to update.</param>
        /// <param name="job">The job object containing updated information.</param>
        /// <returns>HTTP status codes indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/JobData/UpdateJob/5
        /// </example>
        [Route("api/JobData/UpdateJob/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Adds a new job to the database.
        /// </summary>
        /// <param name="job">The job object to add.</param>
        /// <returns>HTTP status codes indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/JobData/AddJob
        /// </example>
        [Route("api/JobData/AddJob")]
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Deletes a specific job from the database.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>HTTP status codes indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/JobData/DeleteJob/5
        /// </example>
        [Route("api/JobData/DeleteJob/{id}")]
        [ResponseType(typeof(Job))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether to release both managed and unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if a job exists in the database.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>A boolean value indicating whether the job exists.</returns>
        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.JobId == id) > 0;
        }
    }
}
