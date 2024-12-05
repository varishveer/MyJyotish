using Microsoft.EntityFrameworkCore;
using ModelAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DbServices
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options) { }
        public DbSet<AdminModel> AdminRecords { get; set; }
        public DbSet<JyotishModel> JyotishRecords { get; set; }
      /*  public DbSet<PendingJyotishModel> PendingJyotishRecords { get; set; }*/
        public DbSet<UserModel> Users { get; set; }
        //public DbSet<UserTempModel> TempUser { get; set;  }
        public DbSet<AppointmentModel> AppointmentRecords { get; set; }
        public DbSet<TeamMemberModel> TeamMemberRecords { get; set; }
        public DbSet<PoojaListModel> PoojaList { get; set; }
        public DbSet<ExpertiseModel> ExpertiseRecords { get; set; } 
        public DbSet<CallingModel> CallingRecords { get; set; }
        public DbSet<ChattingModel> ChatingRecords { get; set; }

        public DbSet<PoojaRecordModel> PoojaRecord { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }
        public DbSet<SliderImagesModel> Sliders { get; set; }
        public DbSet<LanguageModel> Languages { get; set; }
        public DbSet<SlotModel> Slots { get; set; }
        public DbSet<SlotBookingModel> SlotBooking { get; set; }
        public DbSet<SpecializationListModel> SpecializationList { get; set; }
        public DbSet<JyotishGalleryModel> JyotishGallery { get; set; }
        public DbSet<JyotishVideosModel> JyotishVideos { get; set; }
        public DbSet<SubscriptionFeaturesModel> SubscriptionFeatures { get; set; }
        public DbSet<SubscriptionModel> Subscriptions { get; set; }
        public DbSet<ManageSubscriptionModel> ManageSubscriptionModels { get; set; }
        public DbSet<UserPaymentRecordModel> UserPaymentRecord { get; set; }
        public DbSet<JyotishPaymentRecordModel> JyotishPaymentRecord { get; set; }
        public DbSet<AppointmentSlotModel> AppointmentSlots { get; set; }
        public DbSet<ChatModel> Chat { get; set; }
        public DbSet<ChatedUser> ChatedUser { get; set; }
        public DbSet<ProblemSolutionModel> ProblemSolution { get; set; }
        public DbSet<jyotishWallet> JyotishWallets { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<JyotishUserAttachmentModel> JyotishUserAttachmentRecord { get; set; }
        public DbSet<WalletHistoryModel> WalletHistroy { get; set; }
        public DbSet<JyotishTempRecord> jyotishTempRecords { get; set; }
        public DbSet<ClientMembers> ClientMembers { get; set; }
        public DbSet<SubsciptionManagementModel> PackageManager { get; set; }
        public DbSet<CountryCode> CountryCode { get; set; }
        public DbSet<InterviewFeedbackModel> InterviewFeedback { get; set; }
        public DbSet<redeamCode> RedeamCode { get; set; }
        public DbSet<JyotishRatingModel> JyotishRating { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<levels> Levels { get; set; }
        public DbSet<EmployeesDocs> EmployeeDocs { get; set; }
        public DbSet<EmployeesAccessPages> EmployeeAccessPages { get; set; }
        public DbSet<DepartmentPagesValidation> DepartmentPagesAccess { get; set; }
        public DbSet<levelsAccessValidation> LevelAccessValidation { get; set; }
        public DbSet<InterviewMeeting> InterviewMeeting { get; set; }
        public DbSet<EmployeeInterviewFeedbackModel> EmployeeInterviewFeedback { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
          
            modelBuilder.Entity<AppointmentModel>().HasOne(c => c.JyotishRecord).WithMany(j => j.AppointmentRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<AppointmentModel>().HasOne(c => c.UserRecord).WithMany(j => j.AppointmentRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<ChatedUser>().HasOne(c => c.Jyotish).WithMany(j => j.ChatedUserRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<ChatedUser>().HasOne(c => c.User).WithMany(j => j.ChatedUserRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CallingModel>().HasOne(c => c.Jyotish).WithMany(j => j.CallingModelRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<CallingModel>().HasOne(c => c.User).WithMany(j => j.CallingModelRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<ChattingModel>().HasOne(c => c.Jyotish).WithMany(j => j.ChattingModelRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<ChattingModel>().HasOne(c => c.User).WithMany(j => j.ChattingModelRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<JyotishModel>().HasOne(j => j.DocumentModel).WithOne(d => d.Jyotish).HasForeignKey<DocumentModel>(d => d.JId);

          
            modelBuilder.Entity<SlotBookingModel>().HasOne(c => c.JyotishRecords).WithMany(j => j.Slots).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<SlotBookingModel>().HasOne(c => c.SlotRecords).WithMany(j => j.SlotBookingRecords).HasForeignKey(c => c.SlotId);


            modelBuilder.Entity<JyotishVideosModel>().HasOne(c => c.Jyotish).WithMany(j => j.JyotishVideos).HasForeignKey(c => c.JyotishId);
            modelBuilder.Entity<JyotishGalleryModel>().HasOne(c => c.Jyotish).WithMany(j => j.JyotishGallery).HasForeignKey(c => c.JyotishId);
            modelBuilder.Entity<ManageSubscriptionModel>().HasOne(c => c.Feature).WithMany(j => j.ManageSubscription).HasForeignKey(c => c.FeatureId);

            modelBuilder.Entity<ManageSubscriptionModel>().HasOne(c => c.Subscription).WithMany(j => j.ManageSubscription).HasForeignKey(c => c.SubscriptionId);

            modelBuilder.Entity<jyotishWallet>().HasOne(c => c.jyotish).WithMany(j => j.JyotishRecord).HasForeignKey(c => c.jyotishId);
            
            modelBuilder.Entity<UserWallet>().HasOne(c => c.User).WithMany(j => j.UserRecord).HasForeignKey(c => c.userId);
            
            modelBuilder.Entity<UserPaymentRecordModel>().HasOne(c => c.User).WithMany(j => j.UserPaymentRecords).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<JyotishPaymentRecordModel>().HasOne(c => c.Jyotish).WithMany(j => j.jyotishPaymentRecords).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<WalletHistoryModel>().HasOne(c => c.jyotish).WithMany(j => j.JytoishWalletHistoryRecord).HasForeignKey(c => c.JId);
                        
            modelBuilder.Entity<WalletHistoryModel>().HasOne(c => c.Users).WithMany(j => j.UserWalletHistoryRecords).HasForeignKey(c => c.UId);

            modelBuilder.Entity<ClientMembers>().HasOne(c => c.user).WithMany(j => j.memebers).HasForeignKey(c => c.UId);

            modelBuilder.Entity<ProblemSolutionModel>().HasOne(c => c.Member).WithMany(j => j.Solution).HasForeignKey(c => c.memberId);
            modelBuilder.Entity<JyotishUserAttachmentModel>().HasOne(c => c.Member).WithMany(j => j.Attachment).HasForeignKey(c => c.MemberId);

            modelBuilder.Entity<ProblemSolutionModel>().HasOne(c => c.Member).WithMany(j => j.Solution).HasForeignKey(c => c.memberId);
            modelBuilder.Entity<AppointmentSlotModel>()
       .HasOne(c => c.JyotishData)  // Each AppointmentSlot is linked to one Jyotish
       .WithMany(j => j.AppointmentSlotData) // Each Jyotish can have multiple AppointmentSlots
       .HasForeignKey(c => c.JyotishId);

            // Configure AppointmentModel and AppointmentSlotModel relationship (Many-to-One)
            modelBuilder.Entity<AppointmentModel>()
                .HasOne(a => a.AppointmentSlotData)   // Each AppointmentModel has one AppointmentSlot
                .WithMany(s => s.AppointmentData)     // Each AppointmentSlot can have multiple AppointmentModels
                .HasForeignKey(a => a.SlotId);

            // Optionally, configure cascading delete behavior if required
            modelBuilder.Entity<AppointmentSlotModel>()
                .HasOne(c => c.JyotishData)
                .WithMany(j => j.AppointmentSlotData)
                .HasForeignKey(c => c.JyotishId);
            
            modelBuilder.Entity<JyotishUserAttachmentModel>()
                .HasOne(c => c.Appointment)
                .WithMany(j => j.JyotishUserAttachmentRecords)
                .HasForeignKey(c => c.AppointmentId);
                
            modelBuilder.Entity<SubsciptionManagementModel>()
                .HasOne(c => c.jyotish)
                .WithMany(j => j.subscriptionManage)
                .HasForeignKey(c => c.JyotishId);

                 modelBuilder.Entity<SubsciptionManagementModel>()
                .HasOne(c => c.subscription)
                .WithMany(j => j.subscriptionManage)
                .HasForeignKey(c => c.SubscriptionId);

            modelBuilder.Entity<CountryCode>().HasOne(c => c.countryobj).WithOne(j => j.code).HasForeignKey<CountryCode>(c => c.country);

            modelBuilder.Entity<InterviewFeedbackModel>()
               .HasOne(c => c.Slot)
               .WithMany(j => j.Feedback)
               .HasForeignKey(c => c.InterviewId);

            modelBuilder.Entity<InterviewFeedbackModel>()
              .HasOne(c => c.Jyotish)
              .WithMany(j => j.Feedback)
              .HasForeignKey(c => c.JyotishId); 

            modelBuilder.Entity<JyotishModel>()
              .HasOne(c => c.countryCoderecord)
              .WithMany(j => j.jyotish)
              .HasForeignKey(c => c.countryCode);
            modelBuilder.Entity<redeamCode>()
              .HasOne(c => c.jyotish)
              .WithMany(j => j.redeamCode)
              .HasForeignKey(c => c.jyotishId);

            modelBuilder.Entity<JyotishRatingModel>()
              .HasOne(c => c.Jyotish)
              .WithMany(j => j.JyotishRating)
              .HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<JyotishRatingModel>()
            .HasOne(c => c.User)
            .WithMany(j => j.JyotishRating)
            .HasForeignKey(c => c.UserId); 
            
            modelBuilder.Entity<Employees>()
            .HasOne(c => c.LevelsRelation)
            .WithMany(j => j.employees)
            .HasForeignKey(c => c.levels); 
            
            modelBuilder.Entity<Employees>()
            .HasOne(c => c.departmentRelation)
            .WithMany(j => j.employees)
            .HasForeignKey(c => c.Department);
            
            modelBuilder.Entity<EmployeesDocs>()
            .HasOne(c => c.employeeRecord)
            .WithMany(j => j.employeeDocs)
            .HasForeignKey(c => c.employees); 
            modelBuilder.Entity<DepartmentPagesValidation>()
            .HasOne(c => c.department)
            .WithMany(j => j.EmpPages)
            .HasForeignKey(c => c.DepartmentId);

            modelBuilder.Entity<DepartmentPagesValidation>()
            .HasOne(c => c.pages)
            .WithMany(j => j.EmpPages)
            .HasForeignKey(c => c.PageId);
            
            modelBuilder.Entity<levelsAccessValidation>()
            .HasOne(c => c.departmentRelation)
            .WithMany(j => j.LevelAccess)
            .HasForeignKey(c => c.department);
            modelBuilder.Entity<levelsAccessValidation>()
            .HasOne(c => c.EmpPages)
            .WithMany(j => j.LevelAccess)
            .HasForeignKey(c => c.pages);
            modelBuilder.Entity<levelsAccessValidation>()
            .HasOne(c => c.LevelsRelation)
            .WithMany(j => j.LevelAccess)
            .HasForeignKey(c => c.levels);


            modelBuilder.Entity<InterviewMeeting>()
           .HasOne(c => c.Jyotish)
           .WithMany(j => j.InterviewMeeting)
           .HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<InterviewMeeting>()
           .HasOne(c => c.Employee)
           .WithMany(j => j.InterviewMeeting)
           .HasForeignKey(c => c.EmployeeId);
            
            
            modelBuilder.Entity<EmployeeInterviewFeedbackModel>()
           .HasOne(c => c.SlotBooking)
           .WithMany(j => j.EmployeeInterviewFeedbacks)
           .HasForeignKey(c => c.SlotBookingId);

            modelBuilder.Entity<EmployeeInterviewFeedbackModel>()
           .HasOne(c => c.Employee)
           .WithMany(j => j.EmployeeInterviewFeedbacks)
           .HasForeignKey(c => c.EmployeeId);

        }


    }

    
}
