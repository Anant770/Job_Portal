using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Job_Portal.Models
{
    public class JobCategory
    {
        /// <summary>
        /// Gets or sets the job category ID.
        /// </summary>
        [Key]
        public int JobCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the job category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of jobs that belong to this category.
        /// </summary>
        public virtual ICollection<Job> Jobs { get; set; }
    }
}