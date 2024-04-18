using iText.Kernel.Pdf;
using iText.Layout.Properties;
using iText.Layout;
using iText.Layout.Element;
using Hiraj_Foods.Models.View_Model;
namespace Hiraj_Foods.Service
{
    public class PDFService
    {
        public byte[] GeneratePDF(Invoice invoice)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Invoice")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Invoice Number: {invoice.InvoiceNumber}"));
                document.Add(new Paragraph($"Date: {invoice.Date.ToShortDateString()}"));
                document.Add(new Paragraph($"Customer Name: {invoice.CustomerName}"));
                document.Add(new Paragraph($"Payment Mode: {invoice.PaymentMode}"));

                Table table = new Table(new float[] { 3, 1 });
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.AddHeaderCell("Item Name");
                table.AddHeaderCell("Quantity");
                foreach (var item in invoice.Items)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.ItemName)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString())));
                }

                document.Add(table);

                document.Add(new Paragraph($"Total Amount: {invoice.TotalAmount.ToString("C")}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                document.Close();
                return stream.ToArray();
            }
        }

    }
}
