IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DataProject1')
BEGIN
    CREATE DATABASE DataProject1;
END
GO


USE DataProject1;
GO


IF OBJECT_ID('products', 'U') IS NOT NULL
    DROP TABLE products;
GO


CREATE TABLE products (
    id INT IDENTITY(1,1) PRIMARY KEY,       -- Khóa chính
    name NVARCHAR(255) NOT NULL,             
    details NVARCHAR(MAX),                   
    rate VARCHAR(50),                        
    price DECIMAL(10,2),                     
    sale_price DECIMAL(10,2),                
    img VARCHAR(255)                         
);
GO