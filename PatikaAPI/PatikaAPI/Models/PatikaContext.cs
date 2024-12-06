using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PatikaAPI.Models;

public partial class PatikaContext : DbContext
{
    public PatikaContext()
    {
    }

    public PatikaContext(DbContextOptions<PatikaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Betegseg> Betegsegs { get; set; }

    public virtual DbSet<Gyogyszer> Gyogyszers { get; set; }

    public virtual DbSet<Kezel> Kezels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("SERVER=localhost;PORT=3306;DATABASE=patika;USER=root;PASSWORD=;SSL MODE=none;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Betegseg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("betegseg");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Leiras).HasColumnType("text");
            entity.Property(e => e.Megnevezes).HasMaxLength(255);
        });

        modelBuilder.Entity<Gyogyszer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gyogyszer");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Hatoanyag).HasMaxLength(64);
            entity.Property(e => e.Kepnev)
                .HasMaxLength(32)
                .HasColumnName("kepnev");
            entity.Property(e => e.Nev).HasMaxLength(64);
        });

        modelBuilder.Entity<Kezel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kezel");

            entity.HasIndex(e => e.BetegsegId, "BetegsegId");

            entity.HasIndex(e => e.GyogyszerId, "GyogyszerId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.BetegsegId).HasColumnType("int(11)");
            entity.Property(e => e.GyogyszerId).HasColumnType("int(11)");

            entity.HasOne(d => d.Betegseg).WithMany(p => p.Kezels)
                .HasForeignKey(d => d.BetegsegId)
                .HasConstraintName("kezel_ibfk_2");

            entity.HasOne(d => d.Gyogyszer).WithMany(p => p.Kezels)
                .HasForeignKey(d => d.GyogyszerId)
                .HasConstraintName("kezel_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
