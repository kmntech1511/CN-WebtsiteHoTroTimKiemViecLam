/* =====================================================================
   Đồ án chuyên ngành : Website Hỗ Trợ Tìm Kiếm Việc Làm Cho Người Lao Động
   CSDL    : WebTimViecDB (SQL Server)
   Cach chay: mo bang SSMS / Azure Data Studio / VS Code (mssql) roi Execute.
              Script danh cho CSDL moi. Muon lam lai tu dau thi DROP DATABASE.
   ===================================================================== */

IF DB_ID(N'WebTimViecDB') IS NULL
    CREATE DATABASE WebTimViecDB;
GO

USE WebTimViecDB;
GO

SET NOCOUNT ON;
GO

/* =====================================================================
   1. BANG DANH MUC / THUC THE
   Quy uoc: TrangThai BIT -> 1 = hoat dong / hien thi, 0 = an / khoa
   ===================================================================== */

-- VaiTro: quyen su dung (Quan tri vien, Nguoi tim viec)
CREATE TABLE VaiTro (
    MaVaiTro   INT IDENTITY(1,1) NOT NULL,
    TenVaiTro  NVARCHAR(50)  NOT NULL,
    MoTa       NVARCHAR(255) NULL,
    NgayTao    DATETIME2     NOT NULL CONSTRAINT DF_VaiTro_NgayTao DEFAULT SYSDATETIME(),
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_TenVaiTro UNIQUE (TenVaiTro)
);
GO

-- NguoiDung: tai khoan (quan tri vien va nguoi tim viec), quan he DuocGan voi VaiTro (1,1)
CREATE TABLE NguoiDung (
    MaNguoiDung       INT IDENTITY(1,1) NOT NULL,
    MaVaiTro          INT           NOT NULL,
    TenDangNhap       NVARCHAR(50)  NOT NULL,
    MatKhau           NVARCHAR(255) NOT NULL,   -- luu MAT KHAU DA BAM (hash), khong luu mat khau goc
    HoTen             NVARCHAR(100) NOT NULL,
    Email             NVARCHAR(150) NOT NULL,
    SDT               VARCHAR(20)   NULL,
    NgaySinh          DATE          NULL,
    GioiTinh          NVARCHAR(10)  NULL,
    AnhDaiDienSmall   NVARCHAR(255) NULL,
    AnhDaiDienMedium  NVARCHAR(255) NULL,
    AnhDaiDienLarge   NVARCHAR(255) NULL,
    NgayDangKy        DATETIME2     NOT NULL CONSTRAINT DF_NguoiDung_NgayDangKy DEFAULT SYSDATETIME(),
    LanDangNhapCuoi   DATETIME2     NULL,
    TrangThai         BIT           NOT NULL CONSTRAINT DF_NguoiDung_TrangThai DEFAULT 1,
    CONSTRAINT PK_NguoiDung PRIMARY KEY (MaNguoiDung),
    CONSTRAINT UQ_NguoiDung_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT UQ_NguoiDung_Email UNIQUE (Email),
    CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT CK_NguoiDung_GioiTinh CHECK (GioiTinh IS NULL OR GioiTinh IN (N'Nam', N'Nữ', N'Khác'))
);
GO

-- NhomNganh: nhom nganh co tuyen dung lao dong
CREATE TABLE NhomNganh (
    MaNhomNganh     INT IDENTITY(1,1) NOT NULL,
    TenNhomNganh    NVARCHAR(150) NOT NULL,
    MoTa            NVARCHAR(500) NULL,
    HinhAnhSmall    NVARCHAR(255) NULL,
    HinhAnhMedium   NVARCHAR(255) NULL,
    HinhAnhLarge    NVARCHAR(255) NULL,
    ThuTuHienThi    INT           NOT NULL CONSTRAINT DF_NhomNganh_ThuTu DEFAULT 0,
    TrangThai       BIT           NOT NULL CONSTRAINT DF_NhomNganh_TrangThai DEFAULT 1,
    NgayTao         DATETIME2     NOT NULL CONSTRAINT DF_NhomNganh_NgayTao DEFAULT SYSDATETIME(),
    CONSTRAINT PK_NhomNganh PRIMARY KEY (MaNhomNganh),
    CONSTRAINT UQ_NhomNganh_Ten UNIQUE (TenNhomNganh)
);
GO

