using _038_ETradeCoreLite.Areas.Reports.Models;
using _038_ETradeCoreLite.Settings;
using AppCore.DataAccess.Models;
using DataAccess.Models;
using DataAccess.Services;
using DataAccess.Services.CRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _038_ETradeCoreLite.Areas.Reports.Controllers
{
    [Area("Reports")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly CategoryServiceBase _categoryService;
        private readonly StoreServiceBase _storeService;

        public HomeController(IReportService reportService, CategoryServiceBase categoryService, StoreServiceBase storeService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _storeService = storeService;
        }

        public IActionResult Index(ReportsIndexViewModel viewModel)
        {
            viewModel.Filter = viewModel.Filter ?? new ReportFilterModel();
            viewModel.Page = viewModel.Page ?? new PageModel()
            {
                PageNumber = 1,
                RecordsPerPageCount = 5
            };
            viewModel.Order = viewModel.Order ?? new OrderModel()
            {
                IsDirectionAscending = true
            };
            viewModel.Reports = _reportService.GetList(viewModel.Filter, viewModel.Page, viewModel.Order);
            viewModel.Categories = new SelectList(_categoryService.GetList(), "Id", "Name");
            viewModel.Stores = new MultiSelectList(_storeService.GetList(), "Id", "Name");
            viewModel.Pages = new SelectList(viewModel.Page.PageNumberList);
            viewModel.RecordsPerPageCounts = new SelectList(viewModel.Page.RecordsPerPageCountList);
            viewModel.OrderExpressions = new SelectList(_reportService.GetOrderExpressions());
            return View(viewModel);
        }
    }
}
