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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Floor> Floors { get; set; }

    public virtual DbSet<Hardware> Hardwares { get; set; }

    public virtual DbSet<Lab> Labs { get; set; }

    public virtual DbSet<Pc> Pcs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Software> Softwares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=E_ADMINISTRATION;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__addition__3213E83FDAA90A25");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__additiona__user___4222D4EF");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.ComplaintsId).HasName("PK__complain__5C8661EEB51A97C3");

            entity.ToTable("complaints");

            entity.Property(e => e.ComplaintsId).HasColumnName("complaints_id");
            entity.Property(e => e.ComplaintsResponse)
                .HasColumnType("text")
                .HasColumnName("complaints_response");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__complaint__user___6D0D32F4");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__departme__C22324220800394B");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Floor>(entity =>
        {
            entity.HasKey(e => e.FloorId).HasName("PK__floors__76040CCC689CF7BC");

            entity.ToTable("floors");

            entity.Property(e => e.FloorId).HasColumnName("floor_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FloorName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("floor_name");
        });

        modelBuilder.Entity<Hardware>(entity =>
        {
            entity.HasKey(e => e.HardId).HasName("PK__hardware__2646D038CDD871DC");

            entity.ToTable("hardwares");

            entity.Property(e => e.HardId).HasColumnName("hard_id");
            entity.Property(e => e.HardwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("hardware_name");
            entity.Property(e => e.OperatingSystem)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("operating_system");
            entity.Property(e => e.Processor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("processor");
            entity.Property(e => e.Ram).HasColumnName("ram");
            entity.Property(e => e.StorageCapacity).HasColumnName("storage_capacity");
        });

        modelBuilder.Entity<Lab>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("PK__labs__66DE64DB10E261FE");

            entity.ToTable("labs");

            entity.HasIndex(e => e.LabName, "UQ__labs__6761F66560A4A4B7").IsUnique();

            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.FloorId).HasColumnName("floor_id");
            entity.Property(e => e.LabName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lab_name");

            entity.HasOne(d => d.Floor).WithMany(p => p.Labs)
                .HasForeignKey(d => d.FloorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__labs__floor_id__5535A963");
        });

        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.PcId).HasName("PK__pcs__1D3A69C0273E265A");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pcs__hard_id__693CA210");

            entity.HasOne(d => d.Lab).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.LabId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pcs__lab_id__6A30C649");

            entity.HasOne(d => d.Soft).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.SoftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pcs__soft_id__68487DD7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC60F63F18");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B181C5427B").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Software>(entity =>
        {
            entity.HasKey(e => e.SoftId).HasName("PK__software__FDAD1D125144F152");

            entity.ToTable("softwares");

            entity.Property(e => e.SoftId).HasColumnName("soft_id");
            entity.Property(e => e.SoftwareName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("software_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F951B45F0");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164FF826E34").IsUnique();

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