-- NganhNghe: nganh nghe pho bien, quan he Gom voi NhomNganh (1,1)
CREATE TABLE NganhNghe (
    MaNganh         INT IDENTITY(1,1) NOT NULL,
    MaNhomNganh     INT           NOT NULL,
    TenNganh        NVARCHAR(150) NOT NULL,
    MoTa            NVARCHAR(500) NULL,
    HinhAnhSmall    NVARCHAR(255) NULL,
    HinhAnhMedium   NVARCHAR(255) NULL,
    HinhAnhLarge    NVARCHAR(255) NULL,
    TrangThai       BIT           NOT NULL CONSTRAINT DF_NganhNghe_TrangThai DEFAULT 1,
    NgayTao         DATETIME2     NOT NULL CONSTRAINT DF_NganhNghe_NgayTao DEFAULT SYSDATETIME(),
    CONSTRAINT PK_NganhNghe PRIMARY KEY (MaNganh),
    CONSTRAINT FK_NganhNghe_NhomNganh FOREIGN KEY (MaNhomNganh) REFERENCES NhomNganh(MaNhomNganh),
    CONSTRAINT UQ_NganhNghe_Nhom_Ten UNIQUE (MaNhomNganh, TenNganh)
);
GO

-- KyNang: ky nang ma nha tuyen dung yeu cau
CREATE TABLE KyNang (
    MaKyNang    INT IDENTITY(1,1) NOT NULL,
    TenKyNang   NVARCHAR(100) NOT NULL,
    LoaiKyNang  NVARCHAR(100) NULL,
    MoTa        NVARCHAR(500) NULL,
    TrangThai   BIT           NOT NULL CONSTRAINT DF_KyNang_TrangThai DEFAULT 1,
    NgayTao     DATETIME2     NOT NULL CONSTRAINT DF_KyNang_NgayTao DEFAULT SYSDATETIME(),
    CONSTRAINT PK_KyNang PRIMARY KEY (MaKyNang),
    CONSTRAINT UQ_KyNang_Ten UNIQUE (TenKyNang)
);
GO

-- ChungChi: danh muc chung chi
CREATE TABLE ChungChi (
    MaChungChi    INT IDENTITY(1,1) NOT NULL,
    TenChungChi   NVARCHAR(150) NOT NULL,
    LoaiChungChi  NVARCHAR(100) NULL,
    MoTa          NVARCHAR(500) NULL,
    TrangThai     BIT           NOT NULL CONSTRAINT DF_ChungChi_TrangThai DEFAULT 1,
    NgayTao       DATETIME2     NOT NULL CONSTRAINT DF_ChungChi_NgayTao DEFAULT SYSDATETIME(),
    CONSTRAINT PK_ChungChi PRIMARY KEY (MaChungChi),
    CONSTRAINT UQ_ChungChi_Ten UNIQUE (TenChungChi)
);
GO

-- BangCap: danh muc bang cap
CREATE TABLE BangCap (
    MaBangCap   INT IDENTITY(1,1) NOT NULL,
    TenBangCap  NVARCHAR(150) NOT NULL,
    MoTa        NVARCHAR(500) NULL,
    CapDo       NVARCHAR(100) NULL,
    TrangThai   BIT           NOT NULL CONSTRAINT DF_BangCap_TrangThai DEFAULT 1,
    CONSTRAINT PK_BangCap PRIMARY KEY (MaBangCap),
    CONSTRAINT UQ_BangCap_Ten UNIQUE (TenBangCap)
);
GO

