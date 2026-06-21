using ExpenseTracker.Services.Models.EntityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class InvestmentsDto
    {
        public int InvestmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "ClientName is required.")]
        public string ClientName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PlanId must be greater than 0.")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "PlanName is required.")]
        public string PlanName { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999", ErrorMessage = "InvestmentAmount must be greater than 0.")]
        public decimal InvestmentAmount { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "InvestmentStartDate is required.")]
        public DateOnly InvestmentStartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "InvestmentEndDate is required.")]
        public DateOnly InvestmentEndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PayoutFrequency must be greater than 0.")]
        public int PayoutFrequency { get; set; }

        [Range(typeof(decimal), "0.01", "500", ErrorMessage = "TotalProfitPercent must be greater than 0.")]
        public decimal TotalProfitPercent { get; set; }

        [Range(typeof(decimal), "0.01", "100", ErrorMessage = "PayoutFrequencyProfitRatePercent must be greater than 0.")]
        public decimal PayoutFrequencyProfitRatePercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "InvestmentTenure must be greater than 0.")]
        public int InvestmentTenure { get; set; }

        public bool IsPaymentScheduleAvailable { get; set; }

        public Guid Guid { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }

        public bool IsTdsApplicable { get; set; }

        [Range(0, 100, ErrorMessage = "TdsPercent must be between 0 and 100.")]
        public decimal TdsPercent { get; set; }

        public bool IsReferralBonusApplicable { get; set; }

        [Range(0, 100, ErrorMessage = "ReferralFirstBonusPercent must be between 0 and 100.")]
        public decimal ReferralFirstBonusPercent { get; set; }

        [Range(0, 100, ErrorMessage = "ReferralLastBonusPercent must be between 0 and 100.")]
        public decimal ReferralLastBonusPercent { get; set; }

        public bool IsMaturityBonusApplicable { get; set; }

        public string? BonusTime { get; set; }

        public decimal BonusPercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CapitalReturnPeriod must be greater than 0.")]
        public int CapitalReturnPeriod { get; set; }
        public int Status { get; set; }

        public bool IsJoiningBonusApplicable { get; set; }
        public decimal JoiningBonusPercent { get; set; }
        public int CapitalLockingReturnPeriod { get; set; }

        public string? ClosingComment { get; set; }
        public string? ClosedBy { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool? IsInvestmentMatured { get; set; }




        public List<InvestmentReceivedDetailDto> LstInvestmentReceivedDetails { get; set; }
        public List<ClientBankingDetailsDto>? LstClientBankingDetails { get; set; }



    }

    public class UpdateInvestmentsDto
    {
        public int InvestmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "ClientName is required.")]
        public string ClientName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PlanId must be greater than 0.")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "PlanName is required.")]
        public string PlanName { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999", ErrorMessage = "InvestmentAmount must be greater than 0.")]
        public decimal InvestmentAmount { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "InvestmentStartDate is required.")]
        public DateOnly InvestmentStartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "InvestmentEndDate is required.")]
        public DateOnly InvestmentEndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PayoutFrequency must be greater than 0.")]
        public int PayoutFrequency { get; set; }

        [Range(typeof(decimal), "0.01", "500", ErrorMessage = "TotalProfitPercent must be greater than 0.")]
        public decimal TotalProfitPercent { get; set; }

        [Range(typeof(decimal), "0.01", "100", ErrorMessage = "PayoutFrequencyProfitRatePercent must be greater than 0.")]
        public decimal PayoutFrequencyProfitRatePercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "InvestmentTenure must be greater than 0.")]
        public int InvestmentTenure { get; set; }

        public bool IsPaymentScheduleAvailable { get; set; }

        public Guid Guid { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }

        public bool IsTdsApplicable { get; set; }

        [Range(0, 100, ErrorMessage = "TdsPercent must be between 0 and 100.")]
        public decimal TdsPercent { get; set; }

        public bool IsReferralBonusApplicable { get; set; }

        [Range(0, 100, ErrorMessage = "ReferralFirstBonusPercent must be between 0 and 100.")]
        public decimal ReferralFirstBonusPercent { get; set; }

        [Range(0, 100, ErrorMessage = "ReferralLastBonusPercent must be between 0 and 100.")]
        public decimal ReferralLastBonusPercent { get; set; }

        public bool IsMaturityBonusApplicable { get; set; }

        public string? BonusTime { get; set; }

        public decimal BonusPercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CapitalReturnPeriod must be greater than 0.")]
        public int CapitalReturnPeriod { get; set; }
        public int Status { get; set; }
        public bool IsJoiningBonusApplicable { get; set; }
        public decimal JoiningBonusPercent { get; set; }
        public int CapitalLockingReturnPeriod { get; set; }
    }

}
