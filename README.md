# Mobile-Recharge
Building Mobile recharge system using ASP,Net MVC

Create DB and Table
use  OrgMobileRecharge;

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    MobileNumber NVARCHAR(15) NOT NULL UNIQUE,
    Email NVARCHAR(100) NULL,
    WalletBalance DECIMAL(12,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE()
);

----------------------------------------------------------------

CREATE TABLE Operators
(
    OperatorId INT IDENTITY(1,1) PRIMARY KEY,
    OperatorName NVARCHAR(50) NOT NULL,
    OperatorType NVARCHAR(20) NOT NULL, -- Prepaid / Postpaid / DTH
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE()
);

---------------------------------------------------------


CREATE TABLE RechargeStatus
(
    StatusId INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(20) NOT NULL -- PENDING, SUCCESS, FAILED
);

---------------------------------------------------------------------

INSERT INTO RechargeStatus (StatusName)
VALUES ('PENDING'), ('SUCCESS'), ('FAILED');

---------------------------------------------------------------------


CREATE TABLE Recharges
(
    RechargeId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    OperatorId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    StatusId INT NOT NULL,
    RechargeDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Recharge_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Recharge_Operator FOREIGN KEY (OperatorId) REFERENCES Operators(OperatorId),
    CONSTRAINT FK_Recharge_Status FOREIGN KEY (StatusId) REFERENCES RechargeStatus(StatusId)
);

-------------------------------------------------------------------------------------------
USE OrgMobileRecharge;

CREATE TABLE RechargePlans
(
    PlanId INT IDENTITY(1,1) PRIMARY KEY,
    OperatorId INT NOT NULL,
    PlanName NVARCHAR(100),
    Amount DECIMAL(10,2) NOT NULL,
    ValidityDays INT NOT NULL,
    Description NVARCHAR(200),
    IsActive BIT DEFAULT 1,

    CONSTRAINT FK_Plan_Operator
        FOREIGN KEY (OperatorId) REFERENCES Operators(OperatorId)
);

-------------------------------------------------------------

-- JIO PLANS
INSERT INTO RechargePlans (OperatorId, PlanName, Amount, ValidityDays, Description)
VALUES
(1, 'Jio 239', 239, 28, '1.5GB/day + Unlimited Calls'),
(1, 'Jio 299', 299, 28, '2GB/day + Unlimited Calls'),
(1, 'Jio 666', 666, 84, '1.5GB/day + Unlimited Calls');

-- AIRTEL PLANS
INSERT INTO RechargePlans (OperatorId, PlanName, Amount, ValidityDays, Description)
VALUES
(2, 'Airtel 265', 265, 28, '1GB/day + Unlimited Calls'),
(2, 'Airtel 299', 299, 28, '1.5GB/day + Unlimited Calls'),
(2, 'Airtel 719', 719, 84, '1.5GB/day + Unlimited Calls');

INSERT INTO RechargePlans (OperatorId, PlanName, Amount, ValidityDays, Description)
VALUES
(3, 'VI 265', 299, 28, '1GB/day + Unlimited Calls'),
(3, 'VI 299', 339, 28, '1.5GB/day + Unlimited Calls'),
(3, 'VI 719', 799, 84, '1.5GB/day + Unlimited Calls');

select * from RechargePlans;

------------------------------------------------------------------------------------------

USE OrgMobileRecharge;

INSERT INTO Operators (OperatorName, OperatorType)
VALUES
('Jio', 'Prepaid'),
('Airtel', 'Prepaid'),
('VI', 'Prepaid');

----------------------------------------
SELECT OperatorId, OperatorName FROM Operators;

-------------------------------------------------------------------------------------------

CREATE TABLE WalletTransactions
(
    WalletTxnId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    RechargeId INT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    TransactionType NVARCHAR(20) NOT NULL, -- DEBIT / CREDIT
    BalanceAfterTxn DECIMAL(12,2) NOT NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Wallet_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Wallet_Recharge FOREIGN KEY (RechargeId) REFERENCES Recharges(RechargeId)
);

--------------------------------------------------------------------------------------------

Create Stored Procedure

use OrgMobileRecharge;

CREATE PROCEDURE sp_AddUser
(
    @FullName NVARCHAR(100),
    @MobileNumber NVARCHAR(15),
    @Email NVARCHAR(100)
)
AS
BEGIN
    INSERT INTO Users (FullName, MobileNumber, Email)
    VALUES (@FullName, @MobileNumber, @Email);
END

----------------------------------------------------------