-- HoSoTimViec: ho so tim viec, quan he Tao voi NguoiDung (1,1)
CREATE TABLE HoSoTimViec (
    MaHoSo             INT IDENTITY(1,1) NOT NULL,
    MaNguoiDung        INT            NOT NULL,
    TieuDe             NVARCHAR(200)  NOT NULL,
    GioiThieuBanThan   NVARCHAR(MAX)  NULL,
    MucTieuNgheNghiep  NVARCHAR(500)  NULL,
    HinhThucLamViec    NVARCHAR(50)   NULL,      -- vi du: Toan thoi gian, Ban thoi gian, Thuc tap
    MucLuongMongMuon   DECIMAL(18,0)  NULL,      -- NULL = thoa thuan (don vi: VND)
    DiaDiemMongMuon    NVARCHAR(200)  NULL,
    NgayTao            DATETIME2      NOT NULL CONSTRAINT DF_HoSo_NgayTao DEFAULT SYSDATETIME(),
    NgayCapNhat        DATETIME2      NULL,
    TrangThai          BIT            NOT NULL CONSTRAINT DF_HoSo_TrangThai DEFAULT 1,
    CONSTRAINT PK_HoSoTimViec PRIMARY KEY (MaHoSo),
    CONSTRAINT FK_HoSo_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT CK_HoSo_MucLuong CHECK (MucLuongMongMuon IS NULL OR MucLuongMongMuon >= 0)
);
GO

-- KinhNghiemLamViec: quan he CoKinhNghiem voi HoSoTimViec (1,1)
CREATE TABLE KinhNghiemLamViec (
    MaKinhNghiem    INT IDENTITY(1,1) NOT NULL,
    MaHoSo          INT            NOT NULL,
    TenCongTy       NVARCHAR(200)  NOT NULL,
    ChucDanh        NVARCHAR(150)  NOT NULL,
    DiaDiemLamViec  NVARCHAR(200)  NULL,
    TuNgay          DATE           NOT NULL,
    DenNgay         DATE           NULL,         -- NULL = dang lam
    MoTaCongViec    NVARCHAR(MAX)  NULL,
    CONSTRAINT PK_KinhNghiemLamViec PRIMARY KEY (MaKinhNghiem),
    CONSTRAINT FK_KinhNghiem_HoSo FOREIGN KEY (MaHoSo) REFERENCES HoSoTimViec(MaHoSo) ON DELETE CASCADE,
    CONSTRAINT CK_KinhNghiem_Ngay CHECK (DenNgay IS NULL OR DenNgay >= TuNgay)
);
GO

/* =====================================================================
   2. BANG LIEN KET (quan he nhieu-nhieu, co thuoc tinh rieng)
   ===================================================================== */

-- CoKyNang: HoSoTimViec - KyNang (MucDo, SoNamSuDung)
CREATE TABLE CoKyNang (
    MaHoSo        INT           NOT NULL,
    MaKyNang      INT           NOT NULL,
    MucDo         NVARCHAR(50)  NULL,            -- vi du: Co ban, Kha, Gioi
    SoNamSuDung   DECIMAL(4,1)  NULL,
    CONSTRAINT PK_CoKyNang PRIMARY KEY (MaHoSo, MaKyNang),
    CONSTRAINT FK_CoKyNang_HoSo FOREIGN KEY (MaHoSo) REFERENCES HoSoTimViec(MaHoSo) ON DELETE CASCADE,
    CONSTRAINT FK_CoKyNang_KyNang FOREIGN KEY (MaKyNang) REFERENCES KyNang(MaKyNang),
    CONSTRAINT CK_CoKyNang_SoNam CHECK (SoNamSuDung IS NULL OR SoNamSuDung >= 0)
);
GO

