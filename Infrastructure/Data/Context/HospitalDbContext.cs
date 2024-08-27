using Domain.Entities;
using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class HospitalDbContext : DbContext
    {
        //app
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<ClinicModule> ClinicModules { get; set; }
        public virtual DbSet<AppointmentChangesLog> AppointmentChangesLogs { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }
        // security
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }





        public HospitalDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder); // if we put this at top, it changes also identity table names so dont 
            builder.Entity<User>()
                .HasMany(d => d.Specialities)
                .WithMany(s => s.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserSpeciality",
                    j => j.HasOne<Speciality>().WithMany().HasForeignKey("speciality_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("user_id"),
                    j =>
                    {
                        j.ToTable("user_specialities");
                        // Additional configuration for the join table can go here
                    });
            builder.Entity<User>()
                .HasMany(d => d.Clinics)
                .WithMany(c => c.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserClinic",
                    j => j.HasOne<Clinic>().WithMany().HasForeignKey("clinic_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("user_id"),
                    j =>
                    {
                        j.ToTable("user_clinics"); // Custom join table name
                        // Additional configuration for the join table can go here
                    });
            //     builder.Entity<MessageTemplate>()
            //        .HasMany(d => d.Clinics)
            //       .WithMany(c => c.MessageTemplates)
            //       .UsingEntity<Dictionary<string, object>>(
            //          "MessageTemplateClinic",
            //          j => j.HasOne<Clinic>().WithMany().HasForeignKey("clinic_id"),
            //          j => j.HasOne<MessageTemplate>().WithMany().HasForeignKey("message_template_id"),
            //          j =>
            //         {
            //            j.ToTable("message_template_clinics"); // Custom join table name
            // Additional configuration for the join table can go here
            //        });

            //builder.Entity<Clinic>()
            //   .HasMany(c => c.Modules)
            //   .WithMany(m => m.Clinics)
            //   .UsingEntity<ClinicModule>(
            //       j => j.HasOne<Module>().WithMany().HasForeignKey(cm => cm.ModuleId),
            //       j => j.HasOne<Clinic>().WithMany().HasForeignKey(cm => cm.ClinicId))
            //       ;

            //ContextSeed.Seed(builder);
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Doctor>()
        //        .HasMany(d => d.Specialities)
        //        .WithMany(s => s.Doctors)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "DoctorSpeciality",
        //            j => j.HasOne<Speciality>().WithMany().HasForeignKey("speciality_id"),
        //            j => j.HasOne<Doctor>().WithMany().HasForeignKey("doctor_id"),
        //            j =>
        //            {
        //                j.ToTable("doctor_specialities");
        //                // Additional configuration for the join table can go here
        //            });
        //}

    }
}
