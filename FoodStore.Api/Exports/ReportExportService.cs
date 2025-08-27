using ClosedXML.Excel;
using FoodStore.Api.Helpers;
using FoodStore.Contracts.DTOs.Reports;
using QuestPDF.Fluent;

namespace FoodStore.Api.Exports
{
    public class ReportExportService : IReportExportService
    {
        public Task<byte[]> GenerateSalesByCategoryExcelAsync(IReadOnlyList<SalesByCategoryDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sales By Category");

            // Headers
            worksheet.Cell(1, 1).Value = "Category";
            worksheet.Cell(1, 2).Value = "Quantity";
            worksheet.Cell(1, 3).Value = "Revenue";

            var headerRange = worksheet.Range("A1:C1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.Category;
                worksheet.Cell(row, 2).Value = item.TotalQuantity;
                worksheet.Cell(row, 3).Value = item.Revenue;
                row++;
            }

            // Format Revenue column as currency
            worksheet.Column(3).Style.NumberFormat.Format = "$#,##0.00";

            // Auto fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return Task.FromResult(stream.ToArray());
        }

        public Task<byte[]> GenerateSalesByCategoryPdfAsync(IReadOnlyList<SalesByCategoryDto> data)
        {
            var document = new SalesByCategoryDocument(data);
            var pdfBytes = document.GeneratePdf();   // extension method
            return Task.FromResult(pdfBytes);
        }
    }
}