using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Windows;

namespace WpfApp11
{
    public partial class EmailWindow : Window
    {
        private string attachmentPath;
        

        public EmailWindow(string filePath)
        {
            InitializeComponent();
            attachmentPath = filePath;
        }

        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string from = FromTextBox.Text;
            string to = ToTextBox.Text;
            string subject = SubjectTextBox.Text;
            string body = BodyTextBox.Text;

            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(null, from)); 
                message.To.Add(new MailboxAddress(null, to));
                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;

                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    bodyBuilder.Attachments.Add(attachmentPath);
                }

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = GetSmtpClient(from);
                client.Send(message);
                client.Disconnect(true);

                MessageBox.Show("Письмо успешно отправлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при отправке письма: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private SmtpClient GetSmtpClient(string fromEmail)
        {
            SmtpClient client = new SmtpClient();

            if (fromEmail.EndsWith("@gmail.com"))
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("your-gmail-username", "your-gmail-password");
            }
            else if (fromEmail.EndsWith("@mail.ru"))
            {
                client.Connect("smtp.mail.ru", 587, SecureSocketOptions.StartTls);
                client.Authenticate("your-mail-username", "your-mail-password");
            }
            else if (fromEmail.EndsWith("@rambler.ru"))
            {
                client.Connect("smtp.rambler.ru", 587, SecureSocketOptions.StartTls);
                client.Authenticate("your-rambler-username", "your-rambler-password");
            }
            else if (fromEmail.EndsWith("@yandex.ru") || fromEmail.EndsWith("@yandex.com"))
            {
                client.Connect("smtp.yandex.ru", 587, SecureSocketOptions.StartTls);
                client.Authenticate("your-yandex-username", "your-yandex-password");
            }
            else
            {
                throw new Exception("Невозможно отправить письмо. Неизвестный почтовый сервис.");
            }

            return client;
        }
    }
}
