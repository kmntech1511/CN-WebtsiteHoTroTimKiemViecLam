using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebTimViec.Models.Entities;

namespace WebTimViec.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BangCap> BangCaps { get; set; }

    public virtual DbSet<ChungChi> ChungChis { get; set; }

    public virtual DbSet<CoBangCap> CoBangCaps { get; set; }

    public virtual DbSet<CoKyNang> CoKyNangs { get; set; }

    public virtual DbSet<DatChungChi> DatChungChis { get; set; }

    public virtual DbSet<HoSoTimViec> HoSoTimViecs { get; set; }

    public virtual DbSet<KinhNghiemLamViec> KinhNghiemLamViecs { get; set; }

    public virtual DbSet<KyNang> KyNangs { get; set; }

    public virtual DbSet<NganhNghe> NganhNghes { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhomNganh> NhomNganhs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BangCap>(entity =>
        {
            entity.HasKey(e => e.MaBangCap);

            entity.ToTable("BangCap");

            entity.HasIndex(e => e.TenBangCap, "UQ_BangCap_Ten").IsUnique();

            entity.Property(e => e.CapDo).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenBangCap).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_BangCap_TrangThai");
        });

        modelBuilder.Entity<ChungChi>(entity =>
        {
            entity.HasKey(e => e.MaChungChi);

            entity.ToTable("ChungChi");

            entity.HasIndex(e => e.TenChungChi, "UQ_ChungChi_Ten").IsUnique();

            entity.Property(e => e.LoaiChungChi).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_ChungChi_NgayTao");
            entity.Property(e => e.TenChungChi).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_ChungChi_TrangThai");
        });

        modelBuilder.Entity<CoBangCap>(entity =>
        {
            entity.HasKey(e => new { e.MaHoSo, e.MaBangCap });

            entity.ToTable("CoBangCap");

            entity.HasIndex(e => e.MaBangCap, "IX_CoBangCap_MaBangCap");

            entity.Property(e => e.ChuyenNganh).HasMaxLength(150);
            entity.Property(e => e.TruongDaoTao).HasMaxLength(200);

            entity.HasOne(d => d.MaBangCapNavigation).WithMany(p => p.CoBangCaps)
                .HasForeignKey(d => d.MaBangCap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoBangCap_BangCap");

            entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.CoBangCaps)
                .HasForeignKey(d => d.MaHoSo)
                .HasConstraintName("FK_CoBangCap_HoSo");
        });

        modelBuilder.Entity<CoKyNang>(entity =>
        {
            entity.HasKey(e => new { e.MaHoSo, e.MaKyNang });

            entity.ToTable("CoKyNang");

            entity.HasIndex(e => e.MaKyNang, "IX_CoKyNang_MaKyNang");

            entity.Property(e => e.MucDo).HasMaxLength(50);
            entity.Property(e => e.SoNamSuDung).HasColumnType("decimal(4, 1)");

            entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.CoKyNangs)
                .HasForeignKey(d => d.MaHoSo)
                .HasConstraintName("FK_CoKyNang_HoSo");

            entity.HasOne(d => d.MaKyNangNavigation).WithMany(p => p.CoKyNangs)
                .HasForeignKey(d => d.MaKyNang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoKyNang_KyNang");
        });

        modelBuilder.Entity<DatChungChi>(entity =>
        {
            entity.HasKey(e => new { e.MaHoSo, e.MaChungChi });

            entity.ToTable("DatChungChi");

            entity.HasIndex(e => e.MaChungChi, "IX_DatChungChi_MaChungChi");

            entity.Property(e => e.NoiCap).HasMaxLength(200);
            entity.Property(e => e.SoHieu).HasMaxLength(100);

            entity.HasOne(d => d.MaChungChiNavigation).WithMany(p => p.DatChungChis)
                .HasForeignKey(d => d.MaChungChi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DatChungChi_ChungChi");

            entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.DatChungChis)
                .HasForeignKey(d => d.MaHoSo)
                .HasConstraintName("FK_DatChungChi_HoSo");
        });

        modelBuilder.Entity<HoSoTimViec>(entity =>
        {
            entity.HasKey(e => e.MaHoSo);

            entity.ToTable("HoSoTimViec");

            entity.HasIndex(e => e.MaNguoiDung, "IX_HoSo_MaNguoiDung");

            entity.HasIndex(e => e.NgayTao, "IX_HoSo_NgayTao");

            entity.Property(e => e.DiaDiemMongMuon).HasMaxLength(200);
            entity.Property(e => e.HinhThucLamViec).HasMaxLength(50);
            entity.Property(e => e.MucLuongMongMuon).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MucTieuNgheNghiep).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_HoSo_NgayTao");
            entity.Property(e => e.TieuDe).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_HoSo_TrangThai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.HoSoTimViecs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoSo_NguoiDung");

            entity.HasMany(d => d.MaNganhs).WithMany(p => p.MaHoSos)
                .UsingEntity<Dictionary<string, object>>(
                    "NhamToi",
                    r => r.HasOne<NganhNghe>().WithMany()
                        .HasForeignKey("MaNganh")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NhamToi_NganhNghe"),
                    l => l.HasOne<HoSoTimViec>().WithMany()
                        .HasForeignKey("MaHoSo")
                        .HasConstraintName("FK_NhamToi_HoSo"),
                    j =>
                    {
                        j.HasKey("MaHoSo", "MaNganh");
                        j.ToTable("NhamToi");
                        j.HasIndex(new[] { "MaNganh" }, "IX_NhamToi_MaNganh");
                    });
        });

        modelBuilder.Entity<KinhNghiemLamViec>(entity =>
        {
            entity.HasKey(e => e.MaKinhNghiem);

            entity.ToTable("KinhNghiemLamViec");

            entity.HasIndex(e => e.MaHoSo, "IX_KinhNghiem_MaHoSo");

            entity.Property(e => e.ChucDanh).HasMaxLength(150);
            entity.Property(e => e.DiaDiemLamViec).HasMaxLength(200);
            entity.Property(e => e.TenCongTy).HasMaxLength(200);

            entity.HasOne(d => d.MaHoSoNavigation).WithMany(p => p.KinhNghiemLamViecs)
                .HasForeignKey(d => d.MaHoSo)
                .HasConstraintName("FK_KinhNghiem_HoSo");
        });

        modelBuilder.Entity<KyNang>(entity =>
        {
            entity.HasKey(e => e.MaKyNang);

            entity.ToTable("KyNang");

            entity.HasIndex(e => e.TenKyNang, "UQ_KyNang_Ten").IsUnique();

            entity.Property(e => e.LoaiKyNang).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_KyNang_NgayTao");
            entity.Property(e => e.TenKyNang).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_KyNang_TrangThai");
        });

        modelBuilder.Entity<NganhNghe>(entity =>
        {
            entity.HasKey(e => e.MaNganh);

            entity.ToTable("NganhNghe");

            entity.HasIndex(e => e.MaNhomNganh, "IX_NganhNghe_MaNhomNganh");

            entity.HasIndex(e => new { e.MaNhomNganh, e.TenNganh }, "UQ_NganhNghe_Nhom_Ten").IsUnique();

            entity.Property(e => e.HinhAnhLarge).HasMaxLength(255);
            entity.Property(e => e.HinhAnhMedium).HasMaxLength(255);
            entity.Property(e => e.HinhAnhSmall).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_NganhNghe_NgayTao");
            entity.Property(e => e.TenNganh).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_NganhNghe_TrangThai");

            entity.HasOne(d => d.MaNhomNganhNavigation).WithMany(p => p.NganhNghes)
                .HasForeignKey(d => d.MaNhomNganh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NganhNghe_NhomNganh");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung);

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.MaVaiTro, "IX_NguoiDung_MaVaiTro");

            entity.HasIndex(e => e.NgayDangKy, "IX_NguoiDung_NgayDangKy");

            entity.HasIndex(e => e.Email, "UQ_NguoiDung_Email").IsUnique();

            entity.HasIndex(e => e.TenDangNhap, "UQ_NguoiDung_TenDangNhap").IsUnique();

            entity.Property(e => e.AnhDaiDienLarge).HasMaxLength(255);
            entity.Property(e => e.AnhDaiDienMedium).HasMaxLength(255);
            entity.Property(e => e.AnhDaiDienSmall).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.NgayDangKy).HasDefaultValueSql("(sysdatetime())", "DF_NguoiDung_NgayDangKy");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_NguoiDung_TrangThai");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NguoiDung_VaiTro");
        });

        modelBuilder.Entity<NhomNganh>(entity =>
        {
            entity.HasKey(e => e.MaNhomNganh);

            entity.ToTable("NhomNganh");

            entity.HasIndex(e => e.TenNhomNganh, "UQ_NhomNganh_Ten").IsUnique();

            entity.Property(e => e.HinhAnhLarge).HasMaxLength(255);
            entity.Property(e => e.HinhAnhMedium).HasMaxLength(255);
            entity.Property(e => e.HinhAnhSmall).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_NhomNganh_NgayTao");
            entity.Property(e => e.TenNhomNganh).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true, "DF_NhomNganh_TrangThai");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro);

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ_VaiTro_TenVaiTro").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())", "DF_VaiTro_NgayTao");
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
