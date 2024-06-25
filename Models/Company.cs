using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Job_Portal.Models
{
    public class Company
    {
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        [Key]
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the company.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the collection of jobs posted by the company.
        /// </summary>
        public ICollection<Job> Jobs { get; set; }

    }
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}