using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IInvestmentService
    {
        Task<ResponseDto> GetAllInvestments();
        Task<ResponseDto> GetClientWithInvestmentByClientIdAsync(int clientId);
        Task DeleteInvestment(int investmentId);
        Task<ResponseDto> GetInvestmentByIdAsync(int investmentId);
        Task<ResponseDto> GenerateSplPaymentSchedule(int inestmentid);
        Task<ResponseDto> GetSplPaymentScheduleByInvId(int inestmentid);

        Task<ResponseDto> CreateInvestmentAsync(InvestmentsDto investmentDto);
        Task<ResponseDto> UpdateInvestmentAsync(UpdateInvestmentsDto investmentsDto);
        Task<ResponseDto> UpdateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto);
        Task<ResponseDto> UpdateClientBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto);
        Task<ResponseDto> CreateInvestmentPaymentSchedulesAsync(List<SplPaymentScheduleDto> schedulesDto);
        Task<ResponseDto> GetAllTodaysPaymentsFromInvestPayScheduleAsync();
        Task<ResponseDto> GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync();
        Task<ResponseDto> GetInvestmentPaymentHistoriesAsync(DateOnly startDate, DateOnly endDate);
        Task<ResponseDto> GetAllInvestmentPaymentsToProcessAsync();
        Task<ResponseDto> GetAllExternalPaymentsAsync();
        Task<ResponseDto> GetExternalPaymentByIdAsync(int id);
        Task<ResponseDto> AddExternalPaymentAsync(ExternalPaymentsDto paymentDto);
        Task<ResponseDto> UpdateExternalPaymentAsync(ExternalPaymentsDto paymentDto);
        Task<ResponseDto> DeleteExternalPaymentAsync(int id);
        Task<ResponseDto> UpdateMakerInvestmentPaymentScheduleStatusAsync(List<Guid> payments, int statusToUpdate, string comments);
        Task<ResponseDto> UpdateManualInvestmentPaymentScheduleStatusAsync(UpdateManualInvestmentPaymentStatusDto payment);
        Task<ResponseDto> CreateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto);
        Task<ResponseDto> CreateClientBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto);
        Task<ResponseDto> DeleteInvestmentReceivedDetailsAsync(int id);
        Task<ResponseDto> DeleteClientBankingDetailAsync(int id);
        Task<ResponseDto> SaveInvestmentBorrowLetterDetailsAsync(InvestmentBorrowLetterRequestDto borrowwLetter);
        Task<ResponseDto> UpdateInvestmentStatus(UpdateSubscriptionStatusRequestDto request);
        Task<ResponseDto> GetAllClientsWithActionableInvestmentsAsync();


        Task<ResponseDto> AddInvestmentRecAttachement(int id, IFormFile? file);
    }
}
