namespace BookBuddy.API.Email;

using BookBuddy.API.Models.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;  //Encapsulation
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        this._configuration = configuration;
        this._logger = logger;
    }

    //-------------------constructor + DI above -----------------------------

    //------------------ Implemented methods below --------------------------

    public async Task SendOrderConfirmationEmailAsync(OrderConfirmationEmailDTO orderConfirmationEmailDTO)
    {
        //read the configuration values from appsettings.json file.
        var mail = _configuration["Email:Mail"];
        var password = _configuration["Email:Password"];
        var host = _configuration["Email:Host"];
        var port = int.Parse(_configuration["Email:Port"]);
        var displayname = _configuration["Email:DisplayName"];

        //creating the Confirmation email message.
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(displayname, mail));

        //Set the recipient email address {TO}
        email.To.Add(new MailboxAddress(orderConfirmationEmailDTO.CustomerName, orderConfirmationEmailDTO.CustomerEmail));

        //Set the subject of the email
        email.Subject = "BookBuddy Rental Confirmation";

        //Create the email body using HTML
        email.Body = new TextPart("html")
        {
            Text = $"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8">
            <title>BookBuddy Rental Confirmation</title>
        </head>

        <body style="font-family: Arial, sans-serif; line-height: 1.6;">

            <h2>BookBuddy - Rental Confirmed 🎉</h2>

            <p>Hello <strong>{orderConfirmationEmailDTO.CustomerName}</strong>,</p>

            <p>
                Your BookBuddy rental has been successfully confirmed.
            </p>

            <h3>Rental Details</h3>

            <table cellpadding="8" cellspacing="0" border="1">
                <tr>
                    <td><strong>Book</strong></td>
                    <td>{orderConfirmationEmailDTO.BookTitle}</td>
                </tr>

                <tr>
                    <td><strong>Rental ID</strong></td>
                    <td>{orderConfirmationEmailDTO.RentalId}</td>
                </tr>

                <tr>
                    <td><strong>Payment ID</strong></td>
                    <td>{orderConfirmationEmailDTO.PaymentId}</td>
                </tr>

                <tr>
                    <td><strong>Rental Period</strong></td>
                    <td>{orderConfirmationEmailDTO.RentalDays} days</td>
                </tr>

                <tr>
                    <td><strong>Rental Price</strong></td>
                    <td>₹{orderConfirmationEmailDTO.RentalPrice}</td>
                </tr>

                <tr>
                    <td><strong>Rental Date</strong></td>
                    <td>{orderConfirmationEmailDTO.RentalDate:dd MMM yyyy}</td>
                </tr>

            </table>

            <p>Thank you for choosing BookBuddy.</p>

            <p><strong>Happy Reading! 📚</strong></p>

            <p>Team BookBuddy</p>

        </body>
        </html>
        """
        };


        //Send the email using Gmail SMTP client
        //Create the MailKit  SMTP client 
        try
        {
            using var smtpClient = new SmtpClient();   //Why using ? : The SMTP clients holds network resource. and this 'using' ensures that the resources are released after the email is sent.

            //connect to the SMTP server
            await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);

            //authenticate with the SMTP server
            await smtpClient.AuthenticateAsync(mail, password);

            //send the email
            await smtpClient.SendAsync(email);

            //disconnect from the SMTP server
            await smtpClient.DisconnectAsync(true);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send order confirmation email to {CustomerEmail}", orderConfirmationEmailDTO.CustomerEmail);
        }


    }
}
















