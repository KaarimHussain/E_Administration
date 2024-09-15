using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_Administration.Models;

public partial class EAdministrationContext : DbContext
{
    public EAdministrationContext()
    {
    }

    public EAdministrationContext(DbContextOptions<EAdministrationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalInfo> AdditionalInfos { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Hardware> Hardwares { get; set; }

    public virtual DbSet<HodCourseAssignTeacher> HodCourseAssignTeachers { get; set; }

    public virtual DbSet<HodInstitute> HodInstitutes { get; set; }

    public virtual DbSet<InsertedSoftware> InsertedSoftwares { get; set; }

    public virtual DbSet<Institute> Institutes { get; set; }

    public virtual DbSet<Lab> Labs { get; set; }

    public virtual DbSet<Pc> Pcs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleDay> ScheduleDays { get; set; }

    public virtual DbSet<Software> Softwares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=.;initial catalog=E_ADMINISTRATION;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__addition__3213E83FE8C23257");

            entity.ToTable("additional_info");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("address");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("profile_picture");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.AdditionalInfos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__additiona__user___46E78A0C");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.ComplaintsId).HasName("PK__complain__5C8661EEADF6A0A3");

            entity.ToTable("complaints");

            entity.Property(e => e.ComplaintsId).HasColumnName("complaints_id");
            entity.Property(e => e.ComplaintsResponse)
                .HasColumnType("text")
                .HasColumnName("complaints_response");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__complaint__user___5CD6CB2B");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__courses__8F1EF7AED72A5EAB");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseDuration)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("course_duration");
            entity.Property(e => e.CourseName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__courses__institu__58D1301D");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__departme__C223242251852875");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("department_name");
            entity.Property(e => e.InstituteId).HasColumnName("institute_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Departments)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__departmen__insti__06CD04F7");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__Files__6F0F98BF6B9BD826");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UploadedBy).HasMaxLength(100);
        });

        modelBuilder.Entity<Hardware>(entity =>
        {
            entity.HasKey(e => e.HardId).HasName("PK__hardware__2646D038C28A87DE");

            entity.ToTable("hardwares");

            entity.Property(e => e.HardId).HasColumnName("hard_id");
            entity.Property(e => e.HardwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("hardware_name");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");
            entity.Property(e => e.OsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("os_name");
            entity.Property(e => e.Processor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("processor");
            entity.Property(e => e.Ram).HasColumnName("ram");
            entity.Property(e => e.StorageCapacity).HasColumnName("storage_capacity");

            entity.HasOne(d => d.Institute).WithMany(p => p.Hardwares)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__hardwares__insti__2DE6D218");
        });

        modelBuilder.Entity<HodCourseAssignTeacher>(entity =>
        {
            entity.HasKey(e => e.AssignId).HasName("PK__hod_cour__32E5EDE605D52E27");

            entity.ToTable("hod_course_assign_teacher");

            entity.Property(e => e.AssignId).HasColumnName("assign_id");
            entity.Property(e => e.AssignAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assign_at");
            entity.Property(e => e.AssignBy).HasColumnName("assign_by");
            entity.Property(e => e.AssignTo).HasColumnName("assign_to");
            entity.Property(e => e.CourseId).HasColumnName("course_id");

            entity.HasOne(d => d.AssignByNavigation).WithMany(p => p.HodCourseAssignTeacherAssignByNavigations)
                .HasForeignKey(d => d.AssignBy)
                .HasConstraintName("FK__hod_cours__assig__72C60C4A");

            entity.HasOne(d => d.AssignToNavigation).WithMany(p => p.HodCourseAssignTeacherAssignToNavigations)
                .HasForeignKey(d => d.AssignTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__hod_cours__assig__73BA3083");

            entity.HasOne(d => d.Course).WithMany(p => p.HodCourseAssignTeachers)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__hod_cours__cours__74AE54BC");
        });

        modelBuilder.Entity<HodInstitute>(entity =>
        {
            entity.HasKey(e => e.HodId).HasName("PK__hod_inst__C9000CE086A603F4");

            entity.ToTable("hod_institute");

            entity.Property(e => e.HodId).HasColumnName("hod_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.InstituteId).HasColumnName("institute_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Department).WithMany(p => p.HodInstitutes)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__hod_insti__depar__05D8E0BE");

            entity.HasOne(d => d.Institute).WithMany(p => p.HodInstitutes)
                .HasForeignKey(d => d.InstituteId)
                .HasConstraintName("FK__hod_insti__insti__04E4BC85");

            entity.HasOne(d => d.User).WithMany(p => p.HodInstitutes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__hod_insti__user___03F0984C");
        });

        modelBuilder.Entity<InsertedSoftware>(entity =>
        {
            entity.HasKey(e => e.InstalledId).HasName("PK__inserted__96DF74FF96697EE6");

            entity.ToTable("inserted_software");

            entity.Property(e => e.InstalledId).HasColumnName("installed_id");
            entity.Property(e => e.PcId).HasColumnName("pc_id");
            entity.Property(e => e.SoftId).HasColumnName("soft_id");

            entity.HasOne(d => d.Pc).WithMany(p => p.InsertedSoftwares)
                .HasForeignKey(d => d.PcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inserted___pc_id__662B2B3B");

            entity.HasOne(d => d.Soft).WithMany(p => p.InsertedSoftwares)
                .HasForeignKey(d => d.SoftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inserted___soft___671F4F74");
        });

        modelBuilder.Entity<Institute>(entity =>
        {
            entity.HasKey(e => e.InstituteId).HasName("PK__institut__2A0A74D56F86E6A6");

            entity.ToTable("institute");

            entity.Property(e => e.InstituteId).HasColumnName("institute_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.InstituteAddress)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("text")
                .HasColumnName("institute_address");
            entity.Property(e => e.InstituteDescription)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("text")
                .HasColumnName("institute_description");
            entity.Property(e => e.InstituteName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("institute_name");
        });

        modelBuilder.Entity<Lab>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("PK__labs__66DE64DBB72DBD1B");

            entity.ToTable("labs");

            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Labs)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__labs__institute___634EBE90");
        });

        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.PcId).HasName("PK__pcs__1D3A69C00AE61F41");

            entity.ToTable("pcs");

            entity.Property(e => e.PcId).HasColumnName("pc_id");
            entity.Property(e => e.HardId).HasColumnName("hard_id");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.PcName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pc_name");
            entity.Property(e => e.PurchasedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("purchased_at");

            entity.HasOne(d => d.Hard).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.HardId)
                .HasConstraintName("FK__pcs__hard_id__571DF1D5");

            entity.HasOne(d => d.Institute).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pcs__institute_i__282DF8C2");

            entity.HasOne(d => d.Lab).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK__pcs__lab_id__5812160E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CCE85D2DAD");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");

            entity.HasOne(d => d.Institute).WithMany(p => p.Roles)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__roles__institute__151B244E");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__C46A8A6F1170E491");

            entity.ToTable("schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ClassEndTime)
                .HasDefaultValueSql("(CONVERT([time],getdate()))")
                .HasColumnName("class_end_time");
            entity.Property(e => e.ClassStartTime)
                .HasDefaultValueSql("(CONVERT([time],getdate()))")
                .HasColumnName("class_start_time");
            entity.Property(e => e.DayId).HasColumnName("day_id");
            entity.Property(e => e.LabId).HasColumnName("lab_id");

            entity.HasOne(d => d.Day).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DayId)
                .HasConstraintName("FK__schedule__day_id__6A30C649");

            entity.HasOne(d => d.Lab).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK__schedule__lab_id__693CA210");
        });

        modelBuilder.Entity<ScheduleDay>(entity =>
        {
            entity.HasKey(e => e.DayId).HasName("PK__schedule__8B516ABB7F83E3B3");

            entity.ToTable("schedule_day");

            entity.Property(e => e.DayId).HasColumnName("day_id");
            entity.Property(e => e.DayName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("day_name");
        });

        modelBuilder.Entity<Software>(entity =>
        {
            entity.HasKey(e => e.SoftId).HasName("PK__software__FDAD1D1284739E7E");

            entity.ToTable("softwares");

            entity.Property(e => e.SoftId).HasColumnName("soft_id");
            entity.Property(e => e.ExpireDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("expire_date");
            entity.Property(e => e.InstituteId)
                .HasDefaultValue(1)
                .HasColumnName("institute_id");
            entity.Property(e => e.PurchasedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("purchased_date");
            entity.Property(e => e.SoftwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("software_name");

            entity.HasOne(d => d.Institute).WithMany(p => p.Softwares)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__softwares__insti__2BFE89A6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F4CEF2A85");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616472A7A0C3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__users__role_id__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
