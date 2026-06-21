using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_ROLE", Schema = "dbo")]
    public class TblRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("ROLE_ID", TypeName = "int")] 
        public int RoleId { get; set; }

        [MaxLength(50)]
        [Required]
        [Column("ROLE_NAME", TypeName = "varchar(50)")] 
        public string RoleName { get; set; }

        [Column("ENTRY_ID", TypeName = "int")]
        public int? EntryId { get; set; }

        [Column("MODIFY_ID", TypeName = "int")]
        public int? ModifyId { get; set; }

        [Column("ENTRY_DATE", TypeName = "datetime")]
        public DateTime? EntryDate { get; set; }

        [Column("MODIFY_DATE", TypeName = "datetime")] 
        public DateTime? ModifyDate { get; set; }
        
        [Column("DELETE_ID", TypeName = "int")] 
        public int? DeleteId { get; set; }
        
        [Column("DELETE_DATE", TypeName = "datetime")] 
        public DateTime? DeleteDate { get; set; }
        
        [Column("STATUS", TypeName = "bit")] 
        public bool? Status { get; set; }
        
        [Column("BU_ID", TypeName = "int")] 
        public int? BuId { get; set; }
        
        [Column("ROLE_DESCRIPTION", TypeName = "varchar(MAX)")] 
        public string? RoleDescription { get; set; }
    }
}
