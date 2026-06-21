using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TBL_BUSINESS_UNIT", Schema = "dbo")]
public class TblBusinessUnit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("BUSINESS_UNIT_ID", TypeName = "int")]
    public int BusinessUnitID { get; set; }

    [Required]
    [Column("BUSINESS_UNIT_CODE")]
    public string BusinessUnitCode { get; set; }

    [Required]
    [Column("BU_CODE")]
    public string BUCode { get; set; }

    
    [Column("BU_NAME")]
    public string? BUName { get; set; }

    [Column("STATUS")]
    public bool? Status { get; set; }

    [Column("ENTRY_ID", TypeName = "int")]
    public int? EntryID { get; set; }

    [Column("ENTRY_DATE")]
    public DateTime? EntryDate { get; set; }

    [Column("MODIFY_ID", TypeName = "int")]
    public int? ModifyID { get; set; }

    [Column("MODIFY_DATE")]
    public DateTime? ModifyDate { get; set; }

    [Column("DELETE_ID", TypeName = "int")]
    public int? DeleteID { get; set; }

    [Column("DELETE_DATE")]
    public DateTime? DeleteDate { get; set; }

    
    [Column("UPLOAD_PATH")]
    public string? UploadPath { get; set; }

    
    [Column("DOS_TEMPLATE")]
    public string? DOSTemplate { get; set; }

    
    [Column("SUN_CODE")]
    public string? SunCode { get; set; }

    
    [Column("DIVISION_ACCESS")]
    public string? DivisionAccess { get; set; }

    
    [Column("DEPARTMENT_ACCESS")]
    public string? DepartmentAccess { get; set; }

    
    [Column("LOCATION_ACCESS")]
    public string? LocationAccess { get; set; }

    [Column("ENTITY_ID", TypeName = "int")]
    public int? EntityID { get; set; }

    
    [Column("ENITY_NAME")]
    public string? EntityName { get; set; }

    
    [Column("SUN_WORK_LOCATION_T2")]
    public string? SunWorkLocationT2 { get; set; }

    
    [Column("SUN_ACCOUNTING_LOCATION_T1")]
    public string? SunAccountingLocationT1 { get; set; }

    [Column("DIVISION_ID", TypeName = "int")]
    public int? DivisionID { get; set; }

    
    [Column("DIVISION_NAME")]
    public string? DivisionName { get; set; }

   
    [Column("BU_ADDRESS")]
    public string? BUAddress { get; set; }

   
    [Column("PAN_NO")]
    public string? PANNo { get; set; }

   
    [Column("CIN_NO")]
    public string? CINNo { get; set; }

   
    [Column("TIN_NO")]
    public string? TINNo { get; set; }

    
    [Column("ST_NO")]
    public string? STNo { get; set; }

    
    [Column("NEFT_DETAILS")]
    public string? NEFTDetails { get; set; }

   
    [Column("SUN_EMP_CASH_CODE")]
    public string? SunEmpCashCode { get; set; }

    
    [Column("SUN_INTERCOMPANY")]
    public string? SunIntercompany { get; set; }

    [Column("HIS_LOCATION_CODE", TypeName = "int")]
    public int? HisLocationCode { get; set; }

    
    [Column("SUN_INTER_UNIT")]
    public string? SunInterUnit { get; set; }

    [Column("DEDUCTION_CAP_AMT", TypeName = "decimal(18, 2)")]
    public decimal? DeductionCapAmt { get; set; }

    [Column("DEDUCTION_CAP_PER", TypeName = "decimal(18, 2)")]
    public decimal? DeductionCapPer { get; set; }

    
    [Column("STATE_NUMBER")]
    public string? StateNumber { get; set; }

   
    [Column("COUNTRY_ID")]
    public string? CountryID { get; set; }

    
    [Column("GSTIN")]
    public string? GSTIN { get; set; }

    
    [Column("STATE_CODE")]
    public string? StateCode { get; set; }

    [Column("IACODE")]
    public string? IACode { get; set; }
}
