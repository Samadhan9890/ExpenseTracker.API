using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_USER", Schema = "dbo")]
    public class TblUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("INTERNAL_USER_ID", TypeName = "int")]
        public int InternalUserId { get; set; }

        [Column("USER_LOGIN_CODE", TypeName = "varchar(30)")]
        public string? UserLoginCode { get; set; }

        [Column("USER_NAME", TypeName = "varchar(50)")]
        public string? UserName { get; set; }

        [Column("USER_PASSWORD", TypeName = "varchar(128)")]
        public string? UserPassword { get; set; }

        [Column("EMPLOYEE_CODE", TypeName = "varchar(30)")]
        public string? EmployeeCode { get; set; }

        [Column("LOCATION_ID")]
        public int? LocationId { get; set; }

        [Column("BUSINESS_UNIT")]
        public int? BusinessUnit { get; set; }

        [Column("MANGER_ID")]
        public int? MangerId { get; set; }

        [Column("USER_EMAIL_ID", TypeName = "varchar(100)")]
        public string? UserEmailId { get; set; }

        [Column("USER_ENTRY_CATEGORY")]
        public string? UserEntryCategory { get; set; }

        [Column("JOIN_DATE")]
        public DateTime? JoinDate { get; set; }


        [Column("STATUS", TypeName = "int")]
        public int? Status { get; set; }
     

        [Column("ENTRY_ID")]
        public int? EntryId { get; set; }

        [Column("MODIFY_ID")]
        public int? ModifyId { get; set; }

        [Column("ENTRY_DATE")]
        public DateTime? EntryDate { get; set; }

        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }

        [Column("DELETE_ID")]
        public int? DeleteId { get; set; }

        [Column("DELETE_DATE")]
        public DateTime? DeleteDate { get; set; }


        [Column("DEPARTMENT_ID")]
        public int? DepartmentId { get; set; }

        [Column("DEPARTMENT_NAME")]
        public string? DepartmentName { get; set; }

        [Column("BU_ACCESS")]
        public string? BuAccess { get; set; }

        [Column("BU_NAME")]
        public string? BuName { get; set; }

        [Column("USER_ACCESS")]
        public string? UserAccess { get; set; }

 


        [Column("DIVISION_NAME")]
        public string? DivisionName { get; set; }

        [Column("MANAGER_ID")]
        public int? ManagerId { get; set; }

        [Column("MANAGER_NAME")]
        public string? ManagerName { get; set; }



        [Column("ROLE_ACCESS")]
        public string? RoleAccess { get; set; }

        [Column("ITEM_ACCESS")]
        public string? ItemAccess { get; set; }


        [Column("ROLE_ACCESS_NAME")]
        public string? RoleAccessName { get; set; }

        [Column("DASHBOARD_ID")]
        public int? DashboardId { get; set; }

        [Column("PREV_PASSWORD")]
        public string? PrevPassword { get; set; }

        [Column("IS_LOCKED")]
        public bool? IsLocked { get; set; }

        [Column("PASSWORD_MODIFY_DATE")]
        public DateTime? PasswordModifyDate { get; set; }


        [Column("NO_OF_ATTEMPT")]
        public int? NoOfAttempt { get; set; }
              

        [Column("EMPLOYEE_CATEGORY")]
        public string? EmployeeCategory { get; set; }

        [Column("PAN_NO")]
        public string? PanNo { get; set; }

        [Column("MOBILE_NO")]
        public string? MobileNo { get; set; }

        [Column("ACCOUNT_NAME")]
        public string? AccountName { get; set; }

        [Column("ACCOUNT_NO")]
        public string? AccountNo { get; set; }

        [Column("IFSC_CODE")]
        public string? IfscCode { get; set; }

        [Column("BANK_NAME")]
        public string? BankName { get; set; }

		[Column("IS_MFA")]
		public bool? IS_MFA { get; set; }

		[Column("LAST_OTP")]
		public int? LAST_OTP { get; set; }
		[Column("OTP_EXP_DATE")]
		public DateTime? OtpExpiryDate { get; set; }

	}

    public class ChangePasswordModel
    {
        public string PrevPassword { get; set; }
        public string UserPassword { get; set; }

    }



}
