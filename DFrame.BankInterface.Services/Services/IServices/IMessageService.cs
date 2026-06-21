namespace ExpenseTracker.Services.Services.IServices
{
	public interface IMessageService
	{
		Task  SendMessage(string message, string receipients);
	}
}
