-- create database newEwallet;
-- GO

-- *Show tables in db
SELECT 
    SCHEMA_NAME(schema_id) AS SchemaName,
    name AS TableName,
    create_date,
    modify_date
FROM 
    sys.tables
ORDER BY 
    TableName;
GO



/* ---------- 1. Users (10 user, semua ACTIVE, PasswordHash NULL) ---------- */
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (Id, Username, FullName, Email, Phone, PasswordHash, Status, CreatedAtUtc, UpdatedAtUtc) VALUES
(1,  N'andi.pratama',    N'Andi Pratama',    N'andi.pratama@example.com',    N'081200000001', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(2,  N'budi.santoso',    N'Budi Santoso',    N'budi.santoso@example.com',    N'081200000002', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(3,  N'citra.lestari',   N'Citra Lestari',   N'citra.lestari@example.com',   N'081200000003', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(4,  N'dewi.anggraini',  N'Dewi Anggraini',  N'dewi.anggraini@example.com',  N'081200000004', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(5,  N'eko.wijaya',      N'Eko Wijaya',      N'eko.wijaya@example.com',      N'081200000005', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(6,  N'fitri.handayani', N'Fitri Handayani', N'fitri.handayani@example.com', N'081200000006', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(7,  N'galih.permana',   N'Galih Permana',   N'galih.permana@example.com',   N'081200000007', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(8,  N'hana.putri',      N'Hana Putri',      N'hana.putri@example.com',      N'081200000008', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(9,  N'indra.kusuma',    N'Indra Kusuma',    N'indra.kusuma@example.com',    N'081200000009', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(10, N'joko.susilo',     N'Joko Susilo',     N'joko.susilo@example.com',     N'081200000010', NULL, 'ACTIVE', '2026-11-01T07:00:00', NULL);
SET IDENTITY_INSERT dbo.Users OFF;

---------- 2. Wallets (1 user = 1 wallet) ----------   
SET IDENTITY_INSERT dbo.Wallets ON;
INSERT INTO dbo.Wallets (Id, UserId, WalletNumber, Balance, Status, CreatedAtUtc, UpdatedAtUtc) VALUES
(1,  1,  N'EW0000000001', 650000.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(2,  2,  N'EW0000000002', 350000.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(3,  3,  N'EW0000000003', 750000.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(4,  4,  N'EW0000000004', 175000.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(5,  5,  N'EW0000000005',  75000.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(6,  6,  N'EW0000000006',      0.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(7,  7,  N'EW0000000007',      0.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(8,  8,  N'EW0000000008',      0.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(9,  9,  N'EW0000000009',      0.00, 'ACTIVE', '2026-11-01T07:00:00', NULL),
(10, 10, N'EW0000000010',      0.00, 'ACTIVE', '2026-11-01T07:00:00', NULL);
SET IDENTITY_INSERT dbo.Wallets OFF;

---------- 3. PaymentMethods  ---------- 
SET IDENTITY_INSERT dbo.PaymentMethods ON;
INSERT INTO dbo.PaymentMethods (Id, Code, Name, ProviderName, IsActive, CreatedAtUtc) VALUES
(1, N'BANK_TRANSFER',   N'Bank Transfer',   N'Simulated Bank', 1, '2026-11-01T07:00:00'),
(2, N'QRIS',            N'QRIS',            N'Simulated QRIS', 1, '2026-11-01T07:00:00'),
(3, N'VIRTUAL_ACCOUNT', N'Virtual Account', N'Simulated VA',   0, '2026-11-01T07:00:00');
SET IDENTITY_INSERT dbo.PaymentMethods OFF;

---------- 4. Transactions ----------
SET IDENTITY_INSERT dbo.Transactions ON;
INSERT INTO dbo.Transactions (Id, ReferenceNo, SenderWalletId, ReceiverWalletId, Type, Amount, Status, Description, CreatedAtUtc, CompletedAtUtc) VALUES
(1,  N'TOPUP-20261101-0001', NULL, 1, 'TOPUP',    500000.00, 'SUCCESS', N'Top up via Bank Transfer', '2026-11-01T08:00:00', '2026-11-01T08:05:00'),
(2,  N'TOPUP-20261101-0002', NULL, 2, 'TOPUP',    300000.00, 'SUCCESS', N'Top up via QRIS',          '2026-11-01T08:30:00', '2026-11-01T08:35:00'),
(3,  N'TOPUP-20261101-0003', NULL, 3, 'TOPUP',   1000000.00, 'SUCCESS', N'Top up via Bank Transfer', '2026-11-01T09:00:00', '2026-11-01T09:05:00'),
(4,  N'TOPUP-20261101-0004', NULL, 4, 'TOPUP',    200000.00, 'SUCCESS', N'Top up via QRIS',          '2026-11-01T09:30:00', '2026-11-01T09:35:00'),
(5,  N'TOPUP-20261101-0005', NULL, 5, 'TOPUP',    150000.00, 'PENDING', N'Top up via Bank Transfer', '2026-11-01T10:00:00', NULL),
(6,  N'TOPUP-20261101-0006', NULL, 6, 'TOPUP',    250000.00, 'FAILED',  N'Top up via QRIS',          '2026-11-01T10:30:00', '2026-11-01T11:00:00'),
(7,  N'TRF-20261102-0001',   1,    2, 'TRANSFER', 100000.00, 'SUCCESS', N'Transfer Andi ke Budi',    '2026-11-02T08:00:00', '2026-11-02T08:00:00'),
(8,  N'TRF-20261102-0002',   3,    1, 'TRANSFER', 250000.00, 'SUCCESS', N'Transfer Citra ke Andi',   '2026-11-02T09:00:00', '2026-11-02T09:00:00'),
(9,  N'TRF-20261102-0003',   2,    4, 'TRANSFER',  50000.00, 'SUCCESS', N'Transfer Budi ke Dewi',    '2026-11-02T10:00:00', '2026-11-02T10:00:00'),
(10, N'TRF-20261103-0001',   4,    5, 'TRANSFER',  75000.00, 'SUCCESS', N'Transfer Dewi ke Eko',     '2026-11-03T08:00:00', '2026-11-03T08:00:00');
SET IDENTITY_INSERT dbo.Transactions OFF;

---------- 5. WalletMutations (hanya untuk transaksi SUCCESS) ----------
SET IDENTITY_INSERT dbo.WalletMutations ON;
INSERT INTO dbo.WalletMutations (Id, TransactionId, WalletId, Direction, Amount, BalanceBefore, BalanceAfter, CreatedAtUtc) VALUES
(1,  1,  1, 'IN',   500000.00,       0.00,  500000.00, '2026-11-01T08:05:00'),
(2,  2,  2, 'IN',   300000.00,       0.00,  300000.00, '2026-11-01T08:35:00'),
(3,  3,  3, 'IN',  1000000.00,       0.00, 1000000.00, '2026-11-01T09:05:00'),
(4,  4,  4, 'IN',   200000.00,       0.00,  200000.00, '2026-11-01T09:35:00'),
(5,  7,  1, 'OUT',  100000.00,  500000.00,  400000.00, '2026-11-02T08:00:00'),
(6,  7,  2, 'IN',   100000.00,  300000.00,  400000.00, '2026-11-02T08:00:00'),
(7,  8,  3, 'OUT',  250000.00, 1000000.00,  750000.00, '2026-11-02T09:00:00'),
(8,  8,  1, 'IN',   250000.00,  400000.00,  650000.00, '2026-11-02T09:00:00'),
(9,  9,  2, 'OUT',   50000.00,  400000.00,  350000.00, '2026-11-02T10:00:00'),
(10, 9,  4, 'IN',    50000.00,  200000.00,  250000.00, '2026-11-02T10:00:00'),
(11, 10, 4, 'OUT',   75000.00,  250000.00,  175000.00, '2026-11-03T08:00:00'),
(12, 10, 5, 'IN',    75000.00,       0.00,   75000.00, '2026-11-03T08:00:00');
SET IDENTITY_INSERT dbo.WalletMutations OFF;

---------- 6. TopUpRequests (1 per transaksi TOPUP)
SET IDENTITY_INSERT dbo.TopUpRequests ON;
INSERT INTO dbo.TopUpRequests (Id, TransactionId, WalletId, PaymentMethodId, Amount, Status, ProviderReference, CreatedAtUtc, ExpiresAtUtc, PaidAtUtc) VALUES
(1, 1, 1, 1,  500000.00, 'PAID',    N'SIM-BANK-0001', '2026-11-01T08:00:00', '2026-11-02T08:00:00', '2026-11-01T08:05:00'),
(2, 2, 2, 2,  300000.00, 'PAID',    N'SIM-QRIS-0002', '2026-11-01T08:30:00', '2026-11-02T08:30:00', '2026-11-01T08:35:00'),
(3, 3, 3, 1, 1000000.00, 'PAID',    N'SIM-BANK-0003', '2026-11-01T09:00:00', '2026-11-02T09:00:00', '2026-11-01T09:05:00'),
(4, 4, 4, 2,  200000.00, 'PAID',    N'SIM-QRIS-0004', '2026-11-01T09:30:00', '2026-11-02T09:30:00', '2026-11-01T09:35:00'),
(5, 5, 5, 1,  150000.00, 'PENDING', N'SIM-BANK-0005', '2026-11-01T10:00:00', '2026-12-31T10:00:00', NULL),
(6, 6, 6, 2,  250000.00, 'FAILED',  N'SIM-QRIS-0006', '2026-11-01T10:30:00', '2026-11-02T10:30:00', NULL);
SET IDENTITY_INSERT dbo.TopUpRequests OFF;

 -------- 7. AuditLogs ----------
SET IDENTITY_INSERT dbo.AuditLogs ON;
INSERT INTO dbo.AuditLogs (Id, ActorUserId, TransactionId, Action, EntityType, EntityId, Outcome, Description, CorrelationId, CreatedAtUtc) VALUES
(1,  1,    1,    N'TOPUP_REQUESTED',  N'Transaction', N'1',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T08:00:00'),
(2,  2,    2,    N'TOPUP_REQUESTED',  N'Transaction', N'2',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T08:30:00'),
(3,  3,    3,    N'TOPUP_REQUESTED',  N'Transaction', N'3',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T09:00:00'),
(4,  4,    4,    N'TOPUP_REQUESTED',  N'Transaction', N'4',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T09:30:00'),
(5,  5,    5,    N'TOPUP_REQUESTED',  N'Transaction', N'5',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T10:00:00'),
(6,  6,    6,    N'TOPUP_REQUESTED',  N'Transaction', N'6',  'SUCCESS',  N'Permintaan top up dibuat',        NULL, '2026-11-01T10:30:00'),
(7,  NULL, 1,    N'TOPUP_PAID',       N'Transaction', N'1',  'SUCCESS',  N'Pembayaran simulasi dikonfirmasi', NULL, '2026-11-01T08:05:00'),
(8,  NULL, 2,    N'TOPUP_PAID',       N'Transaction', N'2',  'SUCCESS',  N'Pembayaran simulasi dikonfirmasi', NULL, '2026-11-01T08:35:00'),
(9,  NULL, 3,    N'TOPUP_PAID',       N'Transaction', N'3',  'SUCCESS',  N'Pembayaran simulasi dikonfirmasi', NULL, '2026-11-01T09:05:00'),
(10, NULL, 4,    N'TOPUP_PAID',       N'Transaction', N'4',  'SUCCESS',  N'Pembayaran simulasi dikonfirmasi', NULL, '2026-11-01T09:35:00'),
(11, 1,    7,    N'TRANSFER_CREATED', N'Transaction', N'7',  'SUCCESS',  N'Transfer berhasil',               NULL, '2026-11-02T08:00:00'),
(12, 3,    8,    N'TRANSFER_CREATED', N'Transaction', N'8',  'SUCCESS',  N'Transfer berhasil',               NULL, '2026-11-02T09:00:00'),
(13, 2,    9,    N'TRANSFER_CREATED', N'Transaction', N'9',  'SUCCESS',  N'Transfer berhasil',               NULL, '2026-11-02T10:00:00'),
(14, 4,    10,   N'TRANSFER_CREATED', N'Transaction', N'10', 'SUCCESS',  N'Transfer berhasil',               NULL, '2026-11-03T08:00:00'),
(15, 5,    NULL, N'TRANSFER_REJECTED',N'Wallet',      N'5',  'REJECTED', N'Saldo tidak mencukupi',           NULL, '2026-11-03T09:00:00');
SET IDENTITY_INSERT dbo.AuditLogs OFF;


Select * FROM dbo.AuditLogs;
Select * FROM dbo.TopUpRequests;
Select * FROM dbo.WalletMutations; 
Select * FROM dbo.Transactions;
Select * FROM dbo.PaymentMethods;
Select * FROM dbo.Wallets;
Select * FROM dbo.Users; 