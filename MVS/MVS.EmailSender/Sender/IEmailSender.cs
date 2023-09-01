// <copyright file="IEmailSender.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.EmailSender.Templates.Models;
using FluentEmail.Core.Models;

namespace MVS.EmailSender.Sender;

public interface IEmailSender
{
    public Task SendEmailsAsync(IEnumerable<string> emails, ITemplate templateType);
    public Task<bool> SendEmailAsync(string email, ITemplate templateType, IEnumerable<Attachment> attachments = null);
}
