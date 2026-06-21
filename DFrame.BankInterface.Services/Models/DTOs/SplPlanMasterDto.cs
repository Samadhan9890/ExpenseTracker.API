public class SplPlanMasterDto
{
    public int PlanId { get; set; }
    public string PlanCode { get; set; }
    public string PlanName { get; set; }
    public string? Description { get; set; }
    public decimal MinInvestment { get; set; }
    public int PlanPeriod { get; set; }
    public int PayoutFrequency { get; set; }
    public decimal ProfitRate { get; set; }
    public bool IsBonusApplicable { get; set; }
    public string BonusPayoutTime { get; set; }
    public int BonusPercent { get; set; }
    public int CapitalReturnPeriod { get; set; }
    public DateTime CreatedDate { get; set; }

    public int CapitalLockingPeriod { get; set; }
    public bool IsJoiningBonusApplicable { get; set; }
    public int JoiningBonusPercent { get; set; }

    public bool Status { get; set; }
}
