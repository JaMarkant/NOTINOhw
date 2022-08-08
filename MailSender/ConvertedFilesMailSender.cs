using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace MailSender
{
    public class ConvertedFilesMailSender
    {
        private string FromName { get; set; }
        private string FromAddress { get; set; }
        private string SmtpServer { get; set; }
        private int SmtpPort { get; set; }
        private string SmtpUsername { get; set; }
        private string SmtpPassword { get; set; }
        private SmtpClient SmtpClient { get; set; }
        public ConvertedFilesMailSender(IConfiguration configuration)
        {
            FromName = configuration.GetValue<string>("EmailConfiguration:FromName");
            FromAddress = configuration.GetValue<string>("EmailConfiguration:FromAddress");
            SmtpServer = configuration.GetValue<string>("EmailConfiguration:SmtpServer");
            SmtpPort = configuration.GetValue<int>("EmailConfiguration:SmtpPort");
            SmtpUsername = configuration.GetValue<string>("EmailConfiguration:Username");
            SmtpPassword = configuration.GetValue<string>("EmailConfiguration:Password");
            SmtpClient = new SmtpClient();
        }
        //used for tests
        public ConvertedFilesMailSender()
        {
            FromName = string.Empty;
            FromAddress = string.Empty;
            SmtpServer = string.Empty;
            SmtpPort = 0;
            SmtpUsername = string.Empty;
            SmtpPassword = string.Empty;
            SmtpClient = new SmtpClient();
        }
        public void sendConvertedFile(string emailAddress, string filePath)
        {
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress(FromName, FromAddress);
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress(string.Empty, emailAddress);
            message.To.Add(to);
            message.Subject = "Your converted file";

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "You will find your file in the attachments!";
            bodyBuilder.Attachments.Add(filePath);

            message.Body = bodyBuilder.ToMessageBody();

            SendMessageViaSmtp(message);
        }

        private void ConnectSmtp()
        {
            SmtpClient.Connect(SmtpServer, SmtpPort, true);
        }
        private void AuthenticateSmtp()
        {
            SmtpClient.Authenticate(SmtpUsername, SmtpPassword);
        }
        private void SendMessageViaSmtp(MimeMessage message)
        {
            ConnectSmtp();
            AuthenticateSmtp();
            SmtpClient.Send(message);
            SmtpClient.Disconnect(true);
            SmtpClient.Dispose();
        }
    }
}
