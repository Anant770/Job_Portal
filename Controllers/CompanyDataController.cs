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
    /// The CompanyDataController class handles the API interactions for managing company data.
    /// </summary>
    public class CompanyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all companies.
        /// </summary>
        /// <returns>An IEnumerable of CompanyDto objects.</returns>
        /// <example>
        /// GET: api/CompanyData/ListCompanies
        /// </example>
        [Route("api/CompanyData/ListCompanies")]
        [HttpGet]
        public IEnumerable<CompanyDto> ListCompanies()
        {
            List<Company> companies = db.Companies.ToList();
            List<CompanyDto> companyDtos = new List<CompanyDto>();

            companies.ForEach(c => companyDtos.Add(new CompanyDto()
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                Address = c.Address
            }));

            return companyDtos;
        }

        /// <summary>
        /// Retrieves details of a specific company by ID.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>An IHttpActionResult containing the CompanyDto object.</returns>
        /// <example>
        /// GET: api/CompanyData/FindCompany/5
        /// </example>
        [Route("api/CompanyData/FindCompany/{id}")]
        [ResponseType(typeof(CompanyDto))]
        [HttpGet]
        public IHttpActionResult FindCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            CompanyDto companyDto = new CompanyDto()
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Address = company.Address
            };

            return Ok(companyDto);
        }

        /// <summary>
        /// Adds a new company to the database.
        /// </summary>
        /// <param name="company">The Company object to add.</param>
        /// <returns>An IHttpActionResult containing the added Company object.</returns>
        /// <example>
        /// POST: api/CompanyData/AddCompany
        /// </example>
        [Route("api/CompanyData/AddCompany")]
        [ResponseType(typeof(Company))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companies.Add(company);
            db.SaveChanges();

            return Ok(company);
        }

        /// <summary>
        /// Updates an existing company in the database.
        /// </summary>
        /// <param name="id">The ID of the company to update.</param>
        /// <param name="company">The Company object containing updated information.</param>
        /// <returns>An IHttpActionResult indicating the status of the operation.</returns>
        /// <example>
        /// POST: api/CompanyData/UpdateCompany/5
        /// </example>
        [Route("api/CompanyData/UpdateCompany/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateCompany(int id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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
        /// Deletes a company from the database.
        /// </summary>
        /// <param name="id">The ID of the company to delete.</param>
        /// <returns>An IHttpActionResult containing the deleted Company object.</returns>
        /// <example>
        /// POST: api/CompanyData/DeleteCompany/5
        /// </example>
        [Route("api/CompanyData/DeleteCompany/{id}")]
        [ResponseType(typeof(Company))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            db.SaveChanges();

            return Ok(company);
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
        /// Checks if a company exists in the database.
        /// </summary>
        /// <param name="id">The ID of the company to check.</param>
        /// <returns>A boolean indicating whether the company exists.</returns>
        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.CompanyId == id) > 0;
        }
    }
}
