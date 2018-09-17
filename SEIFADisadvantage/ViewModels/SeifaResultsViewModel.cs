using SEIFADisadvantage.Models;
using SEIFADisadvantage.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEIFADisadvantage.ViewModels
{
    public enum PageSize
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "30")]
        Thirty = 30,

        [Display(Name = "50")]
        Fifty = 50,

        [Display(Name = "100")]
        OneHundred = 100
    }

    public class SeifaResultsViewModel
    {
        public SeifaResultsViewModel()
        {
            Results = new List<SeiafaInfo>();
            Pages = new List<int>();
        }

        public AuState State { get; set; }

        public List<SeiafaInfo> Results { get; set; }
        public int Page { get; set; }

        public PageSize PageSize { get; set; }

        public int TotalItems { get; set; }

        public bool ShowHigherMedianScore { get; set; }

        public List<int> Pages { get; set; }
    }
}
