using MVS.EmailSender.Sender;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MVS.EmailSender;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEmailing(this IServiceCollection services, Action<EmailOptions> options)
    {
        services.Configure(options);

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        EmailOptions emailOptions = serviceProvider.GetService<IOptions<EmailOptions>>().Value;
        services.AddFluentEmail(emailOptions.SenderEmail, emailOptions.SenderName)
            .AddRazorRenderer()
            .AddSmtpSender(
            SenderClient.GetSmtpClient(
                emailOptions.Host,
                emailOptions.Port,
                emailOptions.Username,
                emailOptions.Password,
                true));

        return services;
    }
}
