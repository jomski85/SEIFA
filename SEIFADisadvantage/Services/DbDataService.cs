using SEIFADisadvantage.DataContexts;
using SEIFADisadvantage.Models;
using SEIFADisadvantage.Models._2011;
using SEIFADisadvantage.Models._2016;
using SEIFADisadvantage.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEIFADisadvantage.Services
{
    /// <summary>
    /// Does the bulk of loading the data to the database and collating the different data formats
    /// </summary>
    public class DbDataService : ISeifaDataService
    {
        private SeifaInfoDbContext _dbContext;
        private IDictionary<string, SeiafaInfo> _collatedItems;

        /// <summary>
        /// Constructor that takes in a DbContext to read/write data to/from
        /// </summary>
        /// <param name="dbContext"></param>
        public DbDataService(SeifaInfoDbContext dbContext)
        {
            _collatedItems = new SortedDictionary<string, SeiafaInfo>();
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Data2011.Count() == 0)
                _initialize2011Data();

            if (_dbContext.Data2016.Count() == 0)
                _initialize2016Data();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SearchInfoResults GetData(SearchInfoParam param)
        {
            var resultInfo = new SearchInfoResults();
            resultInfo.Page = param.Page;
            resultInfo.PageSize = param.PageSize;
            resultInfo.State = param.State;

            if (param == null)
                return resultInfo;

            List<SeifaInfo2016> items2016;
            List<SeifaInfo2011> items2011;

            //1. Retrieve places under a specific state
            items2016 = _dbContext.Data2016.Where(item => _isInState(item.LgaCode, param.State))
                                                                    .ToList();

            items2011 = _dbContext.Data2011.Where(item => _isInState(item.State, param.State))
                                                            .ToList();

            //2. Collate Results
            _collateResults(items2011);
            _collateResults(items2016);

            var resultItems = _collatedItems.Values.ToList();
            //now for the median (get the upper half
            if (param.ShowHigherMedianScore && resultItems.Count > 1)
            {
                //Ascending Order
                var descOrderList = resultItems.OrderBy(item => item.TotalScore).ToList();

                //COmpupte the half point index
                int halfLen = descOrderList.Count / 2;
                int startIndex = halfLen;

                //Check if odd or even
                if (descOrderList.Count % 2 > 0)
                    halfLen++;

                //get the greater half
                resultItems = descOrderList.GetRange(startIndex, halfLen);

                //Arrange by name
                resultItems = resultItems.OrderBy(item => item.Name).ToList();
            }

            resultInfo.TotalItems = resultItems.Count;

            if (param.ShowAll)
            {
                resultInfo.Results = resultItems;
            }
            else
            {
                resultInfo.Results = resultItems.Skip(param.Page * param.PageSize).Take(param.PageSize).ToList();
            }

            return resultInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lgaCode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool _isInState(int lgaCode, AuState state)
        {
            if (state == AuState.All)
                return true;

            //we get the first digit of the lgaCode
            while (lgaCode >= 10)
                lgaCode /= 10;

            return (lgaCode == (int)state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemState"></param>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        private bool _isInState(string itemState, AuState stateToCompare)
        {
            if (stateToCompare == AuState.All)
                return true;

            var strState = stateToCompare.GetAttribute<DisplayAttribute>().Name;
            return string.Compare(itemState, strState, true) == 0;
        }

        /// <summary>
        /// Combines 2011 and 2016 results
        /// </summary>
        /// <param name="results"></param>
        private void _collateResults(List<SeifaInfo2011> results)
        {
            foreach (var item in results)
            {
                var newName = _sanitizeName(item.Place);
                if (_collatedItems.ContainsKey(newName))
                {
                    //Do checking here
                    _collatedItems[newName].Score2011 = item.Disadvantage;
                }
                else
                    _collatedItems.Add(newName, new SeiafaInfo() { Name = newName, Score2011 = item.Disadvantage, State =item.State, Score2016 = -1 });
            }
        }

        /// <summary>
        /// Combines 2011 and 2016 results
        /// </summary>
        /// <param name="results"></param>
        private void _collateResults(List<SeifaInfo2016> results)
        {
            foreach (var item in results)
            {
                var newName = _sanitizeName(item.Place);
                if (_collatedItems.ContainsKey(newName))
                {
                    //Do checking here
                    _collatedItems[newName].Score2016 = item.DisadvantageScore;
                }
                else
                    _collatedItems.Add(newName, new SeiafaInfo() { Name = newName, Score2016 = item.DisadvantageScore, State = _getState(item.LgaCode), Score2011 = -1 });
            }
        }

        /// <summary>
        /// Sanitize Place Name (remove the '(X)')
        /// </summary>
        /// <param name="placeName"></param>
        /// <returns></returns>
        private string _sanitizeName(string placeName)
        {
            var foundIndex = placeName.IndexOf('(');

            if (foundIndex > 0)
            {
                var sanitizedStr = placeName.Substring(0, foundIndex);
                sanitizedStr = sanitizedStr.Replace('-', ' ');
                return sanitizedStr.Trim();
            }

            return placeName;
        }

        /// <summary>
        /// Gets the state given the LGA Code. 
        /// </summary>
        /// <param name="LgaCode"></param>
        /// <returns></returns>
        private string _getState(int LgaCode)
        {
            //we get the first digit of the lgaCode
            //The first digit pertains to the State
            while (LgaCode >= 10)
                LgaCode /= 10;

            //Lga Codes are define here
            //http://www.abs.gov.au/ausstats/abs@.nsf/Lookup/by%20Subject/1270.0.55.003~July%202016~Main%20Features~Local%20Government%20Areas%20(LGA)~7
            switch (LgaCode)
            {
                case 1:
                    return "New South Wales";

                case 2:
                    return "Victoria";

                case 3:
                    return "Queensland";

                case 4:
                    return "South Australia";

                case 5:
                    return "Western Australia";

                case 6:
                    return "Tasmania";

                case 7:
                    return "Northern Territory";

                case 8:
                    return "Australian Capital Territory";
                default:
                    return "Others";
            }
        }

        /// <summary>
        /// Populate 2011 Table
        /// </summary>
        private void _initialize2011Data()
        {
            var currentDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var file = string.Format("{0}\\AppData\\SEIFA_2011.csv", currentDir);
            
            string line;
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Get 2011 data
            using (var fileStreamReader = new System.IO.StreamReader(file))
            {
                //This is the header, we read it and move on
                string headerLine = fileStreamReader.ReadLine();
                while ((line = fileStreamReader.ReadLine()) != null)
                {
                    //var attributes = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var attributes = CSVParser.Split(line);
                    if (attributes == null || attributes.Length != 4)
                        continue;

                    try
                    {
                        //Do Parsing here
                        var newItem = new SeifaInfo2011()
                        {
                            State = attributes[0].Trim(),
                            Place = attributes[1].Trim(),
                            Disadvantage = Convert.ToInt32(attributes[2].Trim('\"').Replace(",", string.Empty)),
                            AdvantageDisadvantage = Convert.ToInt32(attributes[3].Trim('\"').Replace(",", string.Empty))
                        };

                        _dbContext.Data2011.Add(newItem);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            _dbContext.SaveChanges();
        }


        /// <summary>
        /// Populate 2016 Table
        /// </summary>
        private void _initialize2016Data()
        {
            var currentDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var file = string.Format("{0}\\AppData\\SEIFA_2016.csv", currentDir);
            
            string line;

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //Get 2016 data
            using (var fileStreamReader = new System.IO.StreamReader(file))
            {
                //This is the header, we read it and move on
                string headerLine = fileStreamReader.ReadLine();

                while ((line = fileStreamReader.ReadLine()) != null)
                {
                    var attributes = CSVParser.Split(line);

                    if (attributes == null || attributes.Length < 11)
                        continue;

                    int value = 0;
                    try
                    {
                        //Do Parsing here
                        var newItem = new SeifaInfo2016()
                        {
                            LgaCode = Convert.ToInt32(attributes[0].Trim('\"').Replace(',', '\0')),
                            Place = attributes[1].Trim(),
                            DisadvantageScore = int.TryParse(attributes[2].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            DisadvantageDecile = int.TryParse(attributes[3].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            AdvantageAndDisadvantageDecile = int.TryParse(attributes[4].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            AdvantageAndDisadvantageScore = int.TryParse(attributes[5].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            EconomicResourcesScore = int.TryParse(attributes[6].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            EconomicResourcesDecile = int.TryParse(attributes[7].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            EducationAndOccupationScore = int.TryParse(attributes[8].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            EducationAndOccupationDecile = int.TryParse(attributes[9].Trim('\"').Replace(",", string.Empty), out value) ? value : -1,
                            Population = int.TryParse(attributes[10].Trim('\"').Replace(",",string.Empty), out value) ? value : -1
                        };

                        _dbContext.Data2016.Add(newItem);
                    }
                    catch
                    {
                        continue;
                    }

                    
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
