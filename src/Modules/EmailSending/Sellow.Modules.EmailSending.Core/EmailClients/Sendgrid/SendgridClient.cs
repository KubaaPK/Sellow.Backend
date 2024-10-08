using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Sellow.Modules.EmailSending.Core.EmailClients.Sendgrid;

internal sealed class SendgridClient
{
    private readonly ILogger<SendgridClient> _logger;
    private readonly ISendGridClient _sendGridClient;

    public SendgridClient(ILogger<SendgridClient> logger, IOptions<SendgridOptions> sendgridOptions)
    {
        _logger = logger;
        _sendGridClient = new SendGridClient(sendgridOptions.Value.ApiKey);
    }

    public async Task SendEmail(SendGridMessage email, CancellationToken cancellationToken = default)
    {
        await _sendGridClient.SendEmailAsync(email, cancellationToken);

        _logger.LogInformation("Email {@Email} has been sent", email.Serialize());
    }
}