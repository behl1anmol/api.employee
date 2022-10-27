using System;
using System.Collections.Generic;
using api.employee.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

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
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                    {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(16),
                            MaxRetries = 5,
                            Mode = RetryMode.Exponential
                    }
                };
                var client = new SecretClient(new Uri(_configuration["KeyVault:Vault"]), new DefaultAzureCredential(), options);

                KeyVaultSecret serverUrlSecret = client.GetSecret("az-sql-serverurl");
                KeyVaultSecret dbNameSecret = client.GetSecret("az-sql-db");
                KeyVaultSecret loginIDSecret = client.GetSecret("az-sql-loginid");
                KeyVaultSecret passSecret = client.GetSecret("az-sql-secret");

                string serverUrl = serverUrlSecret.Value;
                string dbName = dbNameSecret.Value;
                string loginID = loginIDSecret.Value;
                string pass = passSecret.Value;

                var url = $"Server={serverUrl};Database={dbName};User ID={loginID};Password={pass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                optionsBuilder.UseSqlServer(url);


                //string secretValue = secret.Value;

                //optionsBuilder.UseSqlServer($"Server={_configuration["az-sql-serverurl"]};Database={_configuration["az-sql-db"]};User ID={_configuration["az-sql-loginid"]};Password={_configuration["az-sql-secret"]};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
