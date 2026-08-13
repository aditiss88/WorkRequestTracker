CREATE DATABASE WorkRequestTracker_AS;
GO

USE WorkRequestTracker_AS;
GO

-- Now create your table, constraints, indexes, and seed data

CREATE DATABASE WorkRequestTracker_AS;
GO

USE WorkRequestTracker_AS;
GO

-- ============================================
-- WorkRequestTracker Table Schema
-- ============================================

IF OBJECT_ID('dbo.WorkRequests', 'U') IS NOT NULL
    DROP TABLE dbo.WorkRequests;
GO

CREATE TABLE dbo.WorkRequests (
    Id              INT             IDENTITY(1,1)   PRIMARY KEY,
    Title           NVARCHAR(200)   NOT NULL,
    ClientName      NVARCHAR(150)   NOT NULL,
    Description     NVARCHAR(MAX)   NULL,
    Priority        NVARCHAR(20)    NOT NULL,   -- 'Low' | 'Medium' | 'High'
    Status          NVARCHAR(20)    NOT NULL,   -- 'New' | 'InProgress' | 'Blocked' | 'Completed'
    DueDate         DATETIME2       NOT NULL,
    CreatedDate     DATETIME2       NOT NULL    DEFAULT (GETUTCDATE()),
    UpdatedDate     DATETIME2       NOT NULL    DEFAULT (GETUTCDATE()),
    Notes           NVARCHAR(MAX)   NULL,

    CONSTRAINT CHK_WorkRequests_Priority CHECK (Priority IN ('Low', 'Medium', 'High')),
    CONSTRAINT CHK_WorkRequests_Status CHECK (Status IN ('New', 'InProgress', 'Blocked', 'Completed'))
);
GO

-- ============================================
-- Indexes
-- ============================================

CREATE INDEX IX_WorkRequests_Status ON dbo.WorkRequests (Status);
CREATE INDEX IX_WorkRequests_ClientName ON dbo.WorkRequests (ClientName);
CREATE INDEX IX_WorkRequests_Title ON dbo.WorkRequests (Title);
GO

-- ============================================
-- Seed Data
-- ============================================

INSERT INTO dbo.WorkRequests (Title, ClientName, Description, Priority, Status, DueDate)
VALUES
('Fix login page bug', 'Acme Corp', 'Users unable to log in on mobile Safari.', 'High', 'New', '2026-08-20'),
('Update pricing page', 'Globex Inc', 'Add new enterprise tier pricing.', 'Medium', 'InProgress', '2026-08-25');
GO

-- Verify
SELECT * FROM dbo.WorkRequests;