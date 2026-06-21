using ExpenseTracker.Services.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Migrations;

namespace ExpenseTracker.Services.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> Options) : base(Options)
        {
                
        }
        public DbSet<TblUser> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<TblBusinessUnit> TblBusinessUnits { get; set; }
		public DbSet<TblMenu> Menus { get; set; }
		public DbSet<TblMenuAccess> MenuAccess { get; set; }
		public DbSet<TblStatus> Status { get; set; }
		//public DbSet<TblPaymentDetails> PaymentDetails { get; set; }
		public DbSet<TblBankMaster> BankMasters { get; set; }
		//public DbSet<TblBankFileGenDetails> BankFileGenDetails { get; set; }
        public DbSet<TblRole> RoleMasters { get; set; }
		public DbSet<TblCustomReportMaster> CustomReportMaster { get; set; }
        public DbSet<TblApplicationParameter> ApplicationParameter { get; set; }

        public DbSet<TblLocation> LocationMasters { get; set; }

        public DbSet<TblDepartment> DepartmentMasters { get; set; }
		public DbSet<PlanMaster> PlanMasters { get; set; }
		public DbSet<SubPlansMaster> SubPlansMasters { get; set; }

		public DbSet<ClientMaster> ClientMasters { get; set; }
		
		public DbSet<TblSubscription> Subscriptions { get; set; }
        public DbSet<TblPaymentSchedule> PaymentSchedules { get; set; }
        public DbSet<TblAuditTrail> AuditTrail { get; set; }

        public DbSet<TblBorrowLetterDetails> BorrowLetterDetails { get; set; }
        public DbSet<BusinessDevTeam> BusinessDevTeam { get; set; }
        public DbSet<PaymentProof> paymentProofs { get; set; }
        public DbSet<AuditTrail> NcAuditTrail { get; set; }
        public DbSet<SplPlanMaster> SplPlanMaster { get; set; }

        public DbSet<Investment> Investments { get; set; }
        public DbSet<InvestmentReceivedDetails> InvestmentReceivedDetails { get; set; }
        public DbSet<ClientBankingDetail> ClientBankingDetails { get; set; }
        public DbSet<Models.EntityModels.SplPaymentSchedule> SplPaymentSchedules { get;  set; }
        public DbSet<ExternalPayment> ExternalPayments { get;  set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblUser>().HasIndex(u => u.InternalUserId).IsUnique();
			modelBuilder.Entity<TblMenu>().HasIndex(u => u.MENU_ID).IsUnique();
			modelBuilder.Entity<TblMenuAccess>().HasIndex(u => u.MenuId).IsUnique();
			modelBuilder.Entity<TblPaymentDetails>().Property(p => p.Id).ValueGeneratedOnAdd();			
			modelBuilder.Entity<TblBankMaster>().Property(b => b.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<TblBankMaster>().Property(p => p.EntryDate).HasDefaultValueSql("GETDATE()");			
			modelBuilder.Entity<TblRole>().Property(b => b.RoleId).ValueGeneratedOnAdd();
            modelBuilder.Entity<TblCustomReportMaster>().Property(b => b.CustomReportId).ValueGeneratedOnAdd();
            modelBuilder.Entity<TblApplicationParameter>(builder => { builder.HasKey(u => u.ParameterId); });
            modelBuilder.Entity<TblLocation>().Property(b => b.LocationId).ValueGeneratedOnAdd();
            modelBuilder.Entity<TblDepartment>().Property(b => b.DepartmentId).ValueGeneratedOnAdd();
			modelBuilder.Entity<TblStatus>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TblAuditTrail>().HasKey(k => k.Id);
            modelBuilder.Entity<TblAuditTrail>().Property(k => k.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TblAuditTrail>().Property(k => k.ActionDate).HasDefaultValueSql("getdate()");


            modelBuilder.Entity<PlanMaster>(entity =>
			{
				entity.Property(e => e.PlanCode)
					.HasColumnName("PLAN_CODE")
					.HasColumnType("varchar(30)");

				entity.Property(e => e.PlanName)
					.HasColumnName("PLAN_NAME")
					.HasColumnType("varchar(50)");

				entity.Property(e => e.Description)
					.HasColumnName("DESCRIPTION")
					.HasColumnType("varchar(100)");

				entity.Property(e => e.CreatedDate)
					.HasColumnName("CREATED_DATE")
					.HasColumnType("datetime")
					.HasDefaultValueSql("current_timestamp");

				entity.HasKey(e => e.PlanCode);
				
				entity.Property(e => e.PlanId)
					.ValueGeneratedOnAdd();
			});

			modelBuilder.Entity<SubPlansMaster>(entity =>
			{
				entity.Property(e => e.CreatedDate)
					.HasColumnName("CREATED_DATE")
					.HasColumnType("datetime")
					.HasDefaultValueSql("current_timestamp");

				entity.HasKey(e => e.SubPlansId);
			});


			modelBuilder.Entity<ClientMaster>(entity =>
			{
				entity.Property(e => e.ClientId)
					.HasColumnName("CLIENT_ID")
					.UseIdentityColumn(10000, 1);

				entity.Property(e => e.CreatedDate)
					.HasColumnName("CREATED_DATE")
					.HasColumnType("datetime")
					.HasDefaultValueSql("current_timestamp");
				
			});

			modelBuilder.Entity<ClientMaster>()
			.HasMany(c => c.Subscriptions)
			.WithOne(s => s.ClientMaster)
			.HasForeignKey(s => s.ClientId)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<TblSubscription>()
			.HasKey(s => s.SubscriptionId);

			modelBuilder.Entity<TblSubscription>()
				.Property(s => s.CreatedDate)
				.HasDefaultValueSql("GETDATE()");

			

			modelBuilder.Entity<TblSubscription>()
				.HasOne(s => s.ClientMaster)
				.WithMany()
				.HasForeignKey(s => s.ClientId)
				.OnDelete(DeleteBehavior.Cascade);

			//modelBuilder.Entity<TblSubscription>()
			//	.HasOne(s => s.PlanMaster)
			//	.WithMany()
			//	.HasForeignKey(s => s.PlanCode)
			//	.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<TblPaymentSchedule>()
				.Property(s => s.CreatedDate)
				.HasDefaultValueSql("GETDATE()");

			modelBuilder.Entity<ClientMaster>()
				.Property(s => s.Guid)
				.HasDefaultValueSql("NEWID()");

			modelBuilder.Entity<TblSubscription>()
				.Property(s => s.Guid)
				.HasDefaultValueSql("NEWID()");

			modelBuilder.Entity<TblSubscription>()
				.Property(s => s.BorrowLetterStatus)
				.HasDefaultValue(0);

			modelBuilder.Entity<BusinessDevTeam>()
			.HasKey(s => s.BDId);

			modelBuilder.Entity<TblUser>().Property(s => s.IS_MFA).HasDefaultValue(false);

            modelBuilder.Entity<AuditTrail>()
                .Property(s => s.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<SplPlanMaster>()
            .HasKey(s => s.PlanId);


            modelBuilder.Entity<SplPlanMaster>()
                .Property(s => s.CreatedDate)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Investment>(entity =>
            {
                entity.Property(e => e.InvestmentId)                    
                    .UseIdentityColumn(10000, 1);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp");

				entity.Property(e => e.Guid)
				.HasDefaultValueSql("NEWID()");

                entity.Property(e => e.BonusTime)
                .HasDefaultValue("end");

            });

            modelBuilder.Entity<InvestmentReceivedDetails>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityColumn(1, 1);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp");              

            });

            modelBuilder.Entity<ClientBankingDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");               
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Category)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.DateSpent)
                      .IsRequired();
            });
        }

    }
}
