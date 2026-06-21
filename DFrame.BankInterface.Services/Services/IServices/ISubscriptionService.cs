using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;

namespace ExpenseTracker.Services.Services.IServices
{
	public interface ISubscriptionService
	{
		Task<ResponseDto> GetAllClientsWithSubscriptionsAsync();
		Task<ResponseDto> GetAllClientsWithActionableSubscriptionsAsync();
		Task<ResponseDto> GetClientWithSubsByClientIdAsync(int clientId);
		Task<ResponseDto> CreateSubscriptionAsync(AddSubscriptionDto addSubscriptionDto);

		Task<ResponseDto> UpdateSubscriptionAsync(AddSubscriptionDto addSubscriptionDto);

		Task<ResponseDto> GetSubscriptionByIdAsync(int subscriptionId);
        Task<ResponseDto> GeneratePaymentScheduleBySubScriptionIdAsync(int subscriptionId);
        Task<ResponseDto> GetPaymentScheduleBySubScriptionIdAsync(int subscriptionId);
        Task<ResponseDto> CreatePaymentScheduleForSubscriptionAsync(List<PaymentScheduleDto> paymentScheduleDto);
        Task<ResponseDto> GetAllDuedPaymentsFromPayScheduleAsync();
        Task<ResponseDto> GetAllTodaysPaymentsFromPayScheduleAsync();
        Task<ResponseDto> GetAllFuturePaymentsFromPayScheduleAsync();
        Task<ResponseDto> GetAllPaymentsToProcessAsync();
		Task<ResponseDto> GetPaymentHistories(DateTime startDate, DateTime endDate);

		Task<ResponseDto> UpdateMakersPaymentStatus(List<Guid> payments, int statusToUpdate, string comments);

		Task<ResponseDto> UpdateManualPaymentStatus(UpdateManualPaymentStatusDto payment);

		Task<ResponseDto> GetAllPaymentsForClient(int clientId);

		Task<ResponseDto> UpdateSubscriptionStatus(UpdateSubscriptionStatusRequestDto request);

		Task<ResponseDto> GetBorrowLetterDetails(Guid sibsGuid);
		Task<ResponseDto> AddBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowwLetter);
		Task<ResponseDto> UpdateBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowwLetter);

		ResponseDto GetBorrowLetterDetailsToPrint(Guid sibsGuid);
		
        
    }
}
