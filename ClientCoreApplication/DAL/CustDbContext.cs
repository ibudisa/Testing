using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class CustDbContext : DbContext
    {
        public DbSet<CleaningPlan> CleaningPlans { get; set; }

        public CustDbContext(DbContextOptions<CustDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<CleaningPlan>().ToTable("CleaningPlan");
        //    builder.Entity<CleaningPlan>().HasKey(p => p.Id);
        //    builder.Entity<CleaningPlan>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        //    builder.Entity<CleaningPlan>().Property(p => p.Title).IsRequired().HasMaxLength(256);
        //    builder.Entity<CleaningPlan>().Property(p => p.CustomerId).IsRequired();
        //    builder.Entity<CleaningPlan>().Property(p => p.CreationDate).IsRequired();
        //    builder.Entity<CleaningPlan>().Property(p => p.Description).HasMaxLength(512);
        //}
    }
}
