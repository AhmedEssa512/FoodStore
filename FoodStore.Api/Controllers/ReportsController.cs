using FoodStore.Api.Exports;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Reports;
using FoodStore.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IReportExportService _reportExportService;
        public ReportsController(IReportService reportService, IReportExportService reportExportService)
        {
            _reportService = reportService;
            _reportExportService = reportExportService;
        }

        [HttpGet("sales-by-category")]
        public async Task<IActionResult> GetSalesByCategory()
        {
            var result = await _reportService.GetSalesByCategoryAsync();
            return Ok(ApiResponse<IReadOnlyList<SalesByCategoryDto>>.Ok(result));
        }

        [HttpGet("top-foods")]
        public async Task<IActionResult> GetTopFoods([FromQuery] int topN = 5)
        {
            var result = await _reportService.GetTopFoodsAsync(topN);
            return Ok(ApiResponse<IReadOnlyList<TopFoodDto>>.Ok(result));
        }

        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary()
        {
            var result = await _reportService.GetSalesSummaryAsync();
            return Ok(ApiResponse<SalesSummaryDto>.Ok(result));
        }


          // ------------------- Excel / PDF Endpoints -------------------

        [HttpGet("sales-by-category/excel")]
        public async Task<IActionResult> ExportSalesByCategoryExcel()
        {
            var data = await _reportService.GetSalesByCategoryAsync();
            var fileBytes = await _reportExportService.GenerateSalesByCategoryExcelAsync(data);

            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"sales-by-category-{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        [HttpGet("sales-by-category/pdf")]
        public async Task<IActionResult> ExportSalesByCategoryPdf()
        {
            var data = await _reportService.GetSalesByCategoryAsync();
            var fileBytes = await _reportExportService.GenerateSalesByCategoryPdfAsync(data);

            return File(fileBytes,
                "application/pdf",
                $"sales-by-category-{DateTime.UtcNow:yyyyMMdd}.pdf");
        }
    }
}