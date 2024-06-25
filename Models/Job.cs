using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Job_Portal.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime PostedDate { get; set; }

        /// Every company has many jobs
        /// <summary>
        /// Gets or sets the ID of the company that posted the job.
        /// </summary>
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        /// Job category has many jobs
        /// <summary>
        /// Gets or sets the ID of the category to which the job belongs.
        /// </summary>
        [ForeignKey("JobCategory")]
        public int JobCategoryId { get; set; }
        public virtual JobCategory JobCategory { get; set; } // Corrected this property name
    }

    public class JobDto
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public int JobCategoryId { get; set; }
        public string JobCategoryName { get; set; }
    }
}