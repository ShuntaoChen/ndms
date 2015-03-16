
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/12/2015 14:16:28
-- Generated from EDMX file: d:\软件relatated\JYCAM(JSKW)\Web\Areas\JobArrangements\Models\JA.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [JY_NDMS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[JobArrangements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobArrangements];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'JobArrangements'
CREATE TABLE [dbo].[JobArrangements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(100)  NULL,
    [Category] varchar(20)  NULL,
    [Content] nvarchar(255)  NULL,
    [CreatorNo] int  NULL,
    [DoerNoList] varchar(100)  NULL,
    [SupervisorNo] int  NULL,
    [AppointStatus] int  NULL,
    [Remark] nvarchar(255)  NULL,
    [Starttime] datetime  NULL,
    [Endtime] datetime  NULL,
    [LoopCategory] varchar(20)  NULL,
    [CreatorId] int  NULL,
    [CreateOn] datetime  NULL,
    [UpdaterId] int  NULL,
    [LastUpdateOn] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'JobArrangements'
ALTER TABLE [dbo].[JobArrangements]
ADD CONSTRAINT [PK_JobArrangements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------