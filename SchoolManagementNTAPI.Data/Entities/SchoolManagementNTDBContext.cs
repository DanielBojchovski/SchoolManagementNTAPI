﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementNTAPI.Data.Entities;

public partial class SchoolManagementNTDBContext : DbContext
{
    public SchoolManagementNTDBContext(DbContextOptions<SchoolManagementNTDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Principal> Principal { get; set; }

    public virtual DbSet<Professor> Professor { get; set; }

    public virtual DbSet<School> School { get; set; }

    public virtual DbSet<Student> Student { get; set; }

    public virtual DbSet<StudentSubject> StudentSubject { get; set; }

    public virtual DbSet<Subject> Subject { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Principal>(entity =>
        {
            entity.HasIndex(e => e.SchoolId, "IX_Principal").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.School).WithOne(p => p.Principal)
                .HasForeignKey<Principal>(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Principal_School");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.Professor)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Professor_School");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<StudentSubject>(entity =>
        {
            entity.HasIndex(e => new { e.StudentId, e.IsMajor }, "IX_StudentMajor").HasFilter("([IsMajor]=(1))");

            entity.HasIndex(e => new { e.StudentId, e.SubjectId }, "IX_StudentSubject").IsUnique();

            entity.HasOne(d => d.Student).WithMany(p => p.StudentSubject)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSubject_Student");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentSubject)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSubject_Subject");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}