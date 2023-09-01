// <copyright file="EmailSender.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.EmailSender.Templates.Models;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace MVS.EmailSender.Sender;

public class EmailSender : IEmailSender
{
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailSender(IFluentEmailFactory fluentEmailFactory) => this._fluentEmailFactory = fluentEmailFactory;

    public async Task SendEmailsAsync(IEnumerable<string> emails, ITemplate templateType)
    {
        IFluentEmail fluentEmail = this._fluentEmailFactory.Create();
        await Task.WhenAll(emails.Select(async email => await SendEmailAsync(fluentEmail, email, templateType)));
    }

    public async Task<bool> SendEmailAsync(string email, ITemplate templateType,
        IEnumerable<Attachment> attachments = null)
    {
        IFluentEmail fluentEmail = this._fluentEmailFactory.Create();
        return await SendEmailAsync(fluentEmail, email, templateType, attachments);
    }

    private static async Task<bool> SendEmailAsync(
        IFluentEmail fluentEmail,
        string email,
        ITemplate templateType,
        IEnumerable<Attachment> attachments = null)
    {
        try

        {
            SendResponse sendResponse =
                await fluentEmail
                    .To(email)
                    .Subject(templateType.Subject)
                    .UsingTemplateFromEmbedded(
                        $"MVS.EmailSender.Templates.cshtml.{templateType.TemplateName}.cshtml",
                        templateType.GetModel() ?? new object(),
                        typeof(EmailsLayer).Assembly
                    )
                    .Attach(attachments ?? Enumerable.Empty<Attachment>())
                    .SendAsync();
            return sendResponse.Successful;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
