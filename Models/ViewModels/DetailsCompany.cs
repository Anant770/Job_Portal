using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Job_Portal.Models.ViewModels
{
    public class DetailsCompany
    {
        public CompanyDto SelectedCompany { get; set; }
        public IEnumerable<JobDto> RelatedJobs { get; set; }
    }
}