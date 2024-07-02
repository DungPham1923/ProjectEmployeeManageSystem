using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ManageEmployeeSystem.Models;

public partial class ProjectPrn212Context : DbContext
{
    public ProjectPrn212Context()
    {
    }

    public ProjectPrn212Context(DbContextOptions<ProjectPrn212Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Authentication> Authentications { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeJob> EmployeeJobs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =MSI; database = Project_PRN212;uid=sa;pwd=123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authentication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authenti__3214EC27E4CD7BED");

            entity.ToTable("Authentication");

            entity.HasIndex(e => e.Username, "UQ_Authentication_Username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.PassWord)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Authentications)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Authentic__Emplo__38996AB5");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC2764557417");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(55);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(55);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC273B3C3767");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(55);
            entity.Property(e => e.Gender).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(55);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(2)
                .HasColumnName("RoleID");
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__Depar__31EC6D26");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employees__Manag__32E0915F");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__Posit__30F848ED");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__RoleI__300424B4");
        });

        modelBuilder.Entity<EmployeeJob>(entity =>
        {
            entity.HasKey(e => e.EmployeeJobId).HasName("PK__Employee__F7369751213BAC5C");

            entity.HasIndex(e => new { e.EmployeeId, e.JobId }, "UQ_EmployeeJobs").IsUnique();

            entity.Property(e => e.EmployeeJobId).HasColumnName("EmployeeJobID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.JobId).HasColumnName("JobID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeJobs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeJ__Emplo__45F365D3");

            entity.HasOne(d => d.Job).WithMany(p => p.EmployeeJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeJ__JobID__46E78A0C");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs__3214EC27412B9D9F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK__Jobs__AssignedBy__403A8C7D");

            entity.HasOne(d => d.Department).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Jobs__Department__412EB0B6");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Jobs__JobStatusI__3F466844");
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobStatu__3214EC27961F51E5");

            entity.ToTable("JobStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC2709BB84AE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(55);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC271E16EE48");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeleteById).HasColumnName("DeleteByID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.RoleName).HasMaxLength(55);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
