using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace BlackBoxInc.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();//Prep up the email format to be sent later 
            email.From.Add(MailboxAddress.Parse(_config["Smtp:User"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            //Establish connection parameters, host, port and encryption to be used
            await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]),
                SecureSocketOptions.StartTls);
            //Then set up username and password
            await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
            // await smtp.SendAsync(email);
            Console.WriteLine("Mail about to be sent");
            try
            {
                await smtp.SendAsync(email);
                _logger.LogInformation("Email sent successfully to {Recipient}", to);
                Console.WriteLine("Mail sent!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", to);
                Console.WriteLine("Mail not sent!!!");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                Console.WriteLine("Mail processed");
            }
        }
    }
}

