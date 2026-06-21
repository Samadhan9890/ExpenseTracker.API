using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_LOC", Schema = "dbo")]
    public class TblLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("LOCATION_ID", TypeName = "INT")]
        public int LocationId { get; set; }

        [Column("LOCATION_CODE", TypeName = "VARCHAR(50)")]
        public string? LocationCode { get; set; }

        [Column("LOCATION_NAME", TypeName = "VARCHAR(100)")]
        public string? LocationName { get; set; }

        [Column("ADDRESS1", TypeName = "VARCHAR(100)")]
        public string? Address1 { get; set; }

        [Column("ADDRESS2", TypeName = "VARCHAR(100)")]
        public string? Address2 { get; set; }

        [Column("ADDRESS3", TypeName = "VARCHAR(100)")]
        public string? Address3 { get; set; }

        [Column("ADDRESS4", TypeName = "VARCHAR(100)")]
        public string? Address4 { get; set; }

        [Column("ADDRESS5", TypeName = "VARCHAR(100)")]
        public string? Address5 { get; set; }

        [Column("BUSINESS_UNIT")]
        public int? BusinessUnit { get; set; }

        [Column("ENTRY_ID")]
        public int? EntryId { get; set; }

        [Column("ENTRY_DATE")]
        public DateTime? EntryDate { get; set; }

        [Column("MODIFY_ID")]
        public int? ModifyId { get; set; }

        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }

        [Column("DELETE_ID")]
        public int? DeleteId { get; set; }

        [Column("DELETE_DATE")]
        public DateTime? DeleteDate { get; set; }

        [Column("STATUS")]
        public int? Status { get; set; }

        [Column("PRINT_ADDRESS")]
        public string? PrintAddress { get; set; }

        [Column("PINCODE")]
        public string? Pincode { get; set; }

        [Column("EMAIL_ID")]
        public string? EmailId { get; set; }

        [Column("CITY")]
        public string? City { get; set; }

        [Column("STATE_ID")]
        public int? StateId { get; set; }

        [Column("TELEPHONE")]
        public string? Telephone { get; set; }

    }
}


