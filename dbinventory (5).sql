-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 24, 2024 at 02:04 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `dbinventory`
--

-- --------------------------------------------------------

--
-- Table structure for table `activitylogs`
--

CREATE TABLE `activitylogs` (
  `Id` int(11) NOT NULL,
  `Username` longtext DEFAULT NULL,
  `Activity` longtext DEFAULT NULL,
  `Action` longtext DEFAULT NULL,
  `DateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `adjustitems`
--

CREATE TABLE `adjustitems` (
  `Id` int(11) NOT NULL,
  `AdjustmentId` int(11) NOT NULL,
  `ProductId` int(11) DEFAULT NULL,
  `WarehouseId` int(11) DEFAULT NULL,
  `QtyAdd` int(11) NOT NULL,
  `QtyDeduct` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `adjustments`
--

CREATE TABLE `adjustments` (
  `Id` int(11) NOT NULL,
  `LocationId` int(11) NOT NULL,
  `AdjustmentDate` datetime NOT NULL,
  `User` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `brands`
--

CREATE TABLE `brands` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `brands`
--

INSERT INTO `brands` (`Id`, `Name`) VALUES
(1, 'Republic'),
(2, 'Nihonweld'),
(3, 'Eveready'),
(4, 'Makita'),
(5, 'Armak'),
(6, '3M'),
(7, 'Boysen'),
(8, 'G.I.'),
(9, 'Tumbo'),
(10, 'Clover'),
(11, 'Eagle');

-- --------------------------------------------------------

--
-- Table structure for table `categories`
--

CREATE TABLE `categories` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `categories`
--

INSERT INTO `categories` (`Id`, `Name`) VALUES
(1, 'Building Materials'),
(2, 'Tools and Equipment'),
(3, 'Fasteners and Hardware'),
(4, 'Electrical Supplies'),
(5, 'Plumbing Supplies'),
(6, 'Safety and Protective Gear'),
(7, 'Paint and Finishes'),
(8, 'Hardware and Accessories'),
(9, 'HVAC (Heating, Ventilation, and Air Conditioning)'),
(10, 'Landscaping and Outdoor'),
(11, 'Flooring and Tiles'),
(12, 'Windows and Doors'),
(13, 'Interior Fixtures'),
(14, 'Roofing and Gutters'),
(15, 'Site and Erosion Control'),
(16, 'Miscellaneous Supplies');

-- --------------------------------------------------------

--
-- Table structure for table `locations`
--

CREATE TABLE `locations` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `locations`
--

INSERT INTO `locations` (`Id`, `Name`) VALUES
(1, 'Main Store'),
(2, 'Warehouse');

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `BrandId` int(11) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL,
  `SellingPrice` decimal(18,2) NOT NULL,
  `Unit` longtext DEFAULT NULL,
  `PictureFilename` longtext DEFAULT NULL,
  `MinQty` int(11) NOT NULL,
  `CeilingQty` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`Id`, `Name`, `BrandId`, `CategoryId`, `PurchasePrice`, `SellingPrice`, `Unit`, `PictureFilename`, `MinQty`, `CeilingQty`) VALUES
(1, 'Small Black AA', 3, 4, 44.00, 49.00, 'Pack', NULL, 10, 100),
(2, 'Small Red AA', 3, 4, 38.00, 43.00, 'Pack', NULL, 10, 100),
(3, 'AAA Black', 3, 4, 66.00, 71.00, 'Pack', NULL, 10, 100),
(4, 'Med Black', 3, 4, 80.00, 85.00, 'Pack', NULL, 10, 100),
(5, 'Big Black', 3, 4, 82.00, 87.00, 'Pack', NULL, 10, 100),
(6, 'Big Red', 3, 4, 68.00, 73.00, 'Pack', NULL, 10, 100),
(7, 'Battery AA-Blue', 3, 4, 15.00, 20.00, 'Pack', NULL, 10, 100),
(8, 'Fuse 30A', 11, 4, 35.00, 40.00, 'Piece', NULL, 10, 100),
(9, 'Fuse 60A', 11, 4, 75.00, 80.00, 'Piece', NULL, 10, 100),
(10, 'Electrical Tape Small', 5, 8, 20.00, 25.00, 'Piece', NULL, 10, 100),
(11, 'Electrical Tape Big', 5, 8, 45.00, 50.00, 'Piece', NULL, 10, 100),
(12, 'Threebond 25g', 3, 16, 70.00, 75.00, 'Gram', NULL, 10, 100),
(13, 'Threebond 80g', 3, 16, 115.00, 120.00, 'Gram', NULL, 10, 100),
(14, 'Butane', 3, 16, 100.00, 105.00, 'Can', NULL, 10, 100),
(15, 'Staple Nail 1/2', 3, 3, 35.00, 40.00, 'Pack', NULL, 10, 100),
(16, 'Staple Nail 3/4', 3, 3, 45.00, 50.00, 'Pack', NULL, 10, 100),
(17, 'Staple Nail 1', 3, 3, 50.00, 55.00, 'Pack', NULL, 10, 100),
(18, 'Super Strength Molding Tape', 6, 16, 910.00, 915.00, 'Piece', NULL, 10, 100),
(19, 'Smooth Nails', 3, 3, 25.00, 30.00, 'Grams', NULL, 10, 100),
(20, 'Smooth Nails', 3, 3, 130.00, 135.00, 'Box', NULL, 10, 100),
(21, 'Shoe Tacks Gram', 1, 3, 25.00, 30.00, 'Grams', NULL, 10, 100),
(22, 'Shoe Tacks Box', 1, 3, 130.00, 135.00, 'Box', NULL, 10, 100),
(23, '4 & 1 Oil', 1, 16, 50.00, 55.00, 'Bottle', NULL, 10, 100),
(24, 'Singer Oil', 1, 16, 60.00, 65.00, 'Bottle', NULL, 10, 100),
(25, 'Soldering Lead', 1, 8, 435.00, 440.00, 'Piece', NULL, 10, 100),
(26, 'Soldering Paste', 1, 16, 120.00, 125.00, 'Piece', NULL, 10, 100),
(27, 'Metal Polish GLO', 1, 16, 185.00, 190.00, 'Bottle', NULL, 10, 100),
(28, 'Metal Polish KOWI', 1, 16, 95.00, 100.00, 'Bottle', NULL, 10, 100),
(29, 'Metal Polish A-1', 1, 16, 130.00, 135.00, 'Bottle', NULL, 10, 100),
(30, 'Compound', 10, 8, 125.00, 130.00, 'Piece', NULL, 10, 100),
(31, 'Grease Mix', 10, 8, 575.00, 580.00, 'Piece', NULL, 10, 100),
(32, 'Teflon 1/2', 9, 8, 22.00, 27.00, 'Piece', NULL, 10, 100),
(33, 'Teflon 3/4', 9, 8, 38.00, 43.00, 'Piece', NULL, 10, 100),
(34, 'Teflon 1', 9, 8, 45.00, 50.00, 'Piece', NULL, 10, 100),
(35, 'Rubber Tape', 5, 8, 130.00, 135.00, 'Piece', NULL, 10, 100),
(36, 'Nitto Rubber Tape', 1, 8, 310.00, 315.00, 'Piece', NULL, 10, 100),
(37, 'Electrical Tape', 6, 8, 60.00, 65.00, 'Piece', NULL, 10, 100),
(38, 'Trapal Dark Blue', 1, 9, 90.00, 95.00, 'Meter', NULL, 10, 100),
(39, 'Trapal Light Blue', 1, 9, 75.00, 80.00, 'Meter', NULL, 10, 100),
(40, 'Trapal blu orange', 1, 9, 60.00, 65.00, 'Meter', NULL, 10, 100),
(41, 'NIPPLE CLOSE 1/4', 8, 5, 18.00, 23.00, 'Length', NULL, 10, 100),
(42, 'NIPPLE CLOSE 3/8', 8, 5, 16.00, 21.00, 'Length', NULL, 10, 100),
(43, 'NIPPLE CLOSE 1/2', 8, 5, 12.00, 17.00, 'Length', NULL, 10, 100),
(44, 'NIPPLE CLOSE 3/4', 8, 5, 14.00, 19.00, 'Length', NULL, 10, 100),
(45, 'NIPPLE CLOSE 1', 8, 5, 27.00, 32.00, 'Length', NULL, 10, 100),
(46, 'NIPPLE CLOSE 1 1/4', 8, 5, 30.00, 35.00, 'Length', NULL, 10, 100),
(47, 'NIPPLE CLOSE 1 1/2', 8, 5, 42.00, 47.00, 'Length', NULL, 10, 100),
(48, 'NIPPLE 1 1/2* - 1/4', 8, 5, 20.00, 25.00, 'Length', NULL, 10, 100),
(49, 'NIPPLE 1 1/2* - 3/8', 8, 5, 17.00, 22.00, 'Length', NULL, 10, 100),
(50, 'NIPPLE 1 1/2* - 1/2', 8, 5, 12.00, 17.00, 'Length', NULL, 10, 100),
(51, 'NIPPLE 1 1/2* - 3/4', 8, 5, 15.00, 20.00, 'Length', NULL, 10, 100),
(52, 'NIPPLE 1 1/2* - 1', 8, 5, 28.00, 33.00, 'Length', NULL, 10, 100),
(53, 'NIPPLE 1 1/2* - 1 1/4', 8, 5, 30.00, 35.00, 'Length', NULL, 10, 100),
(54, 'NIPPLE 1 1/2* - 1 1/2', 8, 5, 42.00, 47.00, 'Length', NULL, 10, 100);

-- --------------------------------------------------------

--
-- Table structure for table `productsuppliers`
--

CREATE TABLE `productsuppliers` (
  `Id` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `SupplierId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `productsuppliers`
--

INSERT INTO `productsuppliers` (`Id`, `ProductId`, `SupplierId`) VALUES
(1, 1, 9),
(2, 2, 9),
(3, 3, 9),
(4, 4, 9),
(5, 5, 9),
(6, 6, 9),
(7, 7, 9),
(8, 38, 4),
(9, 39, 4),
(10, 40, 4),
(11, 41, 1),
(12, 42, 1),
(13, 43, 1),
(14, 44, 1),
(15, 45, 1),
(16, 46, 1),
(17, 47, 1),
(18, 48, 1),
(19, 49, 1),
(20, 50, 1),
(21, 51, 1),
(22, 52, 1),
(23, 52, 1),
(24, 53, 1),
(25, 54, 1);

-- --------------------------------------------------------

--
-- Table structure for table `productwarehouses`
--

CREATE TABLE `productwarehouses` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `BrandId` int(11) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL,
  `Unit` longtext DEFAULT NULL,
  `PictureFilename` longtext DEFAULT NULL,
  `MinQty` int(11) NOT NULL,
  `CeilingQty` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchaseitems`
--

CREATE TABLE `purchaseitems` (
  `Id` int(11) NOT NULL,
  `PurchaseId` int(11) NOT NULL,
  `OrderDate` datetime NOT NULL,
  `ProductSupId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchaseprices`
--

CREATE TABLE `purchaseprices` (
  `Id` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `PriceDate` datetime NOT NULL,
  `UpdatedPrice` decimal(18,2) NOT NULL,
  `Percent` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchasereturnitems`
--

CREATE TABLE `purchasereturnitems` (
  `Id` int(11) NOT NULL,
  `ReturnId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `WarehouseId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchasereturns`
--

CREATE TABLE `purchasereturns` (
  `Id` int(11) NOT NULL,
  `PurchaseId` int(11) NOT NULL,
  `SupplierId` int(11) NOT NULL,
  `ReturnDate` datetime NOT NULL,
  `Reason` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchases`
--

CREATE TABLE `purchases` (
  `Id` int(11) NOT NULL,
  `PurchaseNo` longtext DEFAULT NULL,
  `SupplierId` int(11) NOT NULL,
  `OrderDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `receiveapproveitems`
--

CREATE TABLE `receiveapproveitems` (
  `Id` int(11) NOT NULL,
  `ReceiveId` int(11) NOT NULL,
  `ProductId` int(11) DEFAULT NULL,
  `WarehouseId` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `receivingitems`
--

CREATE TABLE `receivingitems` (
  `Id` int(11) NOT NULL,
  `ReceiveId` int(11) NOT NULL,
  `ProductSupId` int(11) NOT NULL,
  `ProductId` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `PurchasePrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `receivings`
--

CREATE TABLE `receivings` (
  `Id` int(11) NOT NULL,
  `PurchaseId` int(11) NOT NULL,
  `PurchaseNo` longtext DEFAULT NULL,
  `SupplierId` int(11) NOT NULL,
  `LocationId` int(11) NOT NULL,
  `ReceiveDate` datetime NOT NULL,
  `Status` longtext DEFAULT NULL,
  `Notes` longtext DEFAULT NULL,
  `Reason` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `releaseitems`
--

CREATE TABLE `releaseitems` (
  `Id` int(11) NOT NULL,
  `ReleaseId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `PurchaseDate` datetime NOT NULL,
  `Quantity` int(11) NOT NULL,
  `SellingPrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `releasereturnitemapproves`
--

CREATE TABLE `releasereturnitemapproves` (
  `Id` int(11) NOT NULL,
  `ReturnId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `SellingPrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `releasereturnitems`
--

CREATE TABLE `releasereturnitems` (
  `Id` int(11) NOT NULL,
  `ReturnId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `SellingPrice` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `releasereturns`
--

CREATE TABLE `releasereturns` (
  `Id` int(11) NOT NULL,
  `ReleaseId` int(11) NOT NULL,
  `ReturnDate` datetime NOT NULL,
  `Reason` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `releasings`
--

CREATE TABLE `releasings` (
  `Id` int(11) NOT NULL,
  `ClientName` longtext DEFAULT NULL,
  `OrderDate` datetime NOT NULL,
  `Notes` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `releasings`
--

INSERT INTO `releasings` (`Id`, `ClientName`, `OrderDate`, `Notes`, `Status`) VALUES
(1, 'Sahagon', '2024-01-01 00:00:00', 'none', 'Not Receive'),
(2, 'Maandig', '2024-01-04 00:00:00', 'none', 'Not Receive'),
(3, 'Yabo', '2024-01-07 00:00:00', 'none', 'Not Receive');

-- --------------------------------------------------------

--
-- Table structure for table `suppliers`
--

CREATE TABLE `suppliers` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `Address` longtext DEFAULT NULL,
  `PhoneNo` longtext DEFAULT NULL,
  `Email` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `suppliers`
--

INSERT INTO `suppliers` (`Id`, `Name`, `Address`, `PhoneNo`, `Email`, `Status`) VALUES
(1, 'MIGHTY STEELHOUSE MARKETING', '625 M. De Binondo Street 1006 Manila Metro Manila', '(02) 8734 0718', 'mightysteelhouse@gmail.com', 'Active'),
(2, 'HAN\'S INFINITE TOOLS', '801 TAYUMAN ST., TONDO MANILA', '(02) 252-6141', '', 'Active'),
(3, 'CLK SUPERTOOLS DEPOT', '', '', '', 'Active'),
(4, 'FABRIMETRICS PHILS, INC.', '1 Candido St. San Agustin Village, Brgy. Talipapa, 1116 Quezon City', '(63998) 994-4056', 'boyet_mina@fabphils.com', 'Active'),
(5, 'Republic Cement', '32nd Street, Bonifacio Global City Taguig City', '​(+632) 8885-4599', 'sales@republicement.com', 'Active'),
(6, 'Nihonweld Company', '17 Mac Arthur Hi-way, Potrero, Malabon, Manila', '(+632) 8 361-0255', 'sales@nihonweld.com', 'Active'),
(7, 'Prostech Philippines', '1709 Investment Dr, Alabang, Muntinlupa, 1780 Metro Manila', '(+63) 960 881 8409', 'info@prostech.ph', 'Active'),
(8, 'Pacific Paint (BOYSEN®)', '292 D. Tuazon Ave Quezon City, NCR', '(02) 8364-9999', 'marketing@boysen.com.ph', 'Active'),
(9, 'Eveready Philippines', '11 Mcarthur Highway Cor Lanzones Street, Potrero 1475 Malabon Manila', '(02) 8361 2345', 'sales@evereadyph.com', 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `transactionlodges`
--

CREATE TABLE `transactionlodges` (
  `Id` int(11) NOT NULL,
  `Date` datetime NOT NULL,
  `Activity` longtext DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `CurrentWarehouseQty` decimal(18,2) NOT NULL,
  `CurrentStoreQty` decimal(18,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `transferdetails`
--

CREATE TABLE `transferdetails` (
  `Id` int(11) NOT NULL,
  `LocationId` int(11) NOT NULL,
  `TransferDate` datetime NOT NULL,
  `Notes` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `transferitemstores`
--

CREATE TABLE `transferitemstores` (
  `Id` int(11) NOT NULL,
  `TransferId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `WarehouseId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `transferitemwarehouses`
--

CREATE TABLE `transferitemwarehouses` (
  `Id` int(11) NOT NULL,
  `TransferId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `WarehouseId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `PasswordHash` longblob DEFAULT NULL,
  `PasswordSalt` longblob DEFAULT NULL,
  `CreatedOn` datetime NOT NULL,
  `LastLogin` datetime DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Roles` longtext DEFAULT NULL,
  `Email` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `UserName`, `PasswordHash`, `PasswordSalt`, `CreatedOn`, `LastLogin`, `IsActive`, `Roles`, `Email`) VALUES
('45aa0da1-114a-11ef-a9be-74d4dd1e162a', 'desidido', 0xf23d6370939a490a9592ebe078c1f293d90599ad2981365a9aaca0a2f0c4c4fd76a7ceb2215e3b30d3e3ef903379fc8c75a45b11a7ad1a7c3557dac2af4fd31a, 0xc0e2d28c9fbb28af11700ab287a5b937033cf35bcff6ca42666b1742137785301e0556dffc13c153cad064374067118fdcc71ec3180d2c7f56d7eb7387b0fd79901c68e50f0eb070a410b4161d11bc7fad66c9a60f40adfd88fd55bdc43a036095bdc611ee385073ef41b80346e7bb0612963c3c54423de7937432f33a5d62d7, '2024-05-14 01:00:16', NULL, 1, 'user,wrhmgr', 'joseduke325@gmail.com'),
('839ac473-f983-11ee-8b7c-74d4dd1e162a', 'dukz17', 0xfb050ae2746e7cd840c9d58b339a699a619fa9452d90bf2db3e842b30c8e93f5fdd16dcbfe5818fa3183022f99b3bd7c752912570208e38c8f12c11affd3c4c8, 0xa94ce0e293c9a06af8aed53d6cfc0bdf8e5d3c6c0343930cd6a62784f4a9742f1b45f3b36543432b2fbbfebfcecf7074a4bc6357d4ef92af94f72818a3ea3ad3a104836c15fd7fe8aab534dfb67dafa994a5d414ef22e90d73e57c260fdde6ca5d6ad5ea5c20c1c74a0202ac6a91ec16d7873ae21fdc4c365d41b5ab82a22ab3, '2024-04-13 18:49:33', '2024-05-14 05:41:32', 1, 'user,wrhmgr', 'joseduke325@gmail.com'),
('aff36cb4-02af-11ef-bfa4-74d4dd1e162a', 'macjean', 0xb00371e35f1f8f311c02b1bf30d75f851b7cb0867922326cce83bc1ff2037cbcdce7a8866787a4b7e7d1207584a64b83c529dab230b7299c9d99a520e3c73525, 0x56f10ea60c6a352303aa980ffb4e9994c94394ebaa48a6085126425e2030451e741b5c140c87642df1cc87a94426eb01e1b0616e7ade9e56b92b68a35adc1c9b7938cb93a9ae017c7646d5027dc525c92c26e0ce792bcd753fc938dac4da0f2df2bb283c38442bd401061f55fbe7f62dffcc228d272f6686ddf721542a8f2bd6, '2024-04-25 10:58:26', '2024-04-25 12:33:54', 1, 'user,wrhmgr', 'joseduke325@gmail.com'),
('b40ac649-f7c5-11ee-a99d-74d4dd1e162a', 'admin', 0x25be4dd33f0277dadc4943061f53d69bffa00ceefec74ea7faff6692a2ca829c2f4e0a12b5b996d04c56896f5571691214ea45137ac40c21dfd0270acaed6d2e, 0xb7a3a64db9bfeb62b2f1c6ec3d37994e99b5827a32a60e1a108bcde307fd340e4c01288242de2adebdb5e03e142b404864c04da1d6c3b002d015467acaa9c81e92b501dbbc66df6ac2be1c9d1377288bbcf7a08ba7484d56af8d42f62502e1055b5b2abbea51272ef5b960b3d988c16538ca0433e857fa92aa3c261b24aaf556, '2024-04-11 13:38:19', '2024-05-14 11:11:35', 1, 'admin', 'joseduke325@gmail.com'),
('cc69eefd-02af-11ef-bfa4-74d4dd1e162a', 'joselozano', 0x6fd60a8481f61396473fedbab2084be6b62eebc0c31f9419e1ffa491191a5c51ae8348f5b6d0db2471e217592873c2b36e1ecc64cc1bb6c7c5edab33c9beffe7, 0xe0f257f4e1cd9cf8803fab59d5ef475ccee73dd22ae8838874724242f16ebc6285e352876f14619618e60c7356fd814f9282f0982c920244c76c134148c49cc05e5eaf6e0a7e4b77e5a6f482eccbc878af653c0bbcc9faa06aab8999f27062cbe523bd01066635256bc373b9cfa8ddeee39132015f2660ab8170dd88da1b1389, '2024-04-25 10:59:14', '2024-05-14 14:17:13', 1, 'user,purchmgr', 'joseduke325@gmail.com'),
('e4283837-02af-11ef-bfa4-74d4dd1e162a', 'jamesilaw', 0xa580a93dcb8cea3542d27bc95f873886b38b17b5ff7148599f71905ba1f1413428023ab07972a18e6cd1482175a3e3d22e8dddc15b2bb490ef936448f04cb75f, 0x4bf37500fd31e0c3222dbd22a2ff1374052f12dca1dff249b2ff56a97b5c77ce63bf2a94826b4a5aecbb49718760e1c8f567683588d3d24ee6eed194966ca28764a592f93f04b1bba8e196be52ca44571888a24521678769a28e8cd73363cdce6a7bca3efa727b72d285543ff0b304f31313041096ee95055afc507b74f6328b, '2024-04-25 10:59:54', '2024-05-14 06:45:12', 1, 'user,salesmgr', 'joseduke325@gmail.com');

--
-- Triggers `users`
--
DELIMITER $$
CREATE TRIGGER `Users_IdentityTgr` BEFORE INSERT ON `users` FOR EACH ROW BEGIN
SET NEW.Id = UUID();
DROP TEMPORARY TABLE IF EXISTS tmpIdentity_Users;
CREATE TEMPORARY TABLE tmpIdentity_Users (guid CHAR(36))ENGINE=MEMORY;
INSERT INTO tmpIdentity_Users VALUES(New.Id);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `__migrationhistory`
--

CREATE TABLE `__migrationhistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ContextKey` varchar(300) NOT NULL,
  `Model` longblob NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__migrationhistory`
--

INSERT INTO `__migrationhistory` (`MigrationId`, `ContextKey`, `Model`, `ProductVersion`) VALUES
('202405241157498_AutomaticMigration', 'ASPNETWebApp48.Migrations.Configuration', 0x1f8b0800000000000400ed5dcd92db3892be6fc4be8342c789de92dd3d1dd1ed28cf84db3f3d8eb1cbd52ef7ccde1c2c092e7347226b45ca63c7c63ed91ef691f61516a4f80302994082040942a59b44000964e24322f197f97ffff3bf977ffebadb2ebeb07d16a7c9d3e5e38b47cb054bd6e9264eee9e2e0ff9a77ffb69f9e73ffdebbf5cbedcecbe2efe56e7fba1c8c74b26d9d3e5e73cbf7fb25a65ebcf6c176517bb78bd4fb3f4537eb14e77ab6893aebe7ff4e8e7d5e3c72bc6492c39adc5e2f2fd21c9e31d2bfff0bfcfd364cdeef343b47d9b6ed836abbef3949b92eae22adab1ec3e5ab3a7cb6737d7572f3ffc9ddd3ebbbfffe34f17c702cbc5b36d1cf1c6dcb0eda7e5224a92348f72ded427bf67ec26dfa7c9ddcd3dff106d3f7cbb673cdfa7689bb18a85276d762a378fbe2fb859b5056b52eb4396a73b4b828f7fa8c4b3928bf712f2b2111f17e04b2ee8fc5bc17529442ebf751e7fe19fdea477cb855cdf93e7db7d911711f38550f8bb4537cb770d3a38882e7efaf9f1a38b1f7efcf987c7df2d9e1fb6f961cf9e26ec90efa3ed778bebc3ed365eff957dfb90fe83254f93c3762bb699b79aa7753ef04fd7fbf49eedf36fefd9a78a93d79be562d52db7920b36c5843247065f27f90fdf2f1757bcf2e876cb1a4808c2b8c9d33dfb95256c1fe56c731de539db27050d560a55a95daa8b436f9ff05f758d1c877c542d176fa3af6f5872977f7ebae43f978b57f157b6a9bf54adf83d89f920e485f2fd81992aaa7b65928a0a508e5ccd0b2eee0f712bb8f6bfd25b22a1cb550b76fd10d8fc07c7fdeb9ced7a8c80a6ec790018e152ca6ac7339b6bd553e27f3787b54a465feaefd19e7d4e0f19b32cf75bfeedd966608b398d17ac68b32599abe84b7c574a1f15269feddeb36d9927fb1cdf1f27bd8b36fd638b519ef5d53eddbd4fb71d0a628e8f1fa2fd1d2b9a996ab3dda487fddaa2bd5597816dadd2b08602c94a2ba13c3d9bd8c044d7d62693a1d1603eacf57066880d4bdd56f45e5fdd56943deb36d3f87e93ae4b3a43355b2bf36296b39cf1609bc3fdfc6c504ad55870a994ea3160504af5b8a2b6b7ee35636b858c605b9b745d4bdb4c503bc943ba26633fa0eb92e7e16c1a3757eeed7472ff3633a56df75605cfbdeba177d54a7ed947c966e874f09cf37797eebf0d3698b952fa1c65ec7a1fafdb5985ade35db45d2eaef7fc57b5e3f3d37271b38e0ac2f6b570adb9e5c21cb712de09f9e89d771daf8b91f12adeb24996ec6fe3e4b776c1de132b2c2ea46f4fa7f774dec36e97a7479d6d4f6d6639d2b40dac72a84d2b13d0461d536d9b538f596d8bda4c6aa3ea34b45d4d06dba6d56ae03de3e84e341dabe4fbd8cc4a427bd15cea0207cf6abb4ae3cd659c12de782103d46a2059692e94c7b69d1f3878b24f6c5f1029e72bb8b54a36a8cd6826a5e578ce21ed6f16a6661eda35ac810f25a3961735f7a06571a5106c2dacb2d8d9be9ab3f5dc2a56dbcead4b9efb77cefdabce243d164a0a8d739f9bfabc92d5481bf79664f09d7cdb6df94354713fc3d555bfbdf4a1a69b62749aadbc7137d841a355b3d10ee4376fb84385accf0f3a3408bd6364cd98db606893d9b2dd9c12bab0e72e5543e1ac7acfdb557eb6abce3b495837fada491a76aa8a6c97e88f609d6d32b5d560bb4d520e737bc7db7f6aebd06c44a999cc4d1e7f6b6a0cf3c0c8966eeaa56f5cad59fc858f2b3ecdedd32fda1dac222713f27d54e75d7147cb981dd8e13297997ac70b64ce9c9bbe07e686b5219b614616f11276db637a56c9e69e0a137b834fa57136f9ccabed526681dd933bb9c534ae2569fa579958345947594c8f3093d059525597f55449654a28a0e5a6c94761a3cd3ce8be8ed0b67eaa93173d6b4caada70b5f4bc4a475faadd1ceeefb7319fc00736d9d5ddc36a0838b97878c37bf5908d2ec2ab3467e3d7f29e45d918ef1c7ade4b6cf512782d114846b45d374fdf159ba18d6d36a88975aaa6854d96deb38869a55565d1cd224a166445a5e6b35d6ad45ac120d6361b24d63a5523d626cba0c9adedddbe8770e7a9ed04a7a477fb0ddbf79b488cfac67c6de96884027a07cc809ea17473391dc72d5560182b89e8999ce3413cec34fdbcb29fd24e1d30c2e0852487d24c4ef74f6f1fa1ab51f0e378d241bc8b5d039272122bd2e929281fa9f55aedd5dbf4ec390be81bacb3416d0fcf5b59f63c3baf099cb5ed441aa9b75934e4396c333a20850167c10eb2947cb6034eaf2fe4a10fe90a248fb1c56e2c9cfe43ee3cd666754de5d966b367d9f85b3ed79fd3648a05d7cb5d146fc75fd68db11b67bf013ee4fcb02a7e1e86d31e1d8eb82a78688b80ee06217a8e2865c077f0465f07803569f71d492b017dee318e0a7bedefea9bede86c50b9aa62af1e1512671569d20a8dc8e6b1dfe1fb35432f6d36f0dda23cbacc2f1c9dde8b707e078dcc4fff4b1135c9172ce736a3fef2d931cf47e8469eca169a19bd7286971874dbac4bbebf2a3c963febc1a92e0e347277b11d3dcd91fe58f7137a1ee94b030b3cd7c7f2a0ba47c9d857e3102ff43a523a0676346a6ab2dbbc8a2a365de6950a10eff2caa59c28d74e15c3cccdf31baeb3c9e9c9e41ce0664267aae14e2946333f9dbe11b0e26d1a53d4b1e6a47188e95b072e0786ba1b38ebcaa9ae48b8bacc74ec3727c6ed5837497d5bb7a40379e94d3ae1a5bbc11941ffaba1fd1c613978af4f6468c83b43d2dd8eaa3eddb50e298ba9e96e0e6a05cf5b7d4e8a9ac2672d6b56454761cdc320adc1e444cbba393b1ac79b65cfa3236b9776eaf105eef68e7ed652d0c0cf5aaad48f5d177d723be51c88f73d25dbc087ac4dd3fb6995f3232c824e79ce2780249fe42288bbfbaf27bae148f493d963bcc2aa051dd683d65c95f6edbbe42a8b9fc7ed549378216d37710bee372523a3faffb9661cc649ee796eef40557739bc9301b5c6bbb95cd8e2ed5aa47a1cdddb3057289d47266def612646faa9dad50a2e3546369a17b3b8f102fdcc6f9d7b41b44aa92881ab4e029db76e3177ea67a0ce392b9a89b603ce7ba576c3d8649e8b6e57e983181986e8e84546bbcb7d819ab6901761a0cda2573b423eb7668e03fbe6ac6fce86cdac0c1b9245433765a6b461faeb3d331b0ecd95f2c8392a23e2be4937777dafd60814ce4ac434e29cd81993854b76a3659e1ff67b2e9de6ae83e0a5d7f90e495557d94beeea210fa9227ae5b3f53a3de8a3970ad9421f32bf1ee2cda823a69095617ffec74724acdbced45196fd33dd6ffe12659febca7f8993a8f03c4c196934ea37d136774ffdf99e15c27e970cd6356fa22c7f93dec500257dc1d759a9a69a7efb25e5335a94d82faa78b1f10f3bc678d58a698d675996aee37270346f84c128b3dd16bf4c360b5ac8d92313dd80b09c1bae3fe27bae317893b8f65bca9ae15df282db16395b3c2b27f4c22d77b68e36aae8385b1bcba635eb39b969c7d54bb7697f506ae4ca8a156a3d8eb6cfb9e9c5d55f9ce4aa668b93757c1f6d4952924a133563c17d538f9cf282ddb3a4d06a2459501ad08d3faf36a5a951ea2093bc2e570206c9d06ceff8137a1fb8ec4f81a5daf706ea00b2da70c323427e2824150ea605a4d23d94eac5d7375ec058afe1284a12c80b41b1596576b1f2e8e2e2b1068a10711f3a4ec3e40478d2488152bbb015e2154dc7e817a6ae9622601811a4d3645d9a0070aa801c73d35f60bb27045aa70f28f5363182bc02ac895862c2831ab5c409cc94102702d9369aca5cc126b77e42bcc9fd41a95a8c28e50775ca8dec662b15050a5a0444a292db0a94785d003c11d4cf009d4636a6c0a9b1df029a87e1505a06dd6688aba5004978aad6c7e0d347e6f260f969f99f4e536ae5426946e779e73c5048320bd1086974e41166702cc05a301623c2800f8086694302e1efc8982158956ec019b49d89b3e103a5615a9e3217d0eb43329ab48f11e9081e69a39bd45cc8a0ed613cbb81b5469e3e10ae915718068326d41a061a4adcb5162a502843ba7626046eebb90dda0b8ee6e64c0042b3fc43582f5142e4d9a30270c9313e148158a103976c8ec1a934d02b4a953e0a495376c37fe9b182c4029301793ca5b7c4211c432c88f3430d0393e112ec1a4aedfe4f0fa19826a66d4aea6668df6dd0e037403d6f7d863a895fc3712e4ccb233c7e860299d6538af5921e8dbe110430f53c4cb7d6c1fa2a44783660a2224775c2330a40151f3e423518fdf94154e6c20346e5fea234417457369b19dd8c545210b07166781d5c0da36206a82530e469e6ef83deb6e84cf05b7b99a3014a7138e71ab1b2a73aa082b96354626172704a7d4442a5e04ad22b260953be9c5387c53e380c7576c79a3f21fec29ccf8118ece6bd1c9d261cb84d14a60ec4db3fe9265190da0f0e7965440aed28a70928d7038b3339c0415a3225b042ddf1d10727b302806ec2750bb5a0172c248e7c4137d0250b169f8e862a20589d73e4aa81ee2806c1dcd0aa7031394e95bea2b44088e5e9d982a46b56edfa65a00519e60a066fffa41664c06b98cee93d554bc28526b98f11b6d2d4b3321966f5fd1782fac4636462582204cc6c8104048fa5a3d61c703384d349231713a0d5d867212ca7080151ad91a4bd03370e7807dd809b319c7d5c9ca3f72ba531feefcda131364d28c3036eaa70ae43d7da63198dd619c45d3a131713e215eb2e4a13bcdfaa33c75225e2481758950adb911e7bd003ba0e9b2f5c001917e3f488c60545698b1881d4bb89a186efa44cf59a589e30507a3dc4a3c5030dcd3a463999d89240fb30342b5913b7b517b2c8d6b27b509f90d56ce6cd37de03b79ebb3cc9e17cede087c6f61d1df8587c603fa69143d4238c79033dd2c541182c58dc5a0c6ac620b6d8bbe5fe0e7f02bd5363606202b41afa8a648cf8be5d43084e6c89229373002d6ac7720d606eaa57c7006449fa43754fa7006d888e39c09b7c81168d5b3d82fa0df340dac0c4f4400df4685a0dca8cc1461ba1593c1a16a29fdb9c40e3b19d43d8a7d0b47f022c6afa26843d09248caf1e2b784c5f198c63df5330b60cbc4161314c06201293d264a0c484419bbd9b807e5ea7ef6e5c5bd3bc8a04b955a7ee2aca96fdcc0d07c80d414d6a399870d206fb281c55a909f76a986009b15f152505444fb69ed8cd216443802f9599e9a67c637f06096a7b345bc178107e4f03b87340ec6940b51bda910e2324cee368b045e2e922f585006190232f4006fb3280fd292cae310955509063229ac8c80502244fbebc1b8e56958ba951aaf65508eb2fd4c6e9a973c1e2931bbe27a788b58cf93485c350cb2fcb9890bc4cce4bb07d2d93e40b2796eebfbdb82d92d857287eecef19ab42c86655bc49198d05e91b96d7b1fbaa38c16fd2bb6cb978d904a3ac6354b4c900b0255242480e809210ed8244a8884187133a060634106a6f43ab64da5bc70622957d09d1688c7b0389323c0044a08ac860285e396d8f1944a2759a6fe20338490558028e1869026aae476824255c763110555f4b4164a1376e24c27cdac3e995e603519a3a199289983ac3a61bea13244d2fb4677806923a5a64229d17c35aa193d8042e8fab24817bd544b2f59d2f9c667d75caa29ddaa1815c06b31acde6914ce826e19c05ea24e17482440a1d628d854e64b1dc99d57158ed9ad318546c020db780b567538991b27d9b498da561332a6dc337e9e60e87a590c748b408c20e11eac4bb976808f60f34fd77a27c2d84cc8a2980c703eb58e086a8d80d4b920da2987c468ab50daf50ac7891edf0ae20ec842458395a11216fc33076d49761c3c4a3be0113e8093c0c164dbd8768000f940d6704c80dc9a53516354281688d0e98bad2cae4c4e501c434035bdf0d66d65706dd60650295da6676c6786b28e3bcc381b3c0862b11b3fa4a4089882510122dffe182504cfbe6500512099e5bc3135a081413b422d1490ca70ec80eeb84fe1082a34fe2702244ab8410a18f57a972285a9766bce9c3538ead7ee4d08914e11115121666d19dc026d35540243f8aa0e8da4b13f8cf9db8a6536c72cda0622108d078d95ccfb0eec2b93bb1eaee8adb29d71ea2d6043403044c0d7f26ed921b03a0096c82fb441a7112429e8d3387502272598910799949e4567d98398a50d547983603a1b798bb71a450b16ac24d01fcc101a714b11df73d8cd282434c8db52412361229469fa5b94735f4e826def4c69d1265079f30f4017920ed8d86e4515912f65ccdb3021a84671a51b57bc06659c12f5db44c298f5d5c494b79e0221046293a1a765a99e9f2db0d1aadec060d479df04cbd325886cdc98a4978e0b3560d6ff2b35607e2921fb102245d0a483f1a954c662ef4e30fe3c0406ae411074407d0cefe049c68a2080c99fba78207ecad5e27142b3b1d736c0f89866a4862aeecc734cc319feb544111c615cd4dbb43c14daca931b7df461122be6f350caa8e6f5d884d75704b1ae083d414096814adad71553d444d4dabb81197c8b475311d460627ca6e57c2d3a10af7cc0b0890e8c6b7c39dd991afc01a7451432339b3ebde71d43fc147ac8df428bb3216be654790e7e45b32a88b528d5cf5ee4c41465187a6800c9b5b3e0401a22e4c47daab313bcb34cbcde46153c7a8c6c7a61b496a5c635ac17de06057bd351ac6b9c1bd233afa70078f08bbc493019a4bc7f1b5a6c667a0ad446db527c1dfe08832f6a649bbad51862255e85a3f770431609eeec61039e6db8eae9206df9820ed3e69ddaa696e339076a09a6b85e44b1293ef4269dc76d1a5463c7f35161e49965e4e5e31c75166a95a6cf9615ea6dcc86fbab5a4eac3085c421a5c1d492b3bdcd9516741275ea3d6ae1471f746636d74813e7550c9e8bdef00cca0fe7714f9103620b46e7388021f30ccba0e5e34834ce309061c0bb02f186080d5d7de09e30bf6fe32268a34ee46f091467451020d13b393121512d04d7ff38034bb259948ae5602ed23492b115acace9bd0a4f71514d1699e0e1b98845f0dbb1423fc3218abc1b948050700265162be027006016f0164c64854517139dbc5273d46b75188bdb1a87fc83e9e721c0fa0c5e3f4824af30cba49bb5cddac3fb35d547db85cf12c6b769f1fa2eddb74c3b6599df036babf2ffaba2d597d59dcdc476bced6f37fbb592ebeeeb649f674f939cfef9fac5659493abbd8c5eb7d9aa59ff28b75ba5b459b74f5fda3473faf1e3f5eed8e3456eb8e45243fda6e6acad37d74c7a4545e356fe9ab789fe52fa23cba2d5795cf373b251bf0e8bb2bc046d47585c0bb6eb55feb375a75a1e27755f0e6faeae587bfb35bdec37ffce9e228d0ce3b70895a2bd5579cd1e2a94fc93313af92eb4af3f237eb681bedafab47f7820b80e7e9f6b04b70970078e9e2d559c27f7569b45fe994eac67729b55fed28150fce653aea23741d158e17f62196396bbfaa942e575207c9785829809006a90c321a04b58f1eac11d8fa0fe801404de171f0d73e7c93e97453e8142bb34926277ca6d3ea44de10a9694372e0f47ecbbf3ddb48a4ea6f56545eb0a3c92b11aa3fcf0cdbc7f78faeb05dbab4e88b6db8f038d816e3c989547471e6706a6dfb0b1d868d95639a9dfe5775ff8c10a43bdab4c54fe3cbc41e3d78d171b073a5ccc957c87ceca95f9ad5e9f06e41d6e4845e414bfaee148c42f9024e6e44f3914ea77e33269312bf5b4c995d07119d6953e73b424793afdfb6bceb0092dd140b5595c4d28c77fc62c167bce6ab2bbe9cd832d5e65512e974dfc6c96fb2e15b7fb3e853161772512889df6733fcaba79cc3073ff86a9530f49172be07bea7ee10de890eef12ec7d2ca157f0a20fb4636847b5d633a6ea7dacc7e44920324eafb54e02451ab8eb409cd2ac177b87a814b6b44a6bbe4e333ffb350f85bb2cceec44f4f213dd60d490f0ada7ce96e3d9ce9bd138862ef60f1fc9803f4afbb14c2132d6fc55d6ac4e60cde7f30c16fc0c263c39710578e0d0978c73b0ec38f016639b423dd58fda550a532bbe5b6c2b0881ff3a9b0a9a80803835b73bb4d5f857b7673b0916bce6517ec8243eab6f16a6479a338948f5c986b328934fe4ea6fb319b0ed355d770bac01cbaaa947eb1cc7d7bbfd86edd5f1207c9e1d7a5c2fd1072ecea7346bdceafd3e7d6f30923808113ba94af163763d6833a9ed80ea5ebab3753e761f9fbeccc7298c34801c82aa9f22f6040297bd3fa0dba7eeefe1bb3acf369b3dcb24f3acf96881bccf69a25a00f5473a9d97bbd267bf48a5fa3496d9ea7b75e7764ba32238649517f846c6798e9edf1c4d7ae66e0b783576883de80934c6017e53b14445fcfe50f7f03c83147f65dd17a155249afef0c4088c834db79b520d0fca4aac9b32ed8652103602f228dfadea1c729a49a47356a10f4485ca6fcedded62c10fad2cf6b1300221ec64b9dd133d4a023a2468bf8fb5331fb43a36bcb1b75fb0b591dafa2cd734a5c75aac353185bbbd8d861ac669b954b7f5a05221dd4df1b158eb7f0bd92bc89d9d3a234f8dc9009ff0d4f93957a549ae6ea989dffd9c3e3c18835772a9e1ce7c388eb2fed603527efebbf865c301c5d87eb6b81777bfe12536802eeba658b48eedd72c912edb351f67034bfc29be332b408d60dadb2420901acb3e98e3adea073f973b5c8d69bd5ed8223444b3f5bc7672a54247d19d2e94e6595b3e506da906db76b4d12a06e7eeb9c7aa25310e3e552567abdedc39647187cae787fd9e4bb9d988551f414019ace9979d88d16e136783fd634cf8e1781763c8db435d5b7a1c941755aa4bfef6ab85168eb2ec9fe97ef39728fb2c29e24e8a3dc59b682b2f913a2916e8dcb3629df64eb23984cf16a7845196bf49ef628996f0994eeb75566a05a917daaf16b36aba95b74aaa4fe3dc411a75bc2a9ed5e42c4dedd597e67fe359adf26ad671b756f25d384f2bf9cd2a0f6bb29bb36396e5820be74bbc295c9cbdfd76f39fdb8b22fda2fc79dc186b73bc8d92f813cbf20fe93f58f274f9e3c58fcbc5b36d1c6547677a9503b727eb4396a7bb2849d2bc72b547f0e8f6f887c2a31bdbec5672717bbf7005952cdb74ced3055d27cd5e8a2fb4cbbfb26f722fd7087acf3e2d303574b9920b5e02aaac68c1d3655cc8b5548fbf32deefe5e64a94e76c9f14b95835ed5d1db6dbe8b670fbf729da66ca3891c9b74ed58e956cd3e4ae74542710caf707239d76061f4ea7e8bf61545a876a473ac54e545efebfdeb3759c95087b641096384cf5a8401c94050b8aae9733a122799c3d799d6cd8d7a7cbff2a0b3e59bcfef78f62d9ef16e5d6f693c5a3c57f5b374258aad8b5a02988554f4150e754deae7ea1e89016d42ed884ca2d2528f85ea312b1c4bceab82c58cc8b1799ecfabb2d3908efb2bbb4019a0b56f3764a958c04d80959b038b8b29e0cc992ba861c833d2041a9341a57117623ae2a3668b889ae25ec6a6f4b0e9bdfbaf7b2abe1cec7f82eda7646fbe39f78e7f135094ffedeba96eebeda48951c5d610c8382e2016318b9da0346ffd953f47ce17cfa041c853d2045401613ecb9eb2c29606e31f8cd0a5666ede1879d96aecbcd740d42aa7edc45086d0dd1eca7f7d7a463cc74b6461772133dd86171b6be42b0bece8691ed7035f9cf0a76c00acf576d27b2aae04c67b24076d3429fc710875bc18e07f1d58b25229b924e6680c217c130952a3eb9b163a52d398815ef7b971d5f624e372eeb4b6bc37aa8ba193f8c487d0b6fd4f5db898dee13185cc2eb8c290e13711f5bc183c19baa77d385a85155f910e965579565676ad93da80d02d84d53b863ce2f2a26d6dae4ce3eb15e76b10bd438f61ab84d51fbf51a46a6ba5437d066e86134da2fc44e68869ec7e6c4a9cea30f627fc2e0352bd8a1217a8eb1c3455b72a6b83cdd33286bd442ceb48285acf7eda0ae132fa74b2e273b39a3da0704ff57c122ebac0c4f5919eadc62050b59ef9b4fde3730c577e84e75719f0df1c9176b8847ac60f12cb828b05daa55054356c05de75d4ed1ec620536c26d5b4ba89fce01b1e8626b9886196bfb7ffeb6a8c65555b0b8f0ad815a2f594ec1d4759735d27db4ebda8d960fd5647242152c221ff215e5d398354f6bc9e3d9443caf37ceaf30ce2a6e3e2a4eef2c2a58488ea0605c39b1708118d0e7d44866a1e2836a5a80a22e9ea6c0e6dd21de8ce356455cba265fa26225b65f2ede465fdf300eabcf4f973ff6b871d6711bd5a2f4769bde5adfd8e8388c1a464bf016e574440a9ea3a87429cd6dbd481da9dea6e9d67e4e3cfa909afcbe0b36ae9e6559ba8ecbb9b0b9da53bb94f8d8faa8915cbaf1e1b85914ac747c7a540d2a5c345d889fdf1eb6797cbf8dd7bc7eae0c14ef70ef9217dc02cad9e2e8c7a778e895ada38d2a06cec346df8ed27a52db71fcdc6dc71f14f25c3bb042a5c5d1f6799a64f93e8a55375c7c364ed6f17db455f997b212f54ec15743544e79c1ee5952e813954b4a6d5d9f3c6abd0d7949ce26495cae04dc90e1541f14e3606adc82885dd87e9c10481680760424d825ca983022835617af6b1210553639492155793b9dd77cebf6dca38b8bc74ae7f95729756be7a74f844595571c409e30da7e3ba68a5d567d99447f58e0cf115e00698c83161b6036efc4bd0205f105d2f6569341ecaef6e3892206168b7fd0882ffcfde046f188f2116c7fbfde1b0b3faa1b974e6380e4d0e72d93e71aa4d699cc5fcdfe8c8d41d35e0b049026248664e2e84286fab675b42136fda02500b3a71f4a033584ece03b0b8ba8055348a6915754793096ec80e5df6a92b1a54ecf234d6f0fc3a29a749aec695af99f2e552f502e6d77832105b8a0128942c9a1dbe026af5b48adbe6d701c26d8c39b694cf1992168529dd3134a73d139717217c8e146eb104cc556f935f8a30dc4e5195299ff930dc1734f405b4da849741a531bee4f09a9cff7a476ddf547130292240f3a404bdab4e0f1a473171408a4102f386d8f825d89f5e1c9a2caaa8f3dc04a7c593a9b19cf88ad01fd7aba33e1a450eb392356ee79668034d857a6daa160673e045081029a0b9a8407f95eb114c8346883e510273e1bac7a9ff1da4d822094909f3d8229d58fdd1e8177ddd3711e38f99e76edb41004c3692cf735de19314ccc62135b82448056f42c2036e5ccd51b6a3e0de92edab0a012bd679093c496dd1c3339aa04e7ad9e6da2200c6a3f369117c514924ddd39c50d4b2ff93fe8f5a0a1ac4f787dab29c5417100272baa5365b12d406ae8e6bbc18b3452a96f131e85d6c8d7501e02ea26bdbcd20f7efeefae74dd5807728145f2bd0d61ac4e0afe2a8bcecf3852a3f7fb2c12a894b181a3ab77cf9eb442eb81012fea4c74f2ed7d3a152ea20765adc1133a92e394ac36bbf97a4e969b02b5f0adb7d9a0d09b156757f17c2cb92e20bb53479813af7f2cfa9d80ed90e87d12eebe1d0ae3b85b0a6081bff33a81836f5db00ea446efa7df08a6484f0bfb76eda93f2bb487819f4785ad07d939402f88932adfeaccc735300b1ccfe0e0aa092e13c0ea540c85d33da412be87be1245e3fd20d5f95e7f36415b3e0a2dc767c136c68bda83539e7afa421212e3661e5812dcee7b9de24a27e3012824305c6d6776ab5242574a9a283c4885f3504b6ac096005085c69a013415902974acd142ed8402bb10f1a607da09222c5068e922ff20dd8b77ed83029b467273829ce71d874ea33f62d119676adbdbe23d60fbde02c6be2d7cd42c3b095d360343cdbf66b331d72655702fcb9823bc4cce4bb07dedaf30ddb057f13ecb5f4479740b9d0f15a56e585e8790a8222cbd49b9427bd98431a97dd58aa937ebcf6c173d5d6e6e530e89633014214306bc7693ea123cdfaa550989604d82375f5a45c778124845c744bca222dd5c517b414fa9a64d822a692f049aaa688c67a5862605aaa04a34d3affcd82ad4abef10ed32c94cb97567aa106f9320fa556acc08d2018e7554410199409901c75fc4ee114ecab17e12b2683aacc965ae1a7acca0540e6582aa57f3511b509a4248bd651a5e1d4fa677b1a663f5dd49afc2801f3372ac30d31e5e619069736810536732d7aba9505f13bd0ae9f51f0e0b1312ab2ce61a81cba24aad401ea866e0422db5f6fa120f5a759d41576f7d99c886659de241f29958b75041f2a9a741f552d42e0563c2a90680302115c69770ae43ab0b53714d1a5e8f958aabb6d7512956e93a219659c83204cc784ca040568d7495dc964d32b58350790f2150b827b24d1cc5621458640077b2a06357c865ae5a8cefa9d6da49852a2c32a89508cb14c812ef84e358089915b31c0fdcd159aeaa463eafb6fb595985c9a5c54e174a1f3fcb4be72e8376cc0bf6be9675e4a150a7e9f2a2a36c78fbd1c8b485c87a307d0d8493037886b2e10d97164165ab9b6f9e7bb9e6a45a30e1ac821134d41823423bab2f1a062d043380b576b9867387457200835e08ad6d3ffa63130f4c05314c0c63e504c0f8faf648024876d5eb7020251c0184c04b9048140356948d90388f512ec70ba28863c471df4f782ed827a80453381cb7bac19f28c06d2382548cb7ad471a2e3e748a267e08202a6ab411272a16df462b4940c9238a43e8421bb9a06f594641903f9175a341a022d2048d70644a2b7b9f02e7e557675609d51e99c81241f585db8956f1b68f6b54bd637e37ccc33bb662f936cdb9085ad26619604f04c09714021758f37d8a41e7cc9c381008e218c0949f01027ade36ca037909073e18041a3f2bd6f52342ef4ddad550b011d6a0e94ed7c526efc78efa77fcc90e76c3abe37812ab503ac8eab2eddab0a1ea390bffb4a3283a5fa211f066120aea7eaf3fa87d8b40abf34c1e3f1d29bda9f400e27a92b6229aa6efa75dfce0ae12019910fd2a3a5193e8e97b4901481d4f14da95b3ad43c09116cefec4a538a2d30849efb4ced1021abe39d191439de45a08d03d0fa3344cded6dc303713b474ae8150758dc13398737d038f4724c7b8a221eb1eaa4bab09f4cf1cc4a7dc7ca24a4feb7f69e4a138ade030b73fc693c96996fcddeb3ac001c27822309db758fab671c39ef79316ccaf8a594293ec974c8117c80d08b87630780b71b491a0dc56acd609c277472c2b6e2b50aef50e2e0006d4b552f3d53bf3b09f050de0350e199c9e9574ee7376b05ea5b8823aee1700c73dd19780cb41805e3715410164722f262bf94c2918bd4446118574b1962210edb33cfc8123c6da9c45243c143689067b53ec5a9dda8ad3a55eb1070be149ecf8981949df14ef690b7acd2bcc26ed7275bc395d7de07ff9123dba636fd30ddb66e5d7cbd5fb032fbd63c77f2f5816dfb5242e39cd8495b7ba5ba2759ed7c9a7f4ba7a7e2ab5a8ce5227571dfa96af7136511e3ddbe7f1a7689df3e435cb8e88fb5bb43df02c2f77b76cf33a7977c8ef0f396799ed6eb79dab57c523565dfd972ba5cd97efeecb2d24172cf066c69c05f62ef9e5106f374dbb5f455b79b18291285ec7fecaf8f7635fe6fbe282d9b786d2559a100955e26b1ef57e60bbfb2d2796bd4b6ea20242f66dfb3d636fd85db4fec6bf7f8937853d8d1131774457ec972fe2e86e1fedb28a465b9effe518deecbefee9ff016c786879da690200, '6.4.4');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `activitylogs`
--
ALTER TABLE `activitylogs`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `adjustitems`
--
ALTER TABLE `adjustitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AdjustmentId` (`AdjustmentId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_WarehouseId` (`WarehouseId`) USING HASH;

--
-- Indexes for table `adjustments`
--
ALTER TABLE `adjustments`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_LocationId` (`LocationId`) USING HASH;

--
-- Indexes for table `brands`
--
ALTER TABLE `brands`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `locations`
--
ALTER TABLE `locations`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_BrandId` (`BrandId`) USING HASH,
  ADD KEY `IX_CategoryId` (`CategoryId`) USING HASH;

--
-- Indexes for table `productsuppliers`
--
ALTER TABLE `productsuppliers`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_SupplierId` (`SupplierId`) USING HASH;

--
-- Indexes for table `productwarehouses`
--
ALTER TABLE `productwarehouses`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_BrandId` (`BrandId`) USING HASH,
  ADD KEY `IX_CategoryId` (`CategoryId`) USING HASH;

--
-- Indexes for table `purchaseitems`
--
ALTER TABLE `purchaseitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_PurchaseId` (`PurchaseId`) USING HASH,
  ADD KEY `IX_ProductSupId` (`ProductSupId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `purchaseprices`
--
ALTER TABLE `purchaseprices`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `purchasereturnitems`
--
ALTER TABLE `purchasereturnitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReturnId` (`ReturnId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_WarehouseId` (`WarehouseId`) USING HASH;

--
-- Indexes for table `purchasereturns`
--
ALTER TABLE `purchasereturns`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_PurchaseId` (`PurchaseId`) USING HASH,
  ADD KEY `IX_SupplierId` (`SupplierId`) USING HASH;

--
-- Indexes for table `purchases`
--
ALTER TABLE `purchases`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_SupplierId` (`SupplierId`) USING HASH;

--
-- Indexes for table `receiveapproveitems`
--
ALTER TABLE `receiveapproveitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReceiveId` (`ReceiveId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_WarehouseId` (`WarehouseId`) USING HASH;

--
-- Indexes for table `receivingitems`
--
ALTER TABLE `receivingitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReceiveId` (`ReceiveId`) USING HASH,
  ADD KEY `IX_ProductSupId` (`ProductSupId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `receivings`
--
ALTER TABLE `receivings`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_PurchaseId` (`PurchaseId`) USING HASH,
  ADD KEY `IX_SupplierId` (`SupplierId`) USING HASH,
  ADD KEY `IX_LocationId` (`LocationId`) USING HASH;

--
-- Indexes for table `releaseitems`
--
ALTER TABLE `releaseitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReleaseId` (`ReleaseId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `releasereturnitemapproves`
--
ALTER TABLE `releasereturnitemapproves`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReturnId` (`ReturnId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `releasereturnitems`
--
ALTER TABLE `releasereturnitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReturnId` (`ReturnId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH;

--
-- Indexes for table `releasereturns`
--
ALTER TABLE `releasereturns`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ReleaseId` (`ReleaseId`) USING HASH;

--
-- Indexes for table `releasings`
--
ALTER TABLE `releasings`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `suppliers`
--
ALTER TABLE `suppliers`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `transactionlodges`
--
ALTER TABLE `transactionlodges`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `transferdetails`
--
ALTER TABLE `transferdetails`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_LocationId` (`LocationId`) USING HASH;

--
-- Indexes for table `transferitemstores`
--
ALTER TABLE `transferitemstores`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransferId` (`TransferId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_WarehouseId` (`WarehouseId`) USING HASH;

--
-- Indexes for table `transferitemwarehouses`
--
ALTER TABLE `transferitemwarehouses`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_TransferId` (`TransferId`) USING HASH,
  ADD KEY `IX_ProductId` (`ProductId`) USING HASH,
  ADD KEY `IX_WarehouseId` (`WarehouseId`) USING HASH;

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__migrationhistory`
--
ALTER TABLE `__migrationhistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `activitylogs`
--
ALTER TABLE `activitylogs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `adjustitems`
--
ALTER TABLE `adjustitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `adjustments`
--
ALTER TABLE `adjustments`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `brands`
--
ALTER TABLE `brands`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `categories`
--
ALTER TABLE `categories`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `locations`
--
ALTER TABLE `locations`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `productsuppliers`
--
ALTER TABLE `productsuppliers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `productwarehouses`
--
ALTER TABLE `productwarehouses`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchaseitems`
--
ALTER TABLE `purchaseitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchaseprices`
--
ALTER TABLE `purchaseprices`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchasereturnitems`
--
ALTER TABLE `purchasereturnitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchasereturns`
--
ALTER TABLE `purchasereturns`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchases`
--
ALTER TABLE `purchases`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `receiveapproveitems`
--
ALTER TABLE `receiveapproveitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `receivingitems`
--
ALTER TABLE `receivingitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `receivings`
--
ALTER TABLE `receivings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `releaseitems`
--
ALTER TABLE `releaseitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `releasereturnitemapproves`
--
ALTER TABLE `releasereturnitemapproves`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `releasereturnitems`
--
ALTER TABLE `releasereturnitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `releasereturns`
--
ALTER TABLE `releasereturns`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `releasings`
--
ALTER TABLE `releasings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `suppliers`
--
ALTER TABLE `suppliers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `transactionlodges`
--
ALTER TABLE `transactionlodges`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `transferdetails`
--
ALTER TABLE `transferdetails`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `transferitemstores`
--
ALTER TABLE `transferitemstores`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `transferitemwarehouses`
--
ALTER TABLE `transferitemwarehouses`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `adjustitems`
--
ALTER TABLE `adjustitems`
  ADD CONSTRAINT `FK_AdjustItems_Adjustments_AdjustmentId` FOREIGN KEY (`AdjustmentId`) REFERENCES `adjustments` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_AdjustItems_ProductWarehouses_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `productwarehouses` (`Id`),
  ADD CONSTRAINT `FK_AdjustItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`);

--
-- Constraints for table `adjustments`
--
ALTER TABLE `adjustments`
  ADD CONSTRAINT `FK_Adjustments_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `FK_Products_Brands_BrandId` FOREIGN KEY (`BrandId`) REFERENCES `brands` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Products_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `productsuppliers`
--
ALTER TABLE `productsuppliers`
  ADD CONSTRAINT `FK_ProductSuppliers_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ProductSuppliers_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `productwarehouses`
--
ALTER TABLE `productwarehouses`
  ADD CONSTRAINT `FK_ProductWarehouses_Brands_BrandId` FOREIGN KEY (`BrandId`) REFERENCES `brands` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ProductWarehouses_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `purchaseitems`
--
ALTER TABLE `purchaseitems`
  ADD CONSTRAINT `FK_PurchaseItems_ProductSuppliers_ProductSupId` FOREIGN KEY (`ProductSupId`) REFERENCES `productsuppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_PurchaseItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_PurchaseItems_Purchases_PurchaseId` FOREIGN KEY (`PurchaseId`) REFERENCES `purchases` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `purchaseprices`
--
ALTER TABLE `purchaseprices`
  ADD CONSTRAINT `FK_PurchasePrices_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `purchasereturnitems`
--
ALTER TABLE `purchasereturnitems`
  ADD CONSTRAINT `FK_PurchaseReturnItems_ProductWarehouses_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `productwarehouses` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_PurchaseReturnItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_PurchaseReturnItems_PurchaseReturns_ReturnId` FOREIGN KEY (`ReturnId`) REFERENCES `purchasereturns` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `purchasereturns`
--
ALTER TABLE `purchasereturns`
  ADD CONSTRAINT `FK_PurchaseReturns_Purchases_PurchaseId` FOREIGN KEY (`PurchaseId`) REFERENCES `purchases` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_PurchaseReturns_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `purchases`
--
ALTER TABLE `purchases`
  ADD CONSTRAINT `FK_Purchases_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `receiveapproveitems`
--
ALTER TABLE `receiveapproveitems`
  ADD CONSTRAINT `FK_ReceiveApproveItems_ProductWarehouses_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `productwarehouses` (`Id`),
  ADD CONSTRAINT `FK_ReceiveApproveItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`),
  ADD CONSTRAINT `FK_ReceiveApproveItems_Receivings_ReceiveId` FOREIGN KEY (`ReceiveId`) REFERENCES `receivings` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `receivingitems`
--
ALTER TABLE `receivingitems`
  ADD CONSTRAINT `FK_ReceivingItems_ProductSuppliers_ProductSupId` FOREIGN KEY (`ProductSupId`) REFERENCES `productsuppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ReceivingItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`),
  ADD CONSTRAINT `FK_ReceivingItems_Receivings_ReceiveId` FOREIGN KEY (`ReceiveId`) REFERENCES `receivings` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `receivings`
--
ALTER TABLE `receivings`
  ADD CONSTRAINT `FK_Receivings_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Receivings_Purchases_PurchaseId` FOREIGN KEY (`PurchaseId`) REFERENCES `purchases` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Receivings_Suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `releaseitems`
--
ALTER TABLE `releaseitems`
  ADD CONSTRAINT `FK_ReleaseItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ReleaseItems_Releasings_ReleaseId` FOREIGN KEY (`ReleaseId`) REFERENCES `releasings` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `releasereturnitemapproves`
--
ALTER TABLE `releasereturnitemapproves`
  ADD CONSTRAINT `FK_ReleaseReturnItemApproves_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ReleaseReturnItemApproves_ReleaseReturns_ReturnId` FOREIGN KEY (`ReturnId`) REFERENCES `releasereturns` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `releasereturnitems`
--
ALTER TABLE `releasereturnitems`
  ADD CONSTRAINT `FK_ReleaseReturnItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ReleaseReturnItems_ReleaseReturns_ReturnId` FOREIGN KEY (`ReturnId`) REFERENCES `releasereturns` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `releasereturns`
--
ALTER TABLE `releasereturns`
  ADD CONSTRAINT `FK_ReleaseReturns_Releasings_ReleaseId` FOREIGN KEY (`ReleaseId`) REFERENCES `releasings` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `transferdetails`
--
ALTER TABLE `transferdetails`
  ADD CONSTRAINT `FK_TransferDetails_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `locations` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `transferitemstores`
--
ALTER TABLE `transferitemstores`
  ADD CONSTRAINT `FK_TransferItemStores_ProductWarehouses_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `productwarehouses` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TransferItemStores_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TransferItemStores_TransferDetails_TransferId` FOREIGN KEY (`TransferId`) REFERENCES `transferdetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `transferitemwarehouses`
--
ALTER TABLE `transferitemwarehouses`
  ADD CONSTRAINT `FK_TransferItemWarehouses_ProductWarehouses_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `productwarehouses` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TransferItemWarehouses_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_TransferItemWarehouses_TransferDetails_TransferId` FOREIGN KEY (`TransferId`) REFERENCES `transferdetails` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
