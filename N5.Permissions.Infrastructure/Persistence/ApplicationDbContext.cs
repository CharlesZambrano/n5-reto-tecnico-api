﻿// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Persistence/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.PermissionType)
                .WithMany(pt => pt.Permissions)
                .HasForeignKey(p => p.PermissionTypeId);

            modelBuilder.Entity<PermissionType>()
                .HasIndex(pt => pt.Code)
                .IsUnique();
        }
    }
}
