using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QRDER.Models.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminGirisi> AdminGirisis { get; set; }

    public virtual DbSet<AnaYemek> AnaYemeks { get; set; }

    public virtual DbSet<AraYemek> AraYemeks { get; set; }

    public virtual DbSet<Icecekler> Iceceklers { get; set; }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<QrVerisi> QrVerisis { get; set; }

    public virtual DbSet<Tatlilar> Tatlilars { get; set; }

    public virtual DbSet<Siparisler> Siparislers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Turkish_CI_AS");

        modelBuilder.Entity<AdminGirisi>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Admin_Girisi");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(20)
                .HasColumnName("Kullanici_Adi");
            entity.Property(e => e.Sifre)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<AnaYemek>(entity =>
        {
            entity.HasKey(e => e.AnaYemekId);
            entity.ToTable("Ana_Yemek");

            entity.Property(e => e.AnaYemekId)
                .HasColumnName("Ana_Yemek_ID");
            entity.Property(e => e.AnaYemek1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ana_Yemek");
            entity.Property(e => e.AnaYemekFiyat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ana_Yemek_Fiyat");
            entity.Property(e => e.AnaYemekGorsel)
                .HasColumnName("Ana_Yemek_Gorsel");
        });

        modelBuilder.Entity<AraYemek>(entity =>
        {
            entity.HasKey(e => e.AraYemekId);
            entity.ToTable("Ara_Yemek");

            entity.Property(e => e.AraYemekId)
                .HasColumnName("Ara_Yemek_ID");
            entity.Property(e => e.AraYemek1)
                .HasMaxLength(50)
                .HasColumnName("Ara_Yemek");
            entity.Property(e => e.AraYemekFiyat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ara_Yemek_Fiyat");
            entity.Property(e => e.AraYemekGorsel)
                .HasColumnName("Ara_Yemek_Gorsel");
        });

        modelBuilder.Entity<Icecekler>(entity =>
        {
            entity.HasKey(e => e.İcecekId);
            entity.ToTable("Icecekler");

            entity.Property(e => e.İcecekId)
                .HasColumnName("İcecek_ID");
            entity.Property(e => e.Icecekler1)
                .HasMaxLength(50)
                .HasColumnName("Icecekler");
            entity.Property(e => e.IceceklerFiyat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Icecekler_Fiyat");
            entity.Property(e => e.IceceklerGorsel)
                .HasColumnName("Icecekler_Gorsel");
        });

        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.KategoriId);
            entity.ToTable("Kategoriler");

            entity.Property(e => e.KategoriId)
                .HasColumnName("Kategori_ID");
            entity.Property(e => e.KategoriAdi)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Kategori_Adi");
            entity.Property(e => e.KategoriGorsel)
                .HasColumnName("Kategori_Gorsel");
        });

        modelBuilder.Entity<QrVerisi>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Qr_Verisi");

            entity.Property(e => e.Id)
                .HasColumnName("ID");
            entity.Property(e => e.Numara)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Qrresim)
                .HasColumnName("QRResim");
        });

        modelBuilder.Entity<Tatlilar>(entity =>
        {
            entity.HasKey(e => e.TatlilarId);
            entity.ToTable("Tatlilar");

            entity.Property(e => e.TatlilarId)
                .HasColumnName("Tatlilar_ID");
            entity.Property(e => e.Tatlilar1)
                .HasMaxLength(50)
                .HasColumnName("Tatlilar");
            entity.Property(e => e.TatlilarFiyat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Tatlilar_Fiyat");
            entity.Property(e => e.TatlilarGorsel)
                .HasColumnName("Tatlilar_Gorsel");
        });

        modelBuilder.Entity<Siparisler>(entity =>
        {
            entity.HasKey(e => e.SiparisId);
            entity.ToTable("Siparisler");

            entity.Property(e => e.SiparisId)
                .HasColumnName("Siparis_ID");
            entity.Property(e => e.MasaNo)
                .HasMaxLength(10)
                .HasColumnName("Masa_No");
            entity.Property(e => e.SiparisDetay)
                .HasColumnName("Siparis_Detay");
            entity.Property(e => e.ToplamFiyat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Toplam_Fiyat");
            entity.Property(e => e.SiparisTarihi)
                .HasColumnName("Siparis_Tarihi");
            entity.Property(e => e.Durum)
                .HasMaxLength(20)
                .HasColumnName("Durum");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
