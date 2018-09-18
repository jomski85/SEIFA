using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SEIFADisadvantage.Services;
using SEIFADisadvantage.ViewModels;

namespace SEIFADisadvantage.Controllers
{
    public class HomeController : Controller
    {
        private ISeifaDataService _dataProvider;

        public HomeController(ISeifaDataService dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            //Let's set up our default parameters
            var param = new SearchInfoParam()
            {
                State = AuState.VIC,
                Page = 0,
                PageSize = 50,
                ShowAll =false,
                ShowHigherMedianScore =true
            };

            //Call our service
            var items = _dataProvider.GetData(param);

            //Transfer back the results to our View model
            var vm = new SeifaResultsViewModel()
            {
                State = AuState.VIC,
                Page = 0,
                PageSize = PageSize.Fifty,
                ShowHigherMedianScore = param.ShowHigherMedianScore,
                Results = items.Results,
                TotalItems = items.TotalItems,
            };

            //Compute the number of pages
            var totalPages = vm.TotalItems / (int)vm.PageSize;

            if (totalPages % (int)vm.PageSize > 0)
                totalPages++;

            for (int i = 0; i < totalPages; i++)
                vm.Pages.Add(i+1);

            //Display
            return View(vm);
        }

        [HttpPost]
        public IActionResult SearchResults(SeifaResultsViewModel param)
        {
            //Transfer the search param from the view model to the proper parameter
            var searchParam = new SearchInfoParam()
            {
                State = param.State,
                Page = param.Page-1,
                PageSize = param.PageSize == PageSize.All ? 2000 : (int)param.PageSize,
                ShowAll = param.PageSize == PageSize.All,
                ShowHigherMedianScore = param.ShowHigherMedianScore
            };

            //Call our service
            var items = _dataProvider.GetData(searchParam);

            //Transfer back the result to our view model
            var vm = new SeifaResultsViewModel()
            {
                State = param.State,
                Page = param.Page,
                PageSize = param.PageSize,
                ShowHigherMedianScore = param.ShowHigherMedianScore,
                Results = items.Results,
                TotalItems = items.TotalItems
            };

            //COmpute for the number of pages
            if (param.PageSize != PageSize.All)
            {
                var totalPages = vm.TotalItems / (int)vm.PageSize;

                if (totalPages % (int)vm.PageSize > 0)
                    totalPages++;

                for (int i = 0; i < totalPages; i++)
                    vm.Pages.Add(i + 1);
            }

            //Display
            return View("Index",vm);
        }
    }
}