-- NhamToi: HoSoTimViec - NganhNghe (ho so nham toi nganh nghe nao)
CREATE TABLE NhamToi (
    MaHoSo    INT NOT NULL,
    MaNganh   INT NOT NULL,
    CONSTRAINT PK_NhamToi PRIMARY KEY (MaHoSo, MaNganh),
    CONSTRAINT FK_NhamToi_HoSo FOREIGN KEY (MaHoSo) REFERENCES HoSoTimViec(MaHoSo) ON DELETE CASCADE,
    CONSTRAINT FK_NhamToi_NganhNghe FOREIGN KEY (MaNganh) REFERENCES NganhNghe(MaNganh)
);
GO

-- DatChungChi: HoSoTimViec - ChungChi (SoHieu, NgayCap, NgayHetHan, NoiCap)
CREATE TABLE DatChungChi (
    MaHoSo       INT           NOT NULL,
    MaChungChi   INT           NOT NULL,
    SoHieu       NVARCHAR(100) NULL,
    NgayCap      DATE          NULL,
    NgayHetHan   DATE          NULL,             -- NULL = khong het han
    NoiCap       NVARCHAR(200) NULL,
    CONSTRAINT PK_DatChungChi PRIMARY KEY (MaHoSo, MaChungChi),
    CONSTRAINT FK_DatChungChi_HoSo FOREIGN KEY (MaHoSo) REFERENCES HoSoTimViec(MaHoSo) ON DELETE CASCADE,
    CONSTRAINT FK_DatChungChi_ChungChi FOREIGN KEY (MaChungChi) REFERENCES ChungChi(MaChungChi),
    CONSTRAINT CK_DatChungChi_Ngay CHECK (NgayHetHan IS NULL OR NgayCap IS NULL OR NgayHetHan >= NgayCap)
);
GO

-- CoBangCap: HoSoTimViec - BangCap (TruongDaoTao, ChuyenNganh, NamTotNghiep)
CREATE TABLE CoBangCap (
    MaHoSo         INT           NOT NULL,
    MaBangCap      INT           NOT NULL,
    TruongDaoTao   NVARCHAR(200) NULL,
    ChuyenNganh    NVARCHAR(150) NULL,
    NamTotNghiep   INT           NULL,
    CONSTRAINT PK_CoBangCap PRIMARY KEY (MaHoSo, MaBangCap),
    CONSTRAINT FK_CoBangCap_HoSo FOREIGN KEY (MaHoSo) REFERENCES HoSoTimViec(MaHoSo) ON DELETE CASCADE,
    CONSTRAINT FK_CoBangCap_BangCap FOREIGN KEY (MaBangCap) REFERENCES BangCap(MaBangCap),
    CONSTRAINT CK_CoBangCap_Nam CHECK (NamTotNghiep IS NULL OR NamTotNghiep BETWEEN 1950 AND 2100)
);
GO

/* =====================================================================
   3. CHI MUC (tang toc tim kiem / thong ke)
   ===================================================================== */
CREATE INDEX IX_NguoiDung_MaVaiTro      ON NguoiDung(MaVaiTro);
CREATE INDEX IX_NguoiDung_NgayDangKy    ON NguoiDung(NgayDangKy);
CREATE INDEX IX_NganhNghe_MaNhomNganh   ON NganhNghe(MaNhomNganh);
CREATE INDEX IX_HoSo_MaNguoiDung        ON HoSoTimViec(MaNguoiDung);
CREATE INDEX IX_HoSo_NgayTao            ON HoSoTimViec(NgayTao);
CREATE INDEX IX_KinhNghiem_MaHoSo       ON KinhNghiemLamViec(MaHoSo);
CREATE INDEX IX_CoKyNang_MaKyNang       ON CoKyNang(MaKyNang);
CREATE INDEX IX_NhamToi_MaNganh         ON NhamToi(MaNganh);
CREATE INDEX IX_DatChungChi_MaChungChi  ON DatChungChi(MaChungChi);
CREATE INDEX IX_CoBangCap_MaBangCap     ON CoBangCap(MaBangCap);
GO

