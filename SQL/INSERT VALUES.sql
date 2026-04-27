use MusicGlue
INSERT INTO REPORTINGORGANISATION (Name, Country)
VALUES
('OCC', 'England'),
('ARIA', 'Australia');

INSERT INTO CONSIGNMENT (CustomerCountry, ZipCode, ConsignmentStatus, ReportingStatus)
VALUES
('England', 'WC', 1, 0),
('Australia', '11455', 2, 0),
('England', 'N', 0, 0),
('Australia', '33100', 0, 0),
('England', 'YO', 2, 0);

INSERT INTO PRODUCTDESCRIPTION (APN, CatalogNumber, SKU, Barcode)
VALUES
('APN-001', 'CAT-1001', 'SKU-5001', '1234567891'),
('APN-002', 'CAT-1002', 'SKU-5002', '1234567892'),
('APN-003', 'CAT-1003', 'SKU-5003', '1234567893'),
('APN-004', 'CAT-1004', 'SKU-5004', '1234567894'),
('APN-005', 'CAT-1005', 'SKU-5005', '1234567895');

INSERT INTO MUSICPRODUCT (Price, ProductDescriptionId)
VALUES
(111.95, 1),
(8.50, 2),
(2.75, 3),
(5.00, 4),
(17.00, 5);

INSERT INTO MUSICPRODUCT_CONSIGNMENT (ProductId, ConsignmentId)
VALUES
(1, 1),
(2, 1),
(3, 2),
(5, 3),
(4, 4),
(1, 5),
(3, 5);