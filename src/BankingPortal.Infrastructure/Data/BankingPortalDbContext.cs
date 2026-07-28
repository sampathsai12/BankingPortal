using BankingPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Data
{
    public class BankingPortalDbContext : DbContext
    {
        public BankingPortalDbContext(DbContextOptions<BankingPortalDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerKyc> CustomerKycs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransactionLedger> TransactionLedgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys Explicit Configuration
            modelBuilder.Entity<CustomerKyc>().HasKey(k => k.KycId);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<UserRole>().HasKey(ur => ur.UserRoleId);
            modelBuilder.Entity<Account>().HasKey(a => a.AccountId);
            modelBuilder.Entity<TransactionLedger>().HasKey(t => t.TransactionId);
            // Indexes
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique(false);
            modelBuilder.Entity<Customer>().HasIndex(c => c.Phone).IsUnique(false);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();

            // Circular Relationship Setup: Customers.ApprovedBy -> Users.UserId
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.ApprovedByUser)
                .WithMany()
                .HasForeignKey(c => c.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Default Seed Data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Customer" }
            );
        }
    }
}