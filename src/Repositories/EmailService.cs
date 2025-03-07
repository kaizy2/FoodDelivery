﻿using MailKit.Net.Smtp;
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
        emailMessage.From.Add(new MailboxAddress("TinTruong", "kienkoi48@gmail.com")); // Địa chỉ email của bạn
        emailMessage.To.Add(new MailboxAddress("", email)); // Địa chỉ email người nhận
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message }; // Nội dung email

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, false);// Thay đổi thông tin máy chủ SMTP
            client.Authenticate("kienkoi48@gmail.com", "xxns frip zcpa odvl"); // Thay đổi thông tin xác thực
            await client.SendAsync(emailMessage);
            client.Disconnect(true);
        }
    }
}
