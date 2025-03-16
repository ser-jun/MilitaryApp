using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace MilitaryApp.Data;

public partial class MilitaryDbContext : DbContext
{
    public MilitaryDbContext()
    {
    }

    public MilitaryDbContext(DbContextOptions<MilitaryDbContext> options)
        : base(options)
    {
    }
 
    public virtual DbSet<Army> Armies { get; set; }

    public virtual DbSet<Combatequipment> Combatequipments { get; set; }

    public virtual DbSet<Corps> Corps { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Enlistedpersonnel> Enlistedpersonnel { get; set; }

    public virtual DbSet<Infrastructure> Infrastructures { get; set; }

    public virtual DbSet<Militarypersonnel> Militarypersonnel { get; set; }

    public virtual DbSet<Militaryspecialty> Militaryspecialties { get; set; }

    public virtual DbSet<Militaryunit> Militaryunits { get; set; }

    public virtual DbSet<Officer> Officers { get; set; }

    public virtual DbSet<Subunit> Subunits { get; set; }

    public virtual DbSet<Unitcombatequipment> Unitcombatequipments { get; set; }

    public virtual DbSet<Unitweapon> Unitweapons { get; set; }

    public virtual DbSet<Weapon> Weapons { get; set; }

    public virtual DbSet<PersonnelSpecialties> PersonnelSpecialties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=militarydb;user=SeregaAdm;password=60Seri090420", ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Army>(entity =>
        {
            entity.HasKey(e => e.ArmyId).HasName("PRIMARY");

            entity.ToTable("armies");

            entity.Property(e => e.ArmyId).HasColumnName("army_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Combatequipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PRIMARY");

            entity.ToTable("combatequipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Corps>(entity =>
        {
            entity.HasKey(e => e.CorpsId).HasName("PRIMARY");

            entity.ToTable("corps");

            entity.HasIndex(e => e.DivisionId, "division_id");

            entity.Property(e => e.CorpsId).HasColumnName("corps_id");
            entity.Property(e => e.DivisionId).HasColumnName("division_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Division).WithMany(p => p.Corps)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("corps_ibfk_1");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.DivisionId).HasName("PRIMARY");

            entity.ToTable("divisions");

            entity.HasIndex(e => e.ArmyId, "army_id");

            entity.Property(e => e.DivisionId).HasColumnName("division_id");
            entity.Property(e => e.ArmyId).HasColumnName("army_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Army).WithMany(p => p.Divisions)
                .HasForeignKey(d => d.ArmyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("divisions_ibfk_1");
        });

        modelBuilder.Entity<Enlistedpersonnel>(entity =>
        {
            entity.HasKey(e => e.EnlistedId).HasName("PRIMARY");

            entity.ToTable("enlistedpersonnel");

            entity.HasIndex(e => e.PersonnelId, "personnel_id");

            entity.Property(e => e.EnlistedId).HasColumnName("enlisted_id");
            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasColumnName("position");

            entity.HasOne(d => d.Personnel).WithMany(p => p.Enlistedpersonnel)
                .HasForeignKey(d => d.PersonnelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("enlistedpersonnel_ibfk_1");
        });

        modelBuilder.Entity<Infrastructure>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PRIMARY");

            entity.ToTable("infrastructure");

            entity.HasIndex(e => e.UnitId, "unit_id");

            entity.Property(e => e.BuildingId).HasColumnName("building_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.YearBuilt).HasColumnName("year_built");

            entity.HasOne(d => d.Unit).WithMany(p => p.Infrastructures)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("infrastructure_ibfk_1");
        });

        modelBuilder.Entity<Militarypersonnel>(entity =>
        {
            entity.HasKey(e => e.PersonnelId).HasName("PRIMARY");

            entity.ToTable("militarypersonnel");

            entity.HasIndex(e => e.UnitId, "unit_id");

            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Rank)
                .HasMaxLength(255)
                .HasColumnName("rank");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Unit).WithMany(p => p.Militarypersonnel)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("militarypersonnel_ibfk_1");
        });

        modelBuilder.Entity<Militaryspecialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PRIMARY");

            entity.ToTable("militaryspecialties");

            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Militaryunit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PRIMARY");

            entity.ToTable("militaryunits");

            entity.HasIndex(e => e.CorpsId, "corps_id");

            entity.HasIndex(e => e.CommanderId, "fk_commander");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.CommanderId).HasColumnName("commander_id");
            entity.Property(e => e.CorpsId).HasColumnName("corps_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Commander).WithMany(p => p.Militaryunits)
                .HasForeignKey(d => d.CommanderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_commander");

            entity.HasOne(d => d.Corps).WithMany(p => p.Militaryunits)
                .HasForeignKey(d => d.CorpsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("militaryunits_ibfk_1");
        });

        modelBuilder.Entity<Officer>(entity =>
        {
            entity.HasKey(e => e.OfficerId).HasName("PRIMARY");

            entity.ToTable("officers");

            entity.HasIndex(e => e.PersonnelId, "personnel_id");

            entity.Property(e => e.OfficerId).HasColumnName("officer_id");
            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasColumnName("position");

            entity.HasOne(d => d.Personnel).WithMany(p => p.Officers)
                .HasForeignKey(d => d.PersonnelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("officers_ibfk_1");
        });

        modelBuilder.Entity<Subunit>(entity =>
        {
            entity.HasKey(e => e.SubunitId).HasName("PRIMARY");

            entity.ToTable("subunits");

            entity.HasIndex(e => e.UnitId, "unit_id");

            entity.Property(e => e.SubunitId).HasColumnName("subunit_id");
            entity.Property(e => e.SubunitName)
                .HasMaxLength(255)
                .HasColumnName("subunit_name");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Unit).WithMany(p => p.Subunits)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("subunits_ibfk_1");
        });

        modelBuilder.Entity<Unitcombatequipment>(entity =>
        {
            entity.HasKey(e => new { e.UnitId, e.EquipmentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("unitcombatequipment");

            entity.HasIndex(e => e.EquipmentId, "equipment_id");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Unitcombatequipments)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("unitcombatequipment_ibfk_2");

            entity.HasOne(d => d.Unit).WithMany(p => p.Unitcombatequipments)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("unitcombatequipment_ibfk_1");
        });

        modelBuilder.Entity<Unitweapon>(entity =>
        {
            entity.HasKey(e => new { e.UnitId, e.WeaponId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("unitweapons");

            entity.HasIndex(e => e.WeaponId, "weapon_id");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.WeaponId).HasColumnName("weapon_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Unit).WithMany(p => p.Unitweapons)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("unitweapons_ibfk_1");

            entity.HasOne(d => d.Weapon).WithMany(p => p.Unitweapons)
                .HasForeignKey(d => d.WeaponId)
                .HasConstraintName("unitweapons_ibfk_2");
        });

        modelBuilder.Entity<Weapon>(entity =>
        {
            entity.HasKey(e => e.WeaponId).HasName("PRIMARY");

            entity.ToTable("weapons");

            entity.Property(e => e.WeaponId).HasColumnName("weapon_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });
        modelBuilder.Entity<PersonnelSpecialties>(entity =>
        {
            entity.HasKey(ps => new { ps.PersonnelId, ps.SpecialtyId }) 
                  .HasName("PRIMARY")
                  .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("personnelspecialties");

            entity.HasIndex(e => e.SpecialtyId, "specialty_id");

            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");

            entity.HasOne(d => d.Personnel)
                  .WithMany(p => p.Specialties)
                  .HasForeignKey(d => d.PersonnelId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("personnelspecialties_ibfk_1");

            entity.HasOne(d => d.Specialty)
                  .WithMany(p => p.Personnel)
                  .HasForeignKey(d => d.SpecialtyId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("personnelspecialties_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
