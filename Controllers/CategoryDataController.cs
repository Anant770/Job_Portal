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

namespace Job_Portal.Controllers
{
    /// <summary>
    /// The CategoryDataController class handles the API interactions for managing job categories.
    /// </summary>
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all job categories.
        /// </summary>
        /// <returns>An IEnumerable of CategoryDto objects.</returns>
        /// <example>
        /// GET: api/CategoryData/ListCategories
        /// </example>
        [Route("api/CategoryData/ListCategories")]
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategories()
        {
            List<JobCategory> categories = db.JobCategories.ToList();
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            categories.ForEach(c => categoryDtos.Add(new CategoryDto()
            {
                JobCategoryId = c.JobCategoryId,
                Name = c.Name
            }));

            return categoryDtos;
        }

        /// <summary>
        /// Retrieves details of a specific job category by ID.
        /// </summary>
        /// <param name="id">The ID of the job category to retrieve.</param>
        /// <returns>An IHttpActionResult containing the CategoryDto object.</returns>
        /// <example>
        /// GET: api/CategoryData/FindCategory/5
        /// </example>
        [Route("api/CategoryData/FindCategory/{id}")]
        [ResponseType(typeof(CategoryDto))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            JobCategory category = db.JobCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryDto categoryDto = new CategoryDto()
            {
                JobCategoryId = category.JobCategoryId,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        /// <summary>
        /// Adds a new job category to the database.
        /// </summary>
        /// <param name="category">The JobCategory object to add.</param>
        /// <returns>An IHttpActionResult containing the added JobCategory object.</returns>
        /// <example>
        /// POST: api/CategoryData/AddCategory
        /// </example>
        [Route("api/CategoryData/AddCategory")]
        [ResponseType(typeof(JobCategory))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddCategory(JobCategory category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobCategories.Add(category);
            db.SaveChanges();

            return Ok(category);
        }

        /// <summary>
        /// Updates an existing job category in the database.
        /// </summary>
        /// <param name="id">The ID of the job category to update.</param>
        /// <param name="category">The JobCategory object containing updated information.</param>
        /// <returns>An IHttpActionResult indicating the status of the operation.</returns>
        /// <example>
        /// POST: api/CategoryData/UpdateCategory/5
        /// </example>
        [Route("api/CategoryData/UpdateCategory/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateCategory(int id, JobCategory category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.JobCategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        /// Deletes a job category from the database.
        /// </summary>
        /// <param name="id">The ID of the job category to delete.</param>
        /// <returns>An IHttpActionResult containing the deleted JobCategory object.</returns>
        /// <example>
        /// POST: api/CategoryData/DeleteCategory/5
        /// </example>
        [Route("api/CategoryData/DeleteCategory/{id}")]
        [ResponseType(typeof(JobCategory))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteCategory(int id)
        {
            JobCategory category = db.JobCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.JobCategories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        /// <param name="disposing">A boolean indicating whether the context is being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks if a job category exists in the database.
        /// </summary>
        /// <param name="id">The ID of the job category to check.</param>
        /// <returns>A boolean indicating whether the job category exists.</returns>
        private bool CategoryExists(int id)
        {
            return db.JobCategories.Count(e => e.JobCategoryId == id) > 0;
        }
    }
}
