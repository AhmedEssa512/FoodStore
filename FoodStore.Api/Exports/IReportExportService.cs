using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.DTOs.Reports;

namespace FoodStore.Api.Exports
{
    public interface IReportExportService
    {
        Task<byte[]> GenerateSalesByCategoryExcelAsync(IReadOnlyList<SalesByCategoryDto> data);
        Task<byte[]> GenerateSalesByCategoryPdfAsync(IReadOnlyList<SalesByCategoryDto> data);
    }
}