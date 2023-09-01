// <copyright file="EmailOptions.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender;

public class EmailOptions
{
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
