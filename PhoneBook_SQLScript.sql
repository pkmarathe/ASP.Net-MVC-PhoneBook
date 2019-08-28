GO
Create database PhoneBook;
USE [PhoneBook]
GO

CREATE TABLE [dbo].[People](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Phone] [varchar](12) NOT NULL,
	[Email] [varchar](50) NULL,
 CONSTRAINT [PK_PhoneBook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
GO

Create procedure [dbo].[GetPhoneBookList]
@filter varchar(50) null AS
BEGIN
IF @filter IS NOT NULL
SELECT * FROM dbo.People as p WHERE
		 p.FirstName like '%' +@filter+ '%' OR
		 p.LastName like '%' +@filter+ '%' OR
		  p.Phone like '%' +@filter+ '%' OR
		  Email like '%' +@filter+ '%' ;
ELSE
SELECT * FROM dbo.People;
END

GO