/* =====================================================================
   4. THU TUC THONG KE NGUOI TIM VIEC THEO THANG / THEO NAM
   Co so thong ke: ngay dang ky tai khoan (NguoiDung.NgayDangKy)
   cua cac tai khoan co vai tro 'Nguoi tim viec'.
   ===================================================================== */

-- Danh sach nguoi tim viec trong 1 thang cua 1 nam
CREATE PROCEDURE sp_DanhSachNguoiTimViecTheoThang
    @Thang INT,
    @Nam   INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT nd.MaNguoiDung, nd.HoTen, nd.Email, nd.SDT, nd.NgayDangKy
    FROM NguoiDung nd
    JOIN VaiTro vt ON vt.MaVaiTro = nd.MaVaiTro
    WHERE vt.TenVaiTro = N'NguoiTimViec'
      AND MONTH(nd.NgayDangKy) = @Thang
      AND YEAR(nd.NgayDangKy)  = @Nam
    ORDER BY nd.NgayDangKy;
END;
GO

-- So luong nguoi tim viec theo tung thang trong 1 nam
CREATE PROCEDURE sp_ThongKeNguoiTimViecTheoThang
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.Thang, COUNT(nd.MaNguoiDung) AS SoNguoiTimViec
    FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS m(Thang)
    LEFT JOIN (
        SELECT n.MaNguoiDung, n.NgayDangKy
        FROM NguoiDung n
        JOIN VaiTro v ON v.MaVaiTro = n.MaVaiTro
        WHERE v.TenVaiTro = N'NguoiTimViec' AND YEAR(n.NgayDangKy) = @Nam
    ) nd ON MONTH(nd.NgayDangKy) = m.Thang
    GROUP BY m.Thang
    ORDER BY m.Thang;
END;
GO

-- So luong nguoi tim viec theo tung nam
CREATE PROCEDURE sp_ThongKeNguoiTimViecTheoNam
AS
BEGIN
    SET NOCOUNT ON;
    SELECT YEAR(nd.NgayDangKy) AS Nam, COUNT(*) AS SoNguoiTimViec
    FROM NguoiDung nd
    JOIN VaiTro vt ON vt.MaVaiTro = nd.MaVaiTro
    WHERE vt.TenVaiTro = N'NguoiTimViec'
    GROUP BY YEAR(nd.NgayDangKy)
    ORDER BY Nam;
END;
GO

/* =====================================================================
   5. DU LIEU MAU
   Luu y: chua tao tai khoan quan tri o day vi MatKhau phai la ma bam
   do ung dung tao ra. Hay seed tai khoan admin bang code (Program.cs).
   ===================================================================== */
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES
    (N'QuanTri',      N'Quản trị viên: quản lý danh mục, hồ sơ và thống kê'),
    (N'NguoiTimViec', N'Người tìm việc: tạo và quản lý hồ sơ của mình');
GO

INSERT INTO NhomNganh (TenNhomNganh, MoTa, ThuTuHienThi) VALUES
    (N'Công nghệ thông tin', N'Phát triển phần mềm, hạ tầng, dữ liệu, bảo mật', 1),
    (N'Kinh doanh - Bán hàng', N'Kinh doanh, bán hàng, chăm sóc khách hàng', 2),
    (N'Kế toán - Tài chính', N'Kế toán, kiểm toán, ngân hàng, tài chính', 3),
    (N'Marketing - Truyền thông', N'Marketing, quảng cáo, nội dung, truyền thông', 4),
    (N'Giáo dục - Đào tạo', N'Giảng dạy, đào tạo, nghiên cứu', 5),
    (N'Cơ khí - Điện - Điện tử', N'Cơ khí chế tạo, điện công nghiệp, điện tử', 6);
GO

