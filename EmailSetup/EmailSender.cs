using System.Net;
using System.Net.Mail;

namespace Backend.EmailSetup;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("TheLegendsTeam@outlook.com", "Legends@1234"),
                TargetName = "STARTTLS/smtp.live.com",
                EnableSsl = true,
            };

            var mail = new MailMessage("TheLegendsTeam@outlook.com", email, subject, message);


            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            // Log any exceptions for troubleshooting
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw; // Rethrow the exception to be handled elsewhere if needed
        }
    }
}