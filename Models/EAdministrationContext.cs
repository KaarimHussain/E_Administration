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

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<Hardware> Hardwares { get; set; }

    public virtual DbSet<HodCourseAssignTeacher> HodCourseAssignTeachers { get; set; }

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
        => optionsBuilder.UseSqlServer("Server=.;Database=E_ADMINISTRATION;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__addition__3213E83FF1B56CF5");

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
            entity.HasKey(e => e.ComplaintsId).HasName("PK__complain__5C8661EE3A91EB0A");

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
            entity.HasKey(e => e.CourseId).HasName("PK__courses__8F1EF7AE1CF0BFB9");

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
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__departme__C2232422B15C80DC");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__Files__6F0F98BFBF4D1C9C");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UploadedBy).HasMaxLength(100);
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.HasKey(e => e.FloorId).HasName("PK__floors__76040CCC45DFC947");

            entity.ToTable("floors");

            entity.Property(e => e.FloorId).HasColumnName("floor_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FloorName).HasColumnName("floor_name");
            entity.Property(e => e.InstituteId).HasColumnName("institute_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Floors)
                .HasForeignKey(d => d.InstituteId)
                .HasConstraintName("FK__floors__institut__4AB81AF0");
        });

        modelBuilder.Entity<Hardware>(entity =>
        {
            entity.HasKey(e => e.HardId).HasName("PK__hardware__2646D038984D7908");

            entity.ToTable("hardwares");

            entity.Property(e => e.HardId).HasColumnName("hard_id");
            entity.Property(e => e.HardwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("hardware_name");
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
        });

        modelBuilder.Entity<HodCourseAssignTeacher>(entity =>
        {
            entity.HasKey(e => e.AssignId).HasName("PK__hod_cour__32E5EDE665AE54D1");

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
                .HasConstraintName("FK__hod_cours__assig__6E01572D");

            entity.HasOne(d => d.AssignToNavigation).WithMany(p => p.HodCourseAssignTeacherAssignToNavigations)
                .HasForeignKey(d => d.AssignTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__hod_cours__assig__6EF57B66");

            entity.HasOne(d => d.Course).WithMany(p => p.HodCourseAssignTeachers)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__hod_cours__cours__6FE99F9F");
        });

        modelBuilder.Entity<Institute>(entity =>
        {
            entity.HasKey(e => e.InstituteId).HasName("PK__institut__2A0A74D5830A75E2");

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
            entity.HasKey(e => e.LabId).HasName("PK__labs__66DE64DBAEC6E9E7");

            entity.ToTable("labs");

            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.FloorId).HasColumnName("floor_id");

            entity.HasOne(d => d.Floor).WithMany(p => p.Labs)
                .HasForeignKey(d => d.FloorId)
                .HasConstraintName("FK__labs__floor_id__52593CB8");
        });

        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.PcId).HasName("PK__pcs__1D3A69C0CA4FBCF3");

            entity.ToTable("pcs");

            entity.Property(e => e.PcId).HasColumnName("pc_id");
            entity.Property(e => e.HardId).HasColumnName("hard_id");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.PcName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pc_name");
            entity.Property(e => e.PurchasedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("purchased_at");
            entity.Property(e => e.SoftId).HasColumnName("soft_id");

            entity.HasOne(d => d.Hard).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.HardId)
                .HasConstraintName("FK__pcs__hard_id__571DF1D5");

            entity.HasOne(d => d.Lab).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK__pcs__lab_id__5812160E");

            entity.HasOne(d => d.Soft).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.SoftId)
                .HasConstraintName("FK__pcs__soft_id__5629CD9C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC4466056C");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B1CAD02DB9").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__C46A8A6FEBE90332");

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
            entity.HasKey(e => e.DayId).HasName("PK__schedule__8B516ABBB65F22BB");

            entity.ToTable("schedule_day");

            entity.Property(e => e.DayId).HasColumnName("day_id");
            entity.Property(e => e.DayName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("day_name");
        });

        modelBuilder.Entity<Software>(entity =>
        {
            entity.HasKey(e => e.SoftId).HasName("PK__software__FDAD1D1291ED44EC");

            entity.ToTable("softwares");

            entity.Property(e => e.SoftId).HasColumnName("soft_id");
            entity.Property(e => e.SoftwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("software_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F8EBD3DE8");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61641AFDC380").IsUnique();

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
