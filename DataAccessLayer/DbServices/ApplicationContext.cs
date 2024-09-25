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
        public DbSet<PoojaCategoryModel> PoojaCategory { get; set; }
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallingModel>().HasOne(c => c.Jyotish).WithMany(j => j.CallingModelRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<CallingModel>().HasOne(c => c.User).WithMany(j => j.CallingModelRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<ChattingModel>().HasOne(c => c.Jyotish).WithMany(j => j.ChattingModelRecord).HasForeignKey(c => c.JyotishId);

            modelBuilder.Entity<ChattingModel>().HasOne(c => c.User).WithMany(j => j.ChattingModelRecord).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<JyotishModel>().HasOne(j => j.DocumentModel).WithOne(d => d.Jyotish).HasForeignKey<DocumentModel>(d => d.JId);

            /* modelBuilder.Entity<PoojaCategoryModel>().HasOne(j => j.PoojaListModel).WithOne(d => d.PoojaCategoryModel).HasForeignKey<PoojaListModel>(d => d.PoojaCategoryId);*/

            modelBuilder.Entity<PoojaCategoryModel>().HasMany(p =>p.PoojaRecordModel).WithOne(pl => pl.PoojaCategoryModel).HasForeignKey(pl => pl.PoojaCategoryId);
            modelBuilder.Entity<SlotBookingModel>()
     .HasOne(c => c.JyotishRecords)
     .WithMany(j => j.Slots)
     .HasForeignKey(c => c.JyotishId);


        }


    }

    
}
