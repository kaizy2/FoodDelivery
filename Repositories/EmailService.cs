using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Nam", "leokay2406@gmail.com")); // Địa chỉ email của bạn
        emailMessage.To.Add(new MailboxAddress("", email)); // Địa chỉ email người nhận
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message }; // Nội dung email

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, false); // Kết nối tới máy chủ SMTP (Gmail)
            client.Authenticate("leokay2406@gmail.com", "ihvq zrsy lzrr xphj"); // Thay đổi thông tin xác thực
            await client.SendAsync(emailMessage);
            client.Disconnect(true);
        }
    }
}
