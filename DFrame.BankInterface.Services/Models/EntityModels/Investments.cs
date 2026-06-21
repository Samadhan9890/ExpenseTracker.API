using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("Investments")]
    public class Investment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("InvestmentId")]
        public int InvestmentId { get; set; }

        [Column("Guid")]
        public Guid Guid { get; set; }

        [Column("ClientId")]
        public int ClientId { get; set; }

        [Column("ClientName", TypeName = "varchar(200)")]
        public string? ClientName { get; set; }

        [Column("PlanId")]
        public int PlanId { get; set; }

        [Column("PlanName", TypeName = "varchar(50)")]
        public string? PlanName { get; set; }

        [Column("InvestmentAmount", TypeName = "decimal(18,2)")]
        public decimal InvestmentAmount { get; set; }

        [Column("InvestmentStartDate", TypeName = "date")]
        public DateOnly InvestmentStartDate { get; set; }

        [Column("InvestmentEndDate", TypeName = "date")]
        public DateOnly InvestmentEndDate { get; set; }

        [Column("PayoutFrequency")]
        public int PayoutFrequency { get; set; }

        [Column("TotalProfitPercent", TypeName = "decimal(10,2)")]
        public decimal TotalProfitPercent { get; set; }

        [Column("PayoutFrequencyProfitRatePercent", TypeName = "decimal(10,2)")]
        public decimal PayoutFrequencyProfitRatePercent { get; set; }

        [Column("InvestmentTenure")]
        public int InvestmentTenure { get; set; }

        [Column("IsPaymentScheduleAvailable")]
        public bool IsPaymentScheduleAvailable { get; set; }
        

        [Column("CreatedDate", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy", TypeName = "varchar(30)")]
        public string CreatedBy { get; set; }

        [Column("IsTdsApplicable")]
        public bool IsTdsApplicable { get; set; }

        [Column("TdsPercent", TypeName = "decimal(10,2)")]
        public decimal TdsPercent { get; set; }

        [Column("IsReferralBonusApplicable")]
        public bool IsReferralBonusApplicable { get; set; }

        [Column("ReferralFirstBonusPercent", TypeName = "decimal(10,2)")]
        public decimal ReferralFirstBonusPercent { get; set; }

        [Column("ReferralLastBonusPercent", TypeName = "decimal(10,2)")]
        public decimal ReferralLastBonusPercent { get; set; }

        [Column("IsMaturityBonusApplicable")]
        public bool IsMaturityBonusApplicable { get; set; }

        [Column("BonusTime", TypeName = "varchar(10)")]
        public string BonusTime { get; set; }

        [Column("BonusPercent", TypeName = "decimal(10,2)")]
        public decimal BonusPercent { get; set; }

        [Column("CapitalReturnPeriod")]
        public int CapitalReturnPeriod { get; set; }

        [Column("CapitalLockingReturnPeriod")]
        public int CapitalLockingReturnPeriod { get; set; }


        [Column("IsJoiningBonusApplicable")]
        public bool IsJoiningBonusApplicable { get; set; }


        [Column("JoiningBonusPercent", TypeName = "decimal(10,2)")]
        public decimal JoiningBonusPercent { get; set; }

        [Column("Status")]
        public int Status { get; set; }
        [Column("BorrowLetter", TypeName = "varchar(max)")]
        public string? BorrowLetter { get; set; }
        [Column("BorrowLetterStatus")]
        public int BorrowLetterStatus { get; set; }
        [NotMapped]
        public string RefferedByName { get; set; }

        [Column("ClosingDate", TypeName = "datetime")]
        public DateTime? ClosingDate { get; set; }

        [Column("ClosedBy", TypeName = "varchar(30)")]
        public string? ClosedBy { get; set; }

        [Column("ClosingComment", TypeName = "varchar(255)")]
        public string? ClosingComment { get; set; }

        [NotMapped]
        public bool? IsInvestmentMatured { get; set; }


        [NotMapped]
        public List<InvestmentReceivedDetails> LstInvestmentReceivedDetails { get; set; }

        [NotMapped]
        public List<ClientBankingDetail> LstClientBankingDetails { get; set; }
        [NotMapped]
        public bool IsFullyPaid { get; internal set; }
    }
}