-- Tạo cơ sở dữ liệu QLSanpham
CREATE DATABASE QLSanpham;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE QLSanpham;
GO

-- Tạo bảng LoaiSP
CREATE TABLE LoaiSP (
    MaLoai CHAR(2) PRIMARY KEY,
    TenLoai NVARCHAR(30)
);

-- Tạo bảng Sanpham
CREATE TABLE Sanpham (
    MaSP CHAR(6) PRIMARY KEY,
    TenSP NVARCHAR(30),
    Ngaynhap DATETIME,
    MaLoai CHAR(2),
    FOREIGN KEY (MaLoai) REFERENCES LoaiSP(MaLoai)
);
GO
-- Nhập dữ liệu vào bảng LoaiSP
INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('L1', N'Điện thoại');
INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('L2', N'Laptop');

-- Nhập dữ liệu vào bảng Sanpham
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) 
VALUES ('SP0001', N'iPhone 14', '2024-10-18', 'L1');
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) 
VALUES ('SP0002', N'MacBook Air', '2024-10-17', 'L2');
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) 
VALUES ('SP0003', N'Samsung Galaxy S22', '2024-10-16', 'L1');
GO
