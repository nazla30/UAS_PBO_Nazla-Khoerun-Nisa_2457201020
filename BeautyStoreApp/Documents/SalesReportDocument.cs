using BeautyStoreApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BeautyStoreApp.Documents
{
    public class SalesReportDocument : IDocument
    {
        private readonly List<OrderHeader> _orders;

        public SalesReportDocument(List<OrderHeader> orders)
        {
            _orders = orders;
        }

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("LAPORAN PENJUALAN")
                    .FontSize(22)
                    .Bold()
                    .AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("No").Bold();
                        header.Cell().Text("Order");
                        header.Cell().Text("Tanggal");
                        header.Cell().Text("Customer");
                        header.Cell().AlignRight().Text("Total");
                    });

                    int no = 1;

                    foreach (var item in _orders)
                    {
                        table.Cell().Text(no++.ToString());

                        table.Cell().Text(item.OrderNumber);

                        table.Cell().Text(
                            item.OrderDate.ToString("dd/MM/yyyy"));

                        table.Cell().Text(
                            item.Customer?.FullName ?? "-");

                        table.Cell()
                            .AlignRight()
                            .Text("Rp " +
                                  item.TotalAmount.ToString("N0"));
                    }
                });

                page.Footer()
                    .AlignRight()
                    .Text($"Dicetak : {DateTime.Now}");
            });
        }
    }
}