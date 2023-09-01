// <copyright file="SenderClient.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Mail;
namespace MVS.EmailSender.Sender;

public class SenderClient
{
    internal static SmtpClient GetSmtpClient(string host, int port, string username, string password, bool useSsl)
    {
        SmtpClient smtpClient = new SmtpClient(host, port);
        smtpClient.EnableSsl = useSsl;
        smtpClient.Credentials = new NetworkCredential(username, password);
        return smtpClient;
    }
}
