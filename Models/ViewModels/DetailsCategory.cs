using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Job_Portal.Models.ViewModels
{
    public class DetailsCategory
    {
        public CategoryDto SelectedCategory { get; set; }
        public IEnumerable<JobDto> RelatedJobs { get; set; }
    }
}