-- Thêm cột OrderNumber (số đơn hiển thị 1, 2, 3...) cho bảng Orders.
-- Chạy một lần trên SQL Server (database HoangHoanFurniture hoặc tên bạn đang dùng).

IF COL_LENGTH(N'dbo.Orders', N'OrderNumber') IS NULL
BEGIN
    ALTER TABLE dbo.Orders ADD OrderNumber INT NOT NULL CONSTRAINT DF_Orders_OrderNumber DEFAULT 0;
END
GO

-- Gán số thứ tự theo thứ tự thời gian tạo đơn (dựa vào ID nếu chưa có dữ liệu khác).
;WITH Numbered AS (
    SELECT ID, ROW_NUMBER() OVER (ORDER BY ID) AS N
    FROM dbo.Orders
)
UPDATE o
SET o.OrderNumber = n.N
FROM dbo.Orders o
INNER JOIN Numbered n ON o.ID = n.ID
WHERE o.OrderNumber = 0 OR o.OrderNumber IS NULL;
GO

-- Unique: mỗi đơn một số hiển thị.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Orders_OrderNumber' AND object_id = OBJECT_ID(N'dbo.Orders'))
BEGIN
    CREATE UNIQUE INDEX UX_Orders_OrderNumber ON dbo.Orders(OrderNumber);
END
GO

IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = N'DF_Orders_OrderNumber' AND parent_object_id = OBJECT_ID(N'dbo.Orders'))
BEGIN
    ALTER TABLE dbo.Orders DROP CONSTRAINT DF_Orders_OrderNumber;
END
GO
