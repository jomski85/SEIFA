using SEIFADisadvantage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIFADisadvantage.Services
{
    public class SearchInfoResults
    {
        public SearchInfoResults()
        {
            Results = new List<SeiafaInfo>();
        }

        public AuState State { get; set; }

        public List<SeiafaInfo> Results { get; set; }
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public bool ShowHigherMedianScore { get; set; }
    }
}
