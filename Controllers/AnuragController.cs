using Hiraj_Foods.Service;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Controllers
{
    public class AnuragController : Controller
    {
        private readonly IUnitOfWorks unitOfWorks;

        public AnuragController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult GenerateInvoicePDF(int orderId)
        {


            var order = unitOfWorks.Uorders.GetById(orderId);

            var user = unitOfWorks.Users.GetById(order.UserId);

            string invoiceNumber = GenerateRandomInvoiceNumber();

            var productEntries = order.Products.Split(',');

            var invoiceItems = new List<InvoiceItem>();

            foreach (var productEntry in productEntries)
            {
                var parts = productEntry.Trim().Split(':');

                if (parts.Length != 2)
                {
                    continue;
                }

                var productName = parts[0].Trim();
                var quantity = int.Parse(parts[1].Trim());

                var invoiceItem = new InvoiceItem
                {
                    ItemName = productName,
                    Quantity = quantity,
                    UnitPrice = 0.0m
                };

                invoiceItems.Add(invoiceItem);
            }
            var invoice = new Invoice
            {
                InvoiceNumber = GenerateRandomInvoiceNumber(),
                Date = DateTime.Now,
                CustomerName = user.FirstName,
                Items = invoiceItems,
                PaymentMode = order.Paymentmethod
            };

            invoice.TotalAmount = order.Total;


            return new ViewAsPdf("GeneratePDFFromView", invoice)
            {
                FileName = $"{invoice.CustomerName}{invoice.Date}Invoice.pdf"
            };
        }
        private string GenerateRandomInvoiceNumber()
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