CREATE PROCEDURE sp_AddWalletBalance
(
    @UserId INT,
    @Amount DECIMAL(10,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewBalance DECIMAL(12,2);

    UPDATE Users
    SET WalletBalance = WalletBalance + @Amount
    WHERE UserId = @UserId;

    SELECT @NewBalance = WalletBalance FROM Users WHERE UserId = @UserId;

    INSERT INTO WalletTransactions
    (UserId, Amount, TransactionType, BalanceAfterTxn)
    VALUES
    (@UserId, @Amount, 'CREDIT', @NewBalance);
END

----------------------------------------------------------------


CREATE PROCEDURE sp_CreateRecharge
(
    @UserId INT,
    @OperatorId INT,
    @Amount DECIMAL(10,2)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @WalletBalance DECIMAL(12,2);
        DECLARE @RechargeId INT;
        DECLARE @StatusPending INT = 1;
        DECLARE @StatusSuccess INT = 2;

        SELECT @WalletBalance = WalletBalance
        FROM Users
        WHERE UserId = @UserId AND IsActive = 1;

        IF (@WalletBalance < @Amount)
        BEGIN
            RAISERROR ('Insufficient wallet balance', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Create Recharge (Pending)
        INSERT INTO Recharges (UserId, OperatorId, Amount, StatusId)
        VALUES (@UserId, @OperatorId, @Amount, @StatusPending);

        SET @RechargeId = SCOPE_IDENTITY();

        -- Deduct Wallet
        UPDATE Users
        SET WalletBalance = WalletBalance - @Amount
        WHERE UserId = @UserId;

        SELECT @WalletBalance = WalletBalance FROM Users WHERE UserId = @UserId;

        INSERT INTO WalletTransactions
        (UserId, RechargeId, Amount, TransactionType, BalanceAfterTxn)
        VALUES
        (@UserId, @RechargeId, @Amount, 'DEBIT', @WalletBalance);

        -- Mark Recharge Success
        UPDATE Recharges
        SET StatusId = @StatusSuccess
        WHERE RechargeId = @RechargeId;

        COMMIT TRANSACTION;

        SELECT @RechargeId AS RechargeId, 'SUCCESS' AS Status;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END


--------------------------------------------------------------------------


CREATE PROCEDURE sp_GetRechargeHistory
(
    @UserId INT
)
AS
BEGIN
    SELECT 
        r.RechargeId,
        o.OperatorName,
        r.Amount,
        rs.StatusName,
        r.RechargeDate
    FROM Recharges r
    INNER JOIN Operators o ON r.OperatorId = o.OperatorId
    INNER JOIN RechargeStatus rs ON r.StatusId = rs.StatusId
    WHERE r.UserId = @UserId
    ORDER BY r.RechargeDate DESC;
END

---------------------------------------------------------------

USE OrgMobileRecharge;
GO

ALTER PROCEDURE sp_CreateRecharge
(
    @UserId INT,
    @OperatorId INT,
    @Amount DECIMAL(10,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @WalletBalance DECIMAL(12,2);
        DECLARE @RechargeId INT;
        DECLARE @StatusPending INT = 1;  -- PENDING
        DECLARE @StatusSuccess INT = 2;  -- SUCCESS

        -- Get wallet balance
        SELECT @WalletBalance = WalletBalance
        FROM Users
        WHERE UserId = @UserId AND IsActive = 1;

        -- Validate balance
        IF (@WalletBalance IS NULL OR @WalletBalance < @Amount)
        BEGIN
            RAISERROR ('Insufficient wallet balance', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insert Recharge (PENDING)
        INSERT INTO Recharges (UserId, OperatorId, Amount, StatusId)
        VALUES (@UserId, @OperatorId, @Amount, @StatusPending);

        SET @RechargeId = SCOPE_IDENTITY();

        -- Deduct Wallet Balance
        UPDATE Users
        SET WalletBalance = WalletBalance - @Amount
        WHERE UserId = @UserId;

        -- Get updated balance
        SELECT @WalletBalance = WalletBalance
        FROM Users
        WHERE UserId = @UserId;

        -- Insert Wallet Transaction (DEBIT)
        INSERT INTO WalletTransactions
        (
            UserId,
            RechargeId,
            Amount,
            TransactionType,
            BalanceAfterTxn
        )
        VALUES
        (
            @UserId,
            @RechargeId,
            @Amount,
            'DEBIT',
            @WalletBalance
        );

        -- Update Recharge Status to SUCCESS
        UPDATE Recharges
        SET StatusId = @StatusSuccess
        WHERE RechargeId = @RechargeId;

        COMMIT TRANSACTION;

        -- ✅ IMPORTANT FIX: return ONLY RechargeId
        SELECT @RechargeId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO


-------------------------------------------------------------




