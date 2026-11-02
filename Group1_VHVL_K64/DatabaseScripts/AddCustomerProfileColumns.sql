-- Thông tin dùng khi khách đặt / hoàn thành đơn hàng (bảng Customers).
IF COL_LENGTH('Customers', 'Phone') IS NULL
BEGIN
    ALTER TABLE Customers ADD Phone NVARCHAR(20) NULL;
END

IF COL_LENGTH('Customers', 'ShippingAddress') IS NULL
BEGIN
    ALTER TABLE Customers ADD ShippingAddress NVARCHAR(500) NULL;
END
