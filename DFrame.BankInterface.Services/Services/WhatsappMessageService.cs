using Microsoft.FeatureManagement;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Services.IServices;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace ExpenseTracker.Services.Services
{
	public class WhatsappMessageService : IMessageService
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<WhatsappMessageService> _logger;
		private readonly IFeatureManager _featureManager;


		private readonly string accountSid;
		private readonly string authToken;
		private readonly string senderNumber;


		public WhatsappMessageService(IConfiguration configuration, ILogger<WhatsappMessageService> logger, IFeatureManager featureManager)
		{
			_configuration = configuration;
			_logger = logger;
			accountSid = configuration.GetValue<string>("Twilio:accntSid");
			authToken = configuration.GetValue<string>("Twilio:authToken");
			senderNumber = configuration.GetValue<string>("Twilio:whatsappSenderNumber");
			_featureManager = featureManager;
		}
		public async Task SendMessage(string message, string receipients)
		{
			if (!_featureManager.IsEnabledAsync("SendWhatsappMessages").GetAwaiter().GetResult())
			{
				return;
			}
			try
			{
				TwilioClient.Init(accountSid, authToken);

				var recArray = receipients.Split(',');

				foreach (var rec in recArray)
				{
					try
					{
						if (!IsNumericAndLength10(rec))
						{
							throw new ArgumentException($"Invalid whatsapp sender number : {rec}");
						}
						var response = await MessageResource.CreateAsync(
							   body: message,
							   from: new Twilio.Types.PhoneNumber($"whatsapp:{senderNumber}"),
							   to: new Twilio.Types.PhoneNumber($"whatsapp:+91{rec}"));
					}
					catch (Exception ex)
					{
						_logger.LogError($"Unable to send whatsapp Message to : {rec} Message : {message} Exception : {ex}");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Unable to send whatsapp Message  Exception : {ex}");
			}


		}

		private bool IsNumericAndLength10(string str)
		{
			// Check if the length is 10 and if all characters are digits
			return str.Length == 10 && long.TryParse(str, out _);
		}
	}
}
