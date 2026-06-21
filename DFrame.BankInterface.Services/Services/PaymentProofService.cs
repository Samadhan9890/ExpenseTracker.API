using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using System.Collections.Generic;

namespace ExpenseTracker.Services.Services
{
    public class PaymentProofService : IPaymentProofService
    {
        private readonly IPaymentProofRepository _repository;
        private ResponseDto _responseDto;
        private readonly ILogger<PaymentProofService> _logger;
        public PaymentProofService(IPaymentProofRepository repository, ResponseDto responseDto, ILogger<PaymentProofService> logger)
        {
            _repository = repository;
            _responseDto = responseDto;
            _logger = logger;
        }

        public async Task<ResponseDto> CreatePaymentProofAsync(PaymentProofDto paymentProofDto)
        {
            try
            {
                var paymentProof = new PaymentProof
                {
                    PaymentScheduleGuid = paymentProofDto.PaymentScheduleGuid,
                    TReference = paymentProofDto.TReference,
                    Notes = paymentProofDto.Notes,
                    Attachment = paymentProofDto.Attachment,
                    CreatedDate = DateTime.Now,
                    CreatedBy = paymentProofDto.CreatedBy,
                    OriginalFileName = paymentProofDto.OriginalFileName,
                };

                paymentProof = await _repository.AddpaymentProofAsync(paymentProof);

                paymentProofDto.Id = paymentProof.Id;
                paymentProofDto.CreatedDate = paymentProof.CreatedDate;
                _responseDto.Result = paymentProofDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while creating payment proof.");
            }

            return _responseDto;
        }

        public async Task<List<PaymentProof>> GetPaymentProofsByPaymentScheduleGuidAsync(Guid paymentScheduleGuid)
        {
            try
            {
                List<PaymentProof> paymentProofs = await _repository.GetPaymentProofByPaymentScheduleGuidAsync(paymentScheduleGuid);
                return paymentProofs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving payment proofs by PaymentScheduleGuid: {PaymentScheduleGuid}", paymentScheduleGuid);
                throw;
            }
        }

        public async Task<PaymentProof> GetPaymentProofByIdAsync(int id)
        {
            try
            {
                PaymentProof paymentProof = new PaymentProof();

                paymentProof = await _repository.GetPaymentProofByIdAsync(id);
                return paymentProof;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving payment proof with Id: {Id}", id);
                throw;
            }
        }
    }
}
