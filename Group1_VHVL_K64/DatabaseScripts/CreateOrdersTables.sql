-- Chạy script này trên database HoangHoanFurniture nếu chưa có bảng đơn hàng.
-- Sau đó gán role Admin cho tài khoản Identity trong AspNetUserRoles.

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        CustomerID INT NOT NULL,
        OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
        Status NVARCHAR(50) NOT NULL DEFAULT N'Pending',
        TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        ShippingAddress NVARCHAR(500) NULL,
        Phone NVARCHAR(20) NULL,
        Note NVARCHAR(1000) NULL,
        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customer(ID)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetails')
BEGIN
    CREATE TABLE OrderDetails (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        OrderID INT NOT NULL,
        ProductID INT NOT NULL,
        ProductName NVARCHAR(250) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        LineTotal DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(ID) ON DELETE CASCADE,
        CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ID)
    );
END

-- Gán role Admin cho tài khoản Identity (sau khi bạn đã tạo user trong AspNetUsers):
-- INSERT INTO AspNetUserRoles (UserId, RoleId)
-- SELECT u.Id, r.Id
-- FROM AspNetUsers u
-- CROSS JOIN AspNetRoles r
-- WHERE u.Email = N'admin@example.com' AND r.Name = N'Admin';
