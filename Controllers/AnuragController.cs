using Hiraj_Foods.Service;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Hiraj_Foods.Services.IServices;

namespace Hiraj_Foods.Controllers
{
    public class AnuragController : Controller
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServices _services;


        public AnuragController(IUnitOfWorks unitOfWorks, IHttpContextAccessor httpContextAccessor, IServices services)
        {
            this.unitOfWorks = unitOfWorks;
            _httpContextAccessor = httpContextAccessor;
            _services = services;
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

                if (parts.Length != 3)
                {
                    continue;
                }

                var productName = parts[0].Trim();
                var quantity = int.Parse(parts[1].Trim());
                var price = decimal.Parse(parts[2].Trim());

                var invoiceItem = new InvoiceItem
                {
                    ItemName = productName,
                    Quantity = quantity,
                    UnitPrice = price
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

            // Calculate total amount based on product prices
            invoice.TotalAmount = invoiceItems.Sum(item => item.Quantity * item.UnitPrice);

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
        public IActionResult TrackOrder(int orderId)
        {
            SetLayoutModel();
            var orders = unitOfWorks.Uorders.GetById(orderId);
            ViewBag.OrderStatus = orders.status;
            return View(orders);
        }


        public void SetLayoutModel()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId != 0)
            {

                var user = unitOfWorks.Users.GetById(userId);
                var cartItems = unitOfWorks.Cart.GetByUserId(userId);
                var Profilepic = unitOfWorks.UserImage.GetByUserId(userId);

                var layoutModel = new LayoutModel
                {
                    CartItemCount = cartItems.Count(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    profilepic = Profilepic?.user_Profile_Img // Use the null-conditional operator to avoid NullReferenceException
                };
                // If Profilepic is null, set a default image or leave it as null
                if (Profilepic == null)
                {
                    layoutModel.profilepic = "\"~/img/avatars/2.png\""; // Or set a default image path
                }
                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;

            }
        }



    }

}