INSERT INTO NganhNghe (MaNhomNganh, TenNganh, MoTa)
SELECT n.MaNhomNganh, v.TenNganh, v.MoTa
FROM (VALUES
    (N'Công nghệ thông tin', N'Lập trình viên Web',      N'Phát triển website front-end và back-end'),
    (N'Công nghệ thông tin', N'Lập trình viên Mobile',   N'Phát triển ứng dụng di động'),
    (N'Công nghệ thông tin', N'Kiểm thử phần mềm',       N'Kiểm thử thủ công và tự động'),
    (N'Công nghệ thông tin', N'Quản trị cơ sở dữ liệu',  N'Thiết kế, vận hành và tối ưu cơ sở dữ liệu'),
    (N'Kinh doanh - Bán hàng', N'Nhân viên kinh doanh',  N'Tìm kiếm và chăm sóc khách hàng'),
    (N'Kinh doanh - Bán hàng', N'Chăm sóc khách hàng',   N'Hỗ trợ và giải đáp khách hàng'),
    (N'Kế toán - Tài chính', N'Kế toán tổng hợp',        N'Hạch toán, báo cáo tài chính'),
    (N'Marketing - Truyền thông', N'Digital Marketing',  N'Quảng cáo trực tuyến, SEO, mạng xã hội'),
    (N'Giáo dục - Đào tạo', N'Giáo viên',                N'Giảng dạy các cấp'),
    (N'Cơ khí - Điện - Điện tử', N'Kỹ sư điện',          N'Thiết kế và vận hành hệ thống điện')
) AS v(TenNhom, TenNganh, MoTa)
JOIN NhomNganh n ON n.TenNhomNganh = v.TenNhom;
GO

INSERT INTO KyNang (TenKyNang, LoaiKyNang, MoTa) VALUES
    (N'C#',                 N'Lập trình',   N'Ngôn ngữ lập trình C#'),
    (N'ASP.NET Core MVC',   N'Lập trình',   N'Framework xây dựng ứng dụng web'),
    (N'SQL Server',         N'Cơ sở dữ liệu', N'Truy vấn và thiết kế CSDL SQL Server'),
    (N'JavaScript',         N'Lập trình',   N'Ngôn ngữ lập trình web phía client'),
    (N'HTML/CSS',           N'Lập trình',   N'Xây dựng giao diện web'),
    (N'Giao tiếp',          N'Kỹ năng mềm', N'Giao tiếp và trình bày'),
    (N'Làm việc nhóm',      N'Kỹ năng mềm', N'Phối hợp làm việc trong nhóm'),
    (N'Tiếng Anh giao tiếp', N'Ngoại ngữ',  N'Nghe, nói, đọc, viết tiếng Anh'),
    (N'Tin học văn phòng',  N'Tin học',     N'Word, Excel, PowerPoint');
GO

INSERT INTO ChungChi (TenChungChi, LoaiChungChi, MoTa) VALUES
    (N'IELTS',                         N'Ngoại ngữ', N'Chứng chỉ tiếng Anh quốc tế'),
    (N'TOEIC',                         N'Ngoại ngữ', N'Chứng chỉ tiếng Anh giao tiếp công việc'),
    (N'Chuẩn kỹ năng sử dụng CNTT cơ bản', N'Tin học', N'Chứng chỉ ứng dụng CNTT cơ bản'),
    (N'Microsoft Azure Fundamentals',  N'Công nghệ thông tin', N'Chứng chỉ nền tảng điện toán đám mây Azure'),
    (N'Chứng chỉ hành nghề kế toán',   N'Kế toán',  N'Chứng chỉ chuyên môn kế toán');
GO

INSERT INTO BangCap (TenBangCap, MoTa, CapDo) VALUES
    (N'Trung cấp',  N'Bằng trung cấp nghề',      N'Trung cấp'),
    (N'Cao đẳng',   N'Bằng cao đẳng',            N'Cao đẳng'),
    (N'Đại học',    N'Bằng cử nhân / kỹ sư',     N'Đại học'),
    (N'Thạc sĩ',    N'Bằng thạc sĩ',             N'Sau đại học'),
    (N'Tiến sĩ',    N'Bằng tiến sĩ',             N'Sau đại học');
GO

PRINT N'Tao CSDL WebTimViecDB thanh cong.';
GO