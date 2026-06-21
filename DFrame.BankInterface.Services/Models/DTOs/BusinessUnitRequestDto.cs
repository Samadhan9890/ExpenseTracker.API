using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class BusinessUnitRequestDto
    {
        public int BusinessUnitID { get; set; }
        [Required]
        public string BusinessUnitCode { get; set; }
        [Required]
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public bool? Status { get; set; }
        public int? EntryID { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? ModifyID { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DeleteID { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string UploadPath { get; set; }
        public string DOSTemplate { get; set; }
        public string SunCode { get; set; }
        public string DivisionAccess { get; set; }
        public string DepartmentAccess { get; set; }
        public string LocationAccess { get; set; }
        public int? EntityID { get; set; }
        public string EntityName { get; set; }
        public string SunWorkLocationT2 { get; set; }
        public string SunAccountingLocationT1 { get; set; }
        public int? DivisionID { get; set; }
        public string DivisionName { get; set; }
        public string BUAddress { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }
        public string TINNo { get; set; }
        public string STNo { get; set; }
        public string NEFTDetails { get; set; }
        public string SunEmpCashCode { get; set; }
        public string SunIntercompany { get; set; }
        public int? HisLocationCode { get; set; }
        public string SunInterUnit { get; set; }
        public decimal? DeductionCapAmt { get; set; }
        public decimal? DeductionCapPer { get; set; }
        public string StateNumber { get; set; }
        public string CountryID { get; set; }
        public string GSTIN { get; set; }
        public string StateCode { get; set; }
        public string IACode { get; set; }
    }
}
