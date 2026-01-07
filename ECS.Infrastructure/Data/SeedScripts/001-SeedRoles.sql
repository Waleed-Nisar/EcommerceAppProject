-- Seed essential roles for the E-Commerce System
-- Run this script ONLY if seeding via SQL instead of code

-- Check if roles already exist to avoid duplicates
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID())
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Seller')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Seller', 'SELLER', NEWID())
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Customer')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Customer', 'CUSTOMER', NEWID())
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Guest')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Guest', 'GUEST', NEWID())
END

PRINT 'Roles seeded successfully'