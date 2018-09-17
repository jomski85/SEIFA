using SEIFADisadvantage.Models;
using System.Collections.Generic;

namespace SEIFADisadvantage.Services
{
    public interface ISeifaDataService
    {
        SearchInfoResults GetData(SearchInfoParam param);
    }
}