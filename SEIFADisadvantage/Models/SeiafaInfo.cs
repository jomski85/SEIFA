using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIFADisadvantage.Models
{
    public class SeiafaInfo
    {
        public string State { get; set; }

        public string Name { get; set; }

        public int Score2011 { get; set; }
        public int Score2016 { get; set; }

        public int TotalScore
        {
            get
            {
                var item1 = Score2011 == -1 ? 0 : Score2011;
                var item2 = Score2016 == -1 ? 0 : Score2016;

                return item1 + item2;
            }
        }

        public string Difference
        {
            get
            {
                if (Score2011 == -1 || Score2016 == -1)
                    return "N/A";

                int result = Score2016 - Score2011;
                return result.ToString("+#;-#;0");
            }
        }
    }
}
