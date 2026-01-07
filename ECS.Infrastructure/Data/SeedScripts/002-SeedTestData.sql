-- Seed test data for Development environment
-- WARNING: Only run this in non-production databases

-- Seed Categories
IF NOT EXISTS (SELECT 1 FROM Categories)
BEGIN
    INSERT INTO Categories (Name, Description, IsActive, CreatedAt)
    VALUES 
        ('Electronics', 'Electronic devices and accessories', 1, GETUTCDATE()),
        ('Clothing', 'Men''s and women''s apparel', 1, GETUTCDATE()),
        ('Books', 'Physical and digital books', 1, GETUTCDATE()),
        ('Home & Garden', 'Home improvement and gardening supplies', 1, GETUTCDATE()),
        ('Sports & Outdoors', 'Sports equipment and outdoor gear', 1, GETUTCDATE())

    PRINT 'Categories seeded'
END

-- Seed Products
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    DECLARE @ElectronicsId INT = (SELECT CategoryId FROM Categories WHERE Name = 'Electronics')
    DECLARE @ClothingId INT = (SELECT CategoryId FROM Categories WHERE Name = 'Clothing')
    DECLARE @BooksId INT = (SELECT CategoryId FROM Categories WHERE Name = 'Books')
    DECLARE @HomeId INT = (SELECT CategoryId FROM Categories WHERE Name = 'Home & Garden')
    DECLARE @SportsId INT = (SELECT CategoryId FROM Categories WHERE Name = 'Sports & Outdoors')

    INSERT INTO Products (Name, Description, Price, StockQuantity, CategoryId, SKU, ImageUrl, IsActive, CreatedAt)
    VALUES 
        ('Laptop Pro 15', 'High-performance laptop with 16GB RAM and 512GB SSD', 1299.99, 25, @ElectronicsId, 'ELEC-LAP-001', 'https://via.placeholder.com/300x300?text=Laptop', 1, GETUTCDATE()),
        ('Wireless Mouse', 'Ergonomic wireless mouse with precision tracking', 29.99, 150, @ElectronicsId, 'ELEC-MOU-002', 'https://via.placeholder.com/300x300?text=Mouse', 1, GETUTCDATE()),
        ('USB-C Hub', '7-in-1 USB-C hub with HDMI and card reader', 49.99, 8, @ElectronicsId, 'ELEC-HUB-003', 'https://via.placeholder.com/300x300?text=USB-Hub', 1, GETUTCDATE()),
        ('Classic T-Shirt', '100% cotton comfortable t-shirt', 19.99, 200, @ClothingId, 'CLTH-TSH-001', 'https://via.placeholder.com/300x300?text=T-Shirt', 1, GETUTCDATE()),
        ('Denim Jeans', 'Slim fit denim jeans', 59.99, 75, @ClothingId, 'CLTH-JNS-002', 'https://via.placeholder.com/300x300?text=Jeans', 1, GETUTCDATE()),
        ('Clean Code', 'A Handbook of Agile Software Craftsmanship', 39.99, 50, @BooksId, 'BOOK-PGM-001', 'https://via.placeholder.com/300x300?text=Book', 1, GETUTCDATE()),
        ('Design Patterns', 'Elements of Reusable Object-Oriented Software', 44.99, 5, @BooksId, 'BOOK-PGM-002', 'https://via.placeholder.com/300x300?text=Book', 1, GETUTCDATE()),
        ('LED Desk Lamp', 'Adjustable LED desk lamp with USB charging', 34.99, 60, @HomeId, 'HOME-LMP-001', 'https://via.placeholder.com/300x300?text=Lamp', 1, GETUTCDATE()),
        ('Yoga Mat', 'Non-slip yoga mat with carrying strap', 24.99, 100, @SportsId, 'SPRT-YGA-001', 'https://via.placeholder.com/300x300?text=Yoga-Mat', 1, GETUTCDATE()),
        ('Water Bottle', 'Insulated stainless steel water bottle 750ml', 19.99, 3, @SportsId, 'SPRT-BTL-002', 'https://via.placeholder.com/300x300?text=Bottle', 1, GETUTCDATE())

    PRINT 'Products seeded'
END