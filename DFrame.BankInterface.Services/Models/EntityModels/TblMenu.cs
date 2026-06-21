using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("TBL_MENU", Schema = "dbo")]
	public class TblMenu
	{	
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MENU_ID", TypeName = "int")]
        public int MENU_ID { get; set; }

        [Column("MENU_DESC", TypeName = "VARCHAR(50)")]
        public string? MENU_DESC { get; set; }

        [Column("MENU_URL", TypeName = "VARCHAR(5000)")]
        public string? MENU_URL { get; set; }

        [Column("PARENT_MENU", TypeName = "int")]
        public Nullable<int> PARENT_MENU { get; set; }

        [Column("MENU_LEVEL", TypeName = "int")]
        public Nullable<int> MENU_LEVEL { get; set; }

        [Column("ENTRY_ID", TypeName = "int")]
        public Nullable<int> ENTRY_ID { get; set; }

        [Column("MODIFY_ID", TypeName = "int")]
        public Nullable<int> MODIFY_ID { get; set; }

        [Column("ENTRY_DATE", TypeName = "datetime2")]
        public DateTime? ENTRY_DATE { get; set; }

        [Column("MODIFY_DATE", TypeName = "datetime2")]
        public DateTime? MODIFY_DATE { get; set; }

        [Column("DELETE_ID", TypeName = "int")]
        public Nullable<int> DELETE_ID { get; set; }

        [Column("DELETE_DATE", TypeName = "datetime2")]
        public DateTime? DELETE_DATE { get; set; }

        [Column("STATUS", TypeName = "bit")]
        public bool? STATUS { get; set; }

        [Column("ORDER_LEVEL", TypeName = "int")]
        public Nullable<int> ORDER_LEVEL { get; set; }

        [Column("Controller", TypeName = "VARCHAR(500)")]
        public string? Controller { get; set; }

        [Column("Action", TypeName = "VARCHAR(500)")]
        public string? Action { get; set; }
    }
}
