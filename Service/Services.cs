using Hiraj_Foods.Data;
using Hiraj_Foods.Services.IServices;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Hiraj_Foods.Service
{
    public class Services : IServices
    {
        private readonly ApplicationDbContext _db;

        public Services(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool SendLoginCredentials(string email, string password)
        {
            try
            {
                var fromEmail = new MailAddress("jai.borse01@gmail.com", "Hiraj Foods");
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "cqvyopzamjnvawep";  // Make sure to secure your credentials and consider environment variables or secure vaults
                string subject = "Your Account is Successfully Created!";

                // Replace the URL with the actual URL where your logo is hosted
                string logoImageUrl = "https://drive.google.com/uc?export=view&id=1OJyqfkEouzRw56jnqqK1kwxoxor-h-wf";

                string body = $@"
                                <div style='text-align: center;'>
                                        <img src='{logoImageUrl}' alt='Company Logo' style='width: 200px; height: auto;' /><br/><br/>
                                        <h2>Welcome to Hiraj Foods!</h2>
                                        <p>
                                            Dear {email},<br/><br/>
                                            We are thrilled to welcome you to Hiraj Foods, your one-stop destination for all your Healthiest Food Mixes !!!
                                        </p>
                                        <p>
                                            Your account has been successfully created. Here are your login credentials:
                                            <br/><br/>
                                            <strong>Username:</strong> {email}<br/>
                                        </p>
                                        <p>
                                            Now that you're all set up, it's time to start browsing and shopping on our website!
                                            <br/><br/>
                                            <a href='https://hirajfoods.com/' style='padding: 10px 20px; background-color: #f0a500; color: #fff; text-decoration: none; border-radius: 5px;'>Start Shopping Now</a>
                                        </p>
                                        <p>
                                            If you have any questions or need assistance, feel free to reach out to our customer support team. Happy shopping!
                                        </p>
                            </div>";




                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587, // Gmail SMTP port
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                    return true; // Email sent successfully
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }

        public bool SendOrderConfirmation(string email, string productsAndQuantities, decimal total)
        {
            try
            {
                var fromEmail = new MailAddress("jai.borse01@gmail.com", "Hiraj Foods");
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "cqvyopzamjnvawep"; // Ensure this is secured

                // Email settings
                string subject = "Order Confirmation from Hiraj Foods";
                string logoImageUrl = "https://drive.google.com/uc?export=view&id=1OJyqfkEouzRw56jnqqK1kwxoxor-h-wf";

                // Parsing the products, quantities, and prices
                var items = productsAndQuantities.Split(',');
                var productsTableHtml = "<table border='1' style='width: 100%; border-collapse: collapse;'><tr><th>Sr. No.</th><th>Product Name</th><th>Quantity</th><th>Price Each</th><th>Total</th></tr>";
                int index = 1;
                decimal subtotal = 0;

                foreach (var item in items)
                {
                    var parts = item.Trim().Split(new string[] { "\t:" }, StringSplitOptions.None);
                    if (parts.Length == 3)
                    {
                        string productName = parts[0].Trim();
                        if (int.TryParse(parts[1].Trim(), out int quantity) && decimal.TryParse(parts[2].Trim(), out decimal price))
                        {
                            decimal totalOfProduct = quantity * price;
                            subtotal += totalOfProduct;
                            productsTableHtml += $"<tr><td>{index++}</td><td>{productName}</td><td>{quantity}</td><td>Rs. {price:0.00}</td><td> Rs. {totalOfProduct:0.00}</td></tr>";
                        }
                    }
                }
                productsTableHtml += $"<tr><th colspan='4' style='text-align:right;'>Subtotal</th><th>Rs. {subtotal:0.00}</th></tr></table>";

                // Fetch username from database
                var username = _db.Users.Where(u => u.Email == email).SingleOrDefault()?.FirstName ?? "Customer";

                // Email body
                string body = $@"
        <div style='text-align: center;'>
            <img src='{logoImageUrl}' alt='Company Logo' style='width: 200px; height: auto;' /><br/><br/>
            <h2>Order Confirmation</h2>
            <p>Dear {username},<br/><br/>
            Your order has been successfully placed with Hiraj Foods! Here are the order details:</p>
            {productsTableHtml}
            <p><strong>Grand Total:</strong> Rs.    {total.ToString("0.00")}</p>
            <p>Thank you for shopping with us. Your satisfaction is our top priority!</p>
        </div>";

                // SMTP configuration
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                    return true; // Email sent successfully
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }

        public string SendForgetPassword(string email)
        {

            try
            {
                // Generate a random password
                string newPassword = GenerateRandomPassword(8); // Change the length as needed

                // Send the email
                var fromEmail = new MailAddress("jai.borse01@gmail.com", "Hiraj Foods");
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "cqvyopzamjnvawep";  // Make sure to secure your credentials and consider environment variables or secure vaults
                string subject = "Password Reset !!!";

                // Replace the URL with the actual URL where your logo is hosted
                string logoImageUrl = "https://drive.google.com/uc?export=view&id=1OJyqfkEouzRw56jnqqK1kwxoxor-h-wf";

                string body = $@"
    <div style='text-align: center;'>
        <img src='{logoImageUrl}' alt='Company Logo' style='width: 200px; height: auto;' /><br/><br/>
        <h2>Password Reset Request</h2>
        <p>
            Dear User,<br/><br/>
            We have received a request to reset your password. Please find below your new temporary password:
        </p>
        <p style='font-size: 24px; font-weight: bold; color: #f00;'>{newPassword}</p>
        <p>
            After logging in with this temporary password, we recommend changing it to something more memorable.<br/><br/>
            If you did not request a password reset, please ignore this email or contact support immediately.
        </p>
        <p>
            If you have any questions or need further assistance, please feel free to contact our support team.
        </p>
    </div>";



                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587, // Gmail SMTP port
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };


                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                    return newPassword; // Email sent successfully
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return null; // Email sending failed
            }

        }


        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(validChars.Length);
                sb.Append(validChars[index]);
            }

            return sb.ToString();
        }

    }
}
