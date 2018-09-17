using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIFADisadvantage.Models._2016
{
    public class SeifaInfo2016
    {
        public int Id { get; set; }

        public int LgaCode { get; set; }

        public string Place { get; set; }

        public int DisadvantageScore { get; set; }
        public int DisadvantageDecile { get; set; }

        public int AdvantageAndDisadvantageScore { get; set; }

        public int AdvantageAndDisadvantageDecile { get; set; }

        public int EconomicResourcesScore { get; set; }

        public int EconomicResourcesDecile { get; set; }

        public int EducationAndOccupationScore { get; set; }

        public int EducationAndOccupationDecile { get; set; }

        public int Population { get; set; }
    }
}
