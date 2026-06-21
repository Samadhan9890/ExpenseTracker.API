using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Dashboard;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
	public interface ISubscriptionRepository
	{
		Task<List<ClientMaster>> GetAllClientsWithSubscriptionsAsync();
		Task<List<TblSubscription>> GetAllSubscriptionsAsync();
		Task DeleteSubscriptionAsync(int subscriptionId);
		Task<TblSubscription> GetSubscriptionByIdAsync(int subscritptionId);
		Task<ClientMaster?> GetClientWithSubByClientIdAsync(int clientId);

		Task<TblSubscription> CreateSubscriptionAsync(TblSubscription subscription);
        Task<List<TblPaymentSchedule>> GetPaymentSchedulesBySubScriptionIdAsync(int subscriptionId);
        Task<List<TblPaymentSchedule>> CreatePaymentScheduleForSubscriptionAsync(List<TblPaymentSchedule> tblPaymentSchedules);
		Task<TblSubscription> UpdateSubscriptionAsync(TblSubscription subscription);
        Task<List<PaymentScheduleDto>> GetAllDuedPaymentsFromPayScheduleAsync();
        Task<List<PaymentScheduleDto>> GetAllTodaysPaymentsFromPayScheduleAsync();
        Task<List<PaymentScheduleDto>> GetAllFuturePaymentsFromPayScheduleAsync();
		Task<List<PaymentScheduleDto>> GetAllPaymentsToProcessAsync();

		Task<List<PaymentScheduleDto>> GetPaymentHistories(DateTime startDate, DateTime endTime);

		Task UpdateMakersPaymentStatus(List<Guid> payments, int statusToUpdate,string comments,string updatedBy);

		Task UpdateManualPaymentStatus(UpdateManualPaymentStatusDto payment);

		Task<List<SubsWithPaymentsDto>> GetAllClientsSubscriptionWithPayments(int clientId);
		Task UpdateSubscriptionStatus(UpdateSubscriptionStatusRequestDto request);

		Task<BorrowLetterDetailsResponseDto> GetBorrowLetterDetails(Guid subsGuid);
		Task AddBorrowLetterDetails(TblBorrowLetterDetails borrowLetterDetails,int status);
		Task UpdateBorrowLetterDetails(TblBorrowLetterDetails borrowLetterDetails, int status);

		BorrowLetterPrintDto GetBorrowLetterDetailsToPrint(Guid subsGuid);

		Task<ClientMaster> GetClientDetailByPaymentId(Guid guid);
    }
}
