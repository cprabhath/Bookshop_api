using System.Net.Mail;
using System.Net;
using System.Text;

namespace Bookshop_api.Utils
{
    public class OrderApprovalEmail
    {
        public string SendOrderApproveMail(string email, string orderId, string orderDate, string customerName, List<(string itemName, int quantity, double price)> orderItems)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("librarylkactivation@gmail.com", "ocma xovk fjdn hfhc"),
                    EnableSsl = true
                };

                // Read the email template
                string htmlTemplatePath = "E:\\Projects\\AD Course Work\\BookShop\\Bookshop_api\\Bookshop_api\\Utils\\OrderApprove.html";
                string htmlTemplate = File.ReadAllText(htmlTemplatePath);
                // Build the HTML for order items dynamically
                StringBuilder orderItemsHtml = new StringBuilder();
                double totalPrice = 0;
                string orderItemsTable = "";
                foreach (var item in orderItems)
                {
                    double itemTotalPrice = item.price * item.quantity;
                    totalPrice += itemTotalPrice;

                    orderItemsTable += $"<tr>" +
                                       $"<td>{item.itemName}</td>" +
                                       $"<td>{item.quantity}</td>" +
                                       $"<td>{item.price:C}</td>" +
                                       $"<td>{totalPrice:C}</td>" +
                                       $"</tr>";
                }

                string emailBody = htmlTemplate
                    .Replace("{{ORDER_ID}}", orderId)
                    .Replace("{{ORDER_DATE}}", orderDate)
                    .Replace("{{CUSTOMER_NAME}}", customerName)
                    .Replace("{{ORDER_ITEMS}}", orderItemsTable)
                    .Replace("{{TOTAL_PRICE}}", totalPrice.ToString("C"));


                // Create a new MailMessage object
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("librarylkactivation@gmail.com"),
                    Subject = "Bookverse - Your Order Details",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                // Add the recipient's email address
                mailMessage.To.Add(email);

                // Send the email
                smtpClient.Send(mailMessage);

                return "OK";
            }
            catch (Exception ex)
            {
                
                return ex.Message;
            }
        }
    }
}
