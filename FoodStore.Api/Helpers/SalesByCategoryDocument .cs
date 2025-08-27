using FoodStore.Contracts.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FoodStore.Api.Helpers
{
    public class SalesByCategoryDocument : IDocument
    {
    private readonly IReadOnlyList<SalesByCategoryDto> _data;

    public SalesByCategoryDocument(IReadOnlyList<SalesByCategoryDto> data)
    {
        _data = data;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Header().Text("Sales by Category Report")
                         .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

            page.Content().Table(table =>
            {
                // Define 3 columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Category
                    columns.RelativeColumn(2); // Quantity
                    columns.RelativeColumn(3); // Revenue
                });

                // Table Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Category").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Quantity").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Revenue").Bold();
                });

                // Data rows
                foreach (var item in _data)
                {
                    table.Cell().Padding(5).Text(item.Category);
                    table.Cell().Padding(5).Text(item.TotalQuantity.ToString());
                    table.Cell().Padding(5).Text($"{item.Revenue:C}"); // Currency
                }
            });

            page.Footer().AlignCenter().Text($"Generated at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC")
                          .FontSize(10).FontColor(Colors.Grey.Medium);
        });
    }
  }
}