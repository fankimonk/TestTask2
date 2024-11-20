using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class TestTask2Context : DbContext
{
    public TestTask2Context()
    {
    }

    public TestTask2Context(DbContextOptions<TestTask2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BalanceSheetRecord> BalanceSheetRecords { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassTotal> ClassTotals { get; set; }

    public virtual DbSet<GlobalTotal> GlobalTotals { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupTotal> GroupTotals { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<SheetFile> SheetFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A68EF0564E");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Group).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Accounts_Groups");
        });

        modelBuilder.Entity<BalanceSheetRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__BalanceS__FBDF78E9350C2113");

            entity.Property(e => e.ClosingBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.BalanceSheetRecords)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_BalanceSheet_Accounts");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__Banks__AA08CB136B2D7F7E");

            entity.HasIndex(e => e.BankName, "UQ__Banks__DA9ADFAACD4D8184").IsUnique();

            entity.Property(e => e.BankName).HasMaxLength(100);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927C0941F18AE");

            entity.Property(e => e.ClassNumber)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.File).WithMany(p => p.Classes)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_Classes_SheetFiles");
        });

        modelBuilder.Entity<ClassTotal>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__ClassTot__FBDF78E91D0C32F3");

            entity.Property(e => e.ClosingBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassTotals)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_ClassesTotals_Classes");
        });

        modelBuilder.Entity<GlobalTotal>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__GlobalTo__FBDF78E99E0B3C6F");

            entity.Property(e => e.ClosingBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.File).WithMany(p => p.GlobalTotals)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_GlobalTotals_Files");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36AFB6445E4");

            entity.Property(e => e.GroupNumber)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Class).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Groups_Classes");
        });

        modelBuilder.Entity<GroupTotal>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__GroupTot__FBDF78E92AB97703");

            entity.Property(e => e.ClosingBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesActive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalancesPassive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoversDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupTotals)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_GroupsTotals_Groups");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PK__Periods__E521BB166DB56EA0");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "UQ_Period").IsUnique();
        });

        modelBuilder.Entity<SheetFile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__SheetFil__6F0F98BFBB0826F1");

            entity.HasIndex(e => e.FileName, "UQ__SheetFil__589E6EEC4D479B64").IsUnique();

            entity.Property(e => e.FileName).HasMaxLength(100);
            entity.Property(e => e.PublicationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Bank).WithMany(p => p.SheetFiles)
                .HasForeignKey(d => d.BankId)
                .HasConstraintName("FK_Files_Banks");

            entity.HasOne(d => d.Period).WithMany(p => p.SheetFiles)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("FK_Files_Periods");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
