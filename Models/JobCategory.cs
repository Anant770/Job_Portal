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
        public string Name { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
    public class CategoryDto
    {
        public int JobCategoryId { get; set; }
        public string Name { get; set; }
    }
}