using System.Collections.Generic;

namespace ExpenseTracker.Services.Models
{
	public class CommunicationConfigOptions
	{
		public List<Contact> Contacts { get; set; }
		public List<MessageTemplate> MessageTemplates { get; set; }
	}

	public class Contact
	{
		public string Type { get; set; }
		public string ContactNo { get; set; }
	}

	public class MessageTemplate
	{
		public string MessageType { get; set; }
		public string Message { get; set; }
	}
}