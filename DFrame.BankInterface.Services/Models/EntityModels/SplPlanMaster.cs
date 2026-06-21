using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("SPL_PLAN_MASTER")]
    public class SplPlanMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanId { get; set; }

        [Column("PLAN_CODE", TypeName = "varchar(30)")]
        public string PlanCode { get; set; }

        [Column("PLAN_NAME", TypeName = "varchar(50)")]
        public string PlanName { get; set; }

        [Column("DESCRIPTION", TypeName = "varchar(100)")]
        public string? Description { get; set; }

        [Column("MIN_INVESTMENT", TypeName = "decimal(18,2)")]
        public decimal MinInvestment { get; set; }

        [Column("PLAN_PERIOD")]
        public int PlanPeriod { get; set; }

        [Column("PAYOUT_FREQUENCY")]
        public int PayoutFrequency { get; set; }

        [Column("PROFIT_RATE", TypeName = "decimal(5,2)")]
        public decimal ProfitRate { get; set; }

        [Column("IS_BONUS_APPLICABLE")]
        public bool IsBonusApplicable { get; set; }

        [Column("BONUS_PAYOUT_TIME")]
        public string BonusPayoutTime { get; set; } = "end";

        [Column("BONUS_PERCENT")]
        public int BonusPercent { get; set; }

        [Column("CAPITAL_RETURN_PERIOD")]
        public int CapitalReturnPeriod { get; set; }

        [Column("CAPITAL_LOCKING_PERIOD")]
        public int CapitalLockingPeriod { get; set; }

        [Column("IS_JOINING_BONUS_APPLICABLE")]
        public bool IsJoiningBonusApplicable { get; set; }

        [Column("JOINING_BONUS_PERCENT")]
        public int JoiningBonusPercent { get; set; }

        [Column("CREATED_DATE", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("STATUS")]
        public bool Status { get; set; }
    }
}
