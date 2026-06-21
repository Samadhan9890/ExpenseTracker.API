using ExpenseTracker.Services.Models.DTOs.Dashboard;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IInvestmentRepository
    {
        Task<ClientMaster?> GetClientWithInvestmentByClientIdAsync(int clientId);
        Task<List<Investment>> GetAllInvestments();
        Task DeleteInvestment(int investmentId);
        Task<Investment> GetInvestmentByIdAsync(int investmentId);
        Task<Investment> CreateInvestmentAsync(InvestmentsDto investmentsDto);
        Task<Investment> UpdateInvestmentAsync(UpdateInvestmentsDto investmentsDto);
        Task<InvestmentReceivedDetails> UpdateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto);
        Task<ClientBankingDetail> UpdateClientBankingDetailAsync(ClientBankingDetailsDto clientBankingDetailsDto);
        Task<List<SplPaymentSchedule>> SaveInvestmentPaymentSchedulesAsync(List<SplPaymentSchedule> schedules);
        Task<List<SplPaymentSchedule>> GetSplPaymentScheduleByInvestmentId(int investmentId);
        Task<List<SplPaymentScheduleDto>> GetAllTodaysPaymentsFromInvestPayScheduleAsync();
        Task<List<SplPaymentScheduleDto>> GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync();
        Task<List<SplPaymentScheduleDto>> GetInvestmentPaymentHistoriesAsync(DateOnly startDate, DateOnly endDate);
        Task<Dictionary<int, string>> GetClientNamesByIdsAsync(List<int> clientIds);
        Task<List<SplPaymentScheduleDto>> GetAllInvestmentPaymentsToProcessAsync();
        Task<List<ExternalPayment>> GetAllExternalPaymentsAsync();
        Task<ExternalPayment> GetExternalPaymentsByIdAsync(int id);
        Task<ExternalPayment> AddExternalPaymentsAsync(ExternalPayment payment);
        Task<ExternalPayment> UpdateExternalPaymentsAsync(ExternalPayment payment);
        Task DeleteExternalPaymentsAsync(int id);
        Task UpdateMakerInvestmentPaymentScheduleStatusAsync(List<Guid> payments, int statusToUpdate, string comments, string updatedBy);
        Task UpdateManualInvestmentPaymentScheduleStatusAsync(UpdateManualInvestmentPaymentStatusDto payment);

        Task<InvestmentReceivedDetails> CreateInvestmentReceivedDetailsAsync(InvestmentReceivedDetails receivedDetail);
        Task<ClientBankingDetail> CreateClientBankingDetailAsync(ClientBankingDetail clientBankingDetail);
        Task<InvestmentReceivedDetails> DeleteInvestmentReceivedDetailsAsync(int id);
        Task<ClientBankingDetail> DeleteClientBankingDetailAsync(int id);

        Task SaveInvestmentBorrowLetterDetailsAsync(InvestmentBorrowLetterRequestDto borrowLetterDetails);

        Task UpdateInvestmentStatus(UpdateSubscriptionStatusRequestDto request);
        Task<List<ClientMaster>> GetAllClientsWithActionableInvestmentsAsync();

        Task<InvestmentReceivedDetails> AddInvestmentRecAttachement(int id, IFormFile? file);
    }
}
