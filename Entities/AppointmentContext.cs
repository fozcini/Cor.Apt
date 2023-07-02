using Microsoft.EntityFrameworkCore;

namespace Cor.Apt.Entities
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleId = 1, RoleName = "User" },
                new Role() { RoleId = 2, RoleName = "Admin" },
                new Role() { RoleId = 3, RoleName = "Master" }
            );

            modelBuilder.Entity<User>().HasData(
                new User() { UserId = 1, IdentificationNumber = "10203040501", Phone = "", Email = "", Password = "0a8ddfa6d4432303c28ebf79312cabd1d935c26da5985585f0c6a61bb4aa8492", IsActive = true, RoleId = 1 },
                new User() { UserId = 2, IdentificationNumber = "10203040502", Phone = "", Email = "", Password = "0a8ddfa6d4432303c28ebf79312cabd1d935c26da5985585f0c6a61bb4aa8492", IsActive = true, RoleId = 2 },
                new User() { UserId = 3, IdentificationNumber = "10203040503", Phone = "", Email = "", Password = "0a8ddfa6d4432303c28ebf79312cabd1d935c26da5985585f0c6a61bb4aa8492", IsActive = true, RoleId = 3 }
            );
        }

        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<AnalysisType> AnalysisTypes { get; set; }
        public DbSet<Anamnesis> Anamnesises { get; set; }
        public DbSet<Andulation> Andulations { get; set; }
        public DbSet<AndulationRecord> AndulationRecords { get; set; }
        public DbSet<Cryline> Crylines { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<BodyAnalysis> BodyAnalyses { get; set; }
        public DbSet<DecisionAndTracing> DecisionAndTracings { get; set; }
        public DbSet<DecisionAndTracingType> DecisionAndTracingTypes { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Ozone> Ozones { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<PerformanceNote> PerformanceNotes { get; set; }
        public DbSet<PhysicalExamination> PhysicalExaminations { get; set; }
        public DbSet<Radiology> Radiologies { get; set; }
        public DbSet<RadiologyType> RadiologyTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SalaryPayment> SalaryPayments { get; set; }
        public DbSet<SalaryPaymentType> SalaryPaymentTypes { get; set; }
        public DbSet<SocialSecurity> SocialSecurities { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<RadiologyRequest> RadiologyRequests { get; set; }
        public DbSet<RadiologyRequestType> RadiologyRequestTypes { get; set; }
        public DbSet<RadiologyRequestTypeList> RadiologyRequestTypeLists { get; set; }
    }
}