
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contact];

CREATE TABLE [dbo].[Contact](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[AccountName] [nvarchar](100) NULL,
	[VendorName] [nvarchar](100) NULL,
	[Email] [nvarchar](max) NULL,
	[Title] [nvarchar](250) NULL,
	[Phone] [nvarchar](11) NULL,
	[Department] [nvarchar](100) NULL,
	[HomePhone] [nvarchar](11) NULL,
	[HomePhone2] [nvarchar](11) NULL,
	[OtherContect] [nvarchar](11) NULL,
	[Mobile] [nvarchar](11) NULL,
	[Fax] [nvarchar](11) NULL,
	[Assistant] [nvarchar](50) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[BelongTo] [nvarchar](100) NULL,
	[AssistPhone] [nvarchar](11) NULL,
	[EmailOpt] [nvarchar](max) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[MailingStreet] [nvarchar](50) NULL,
	[MailingCity] [nvarchar](50) NULL,
	[MailingState] [nvarchar](50) NULL,
	[MailingCountry] [nvarchar](50) NULL,
	[MailingZip] [varchar](50) NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


