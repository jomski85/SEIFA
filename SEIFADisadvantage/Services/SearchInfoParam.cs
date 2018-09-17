using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEIFADisadvantage.Services
{
    public enum AuState
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "New South Wales")]
        NSW = 1,

        [Display(Name = "Victoria")]
        VIC = 2,

        [Display(Name = "Queensland")]
        QLD = 3,

        [Display(Name = "South Australia")]
        SA = 4,

        [Display(Name = "Western Australia")]
        WA = 5,

        [Display(Name = "Tasmania")]
        TAS = 6,

        [Display(Name = "Northern Territory")]
        NT = 7,

        [Display(Name = "Australian Central Territory")]
        ACT = 8,

        [Display(Name = "Others")]
        Others = 9
    }

    public class SearchInfoParam
    {
        public AuState State { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool ShowAll { get; set; }

        public bool ShowHigherMedianScore { get; set; }
    }
}
