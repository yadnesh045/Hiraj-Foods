using Hiraj_Foods.Data;
using Hiraj_Foods.Services.IServices;
using System.Net.Mail;
using System.Net;

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
                var fromEmail = new MailAddress("jai.borse01@gmail.com", "Login");
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
                var fromEmailPassword = "cqvyopzamjnvawep"; 
                string subject = "Order Confirmation from Hiraj Foods";

                string logoImageUrl = "https://drive.google.com/uc?export=view&id=1OJyqfkEouzRw56jnqqK1kwxoxor-h-wf";

                string productsAndQuantitiesFormatted = productsAndQuantities.Replace(", ", "<br>");


                var username  = _db.Users.Where(u => u.Email == email).SingleOrDefault().FirstName;


                string body = $@"
                <div style='text-align: center;'>
                    <img src='{logoImageUrl}' alt='Company Logo' style='width: 200px; height: auto;' /><br/><br/>

                    <h2>Order Confirmation</h2>
                    <p>

                        Dear {username},<br/><br/>
                        Your order has been successfully placed with Hiraj Foods! Here are the details:
                    </p>
                    <p><strong>Products:</strong> {productsAndQuantitiesFormatted}</p>
                    <p><strong>Total:</strong> {total}</p>
                    <p>
                        Thank you for shopping with us. Your satisfaction is our top priority!
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
    }
}
