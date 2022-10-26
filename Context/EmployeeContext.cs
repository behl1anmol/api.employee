using System;
using System.Collections.Generic;
using api.employee.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api.employee.Context
{
    public partial class EmployeeContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EmployeeContext(DbContextOptions<EmployeeContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server={_configuration["az-sql-serverurl"]};Database={_configuration["az-sql-db"]};User ID={_configuration["az-sql-loginid"]};Password={_configuration["az-sql-secret"]};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Empid)
                    .HasName("PK__Employee__AF4CE865EE17D7BA");

                entity.ToTable("Employee");

                entity.Property(e => e.Empid).HasColumnName("empid");

                entity.Property(e => e.Empdob)
                    .HasColumnType("datetime")
                    .HasColumnName("empdob");

                entity.Property(e => e.Emplocation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("emplocation");

                entity.Property(e => e.Empname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("empname");

                entity.Property(e => e.Empphonenumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("empphonenumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
