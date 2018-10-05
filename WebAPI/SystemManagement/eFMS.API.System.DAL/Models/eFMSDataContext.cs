﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eFMS.API.System.Service.Models
{
    public partial class eFMSDataContext : DbContext
    {
        public eFMSDataContext()
        {
        }

        public eFMSDataContext(DbContextOptions<eFMSDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CatBranch> CatBranch { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserGroup> SysUserGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.168.7.88;Database=eFMSTest;User ID=sa;Password=P@ssw0rd;",
                    options =>
                    {
                        options.UseRowNumberForPaging();
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatBranch>(entity =>
            {
                entity.ToTable("catBranch");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressEn)
                    .HasColumnName("Address_EN")
                    .HasMaxLength(150);

                entity.Property(e => e.AddressVn)
                    .HasColumnName("Address_VN")
                    .HasMaxLength(150);

                entity.Property(e => e.BankAccountUsd)
                    .HasColumnName("BankAccount_USD")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccountVnd)
                    .HasColumnName("BankAccount_VND")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BankAddress).HasMaxLength(255);

                entity.Property(e => e.BankName).HasMaxLength(150);

                entity.Property(e => e.BranchNameEn)
                    .HasColumnName("BranchName_EN")
                    .HasMaxLength(150);

                entity.Property(e => e.BranchNameVn)
                    .HasColumnName("BranchName_VN")
                    .HasMaxLength(150);

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.DatetimeCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InactiveOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Taxcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("sysUser");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DatetimeCreated).HasColumnType("smalldatetime");

                entity.Property(e => e.DatetimeModified).HasColumnType("smalldatetime");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InactiveOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UserCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");

                entity.Property(e => e.UserModified)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkPlaceId).HasColumnName("WorkPlaceID");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.SysUser)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sysUser_sysUserGroup");

                entity.HasOne(d => d.WorkPlace)
                    .WithMany(p => p.SysUser)
                    .HasForeignKey(d => d.WorkPlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sysUser_catBranch");
            });

            modelBuilder.Entity<SysUserGroup>(entity =>
            {
                entity.ToTable("sysUserGroup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DatetimeCreated).HasColumnType("smalldatetime");

                entity.Property(e => e.DatetimeModified).HasColumnType("smalldatetime");

                entity.Property(e => e.Decription).HasMaxLength(500);

                entity.Property(e => e.InactiveOn).HasColumnType("smalldatetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserModified)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
