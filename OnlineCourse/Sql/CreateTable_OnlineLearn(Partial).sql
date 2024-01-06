USE OnlineCourseVer1
GO 

--DROP TABLE [CourseDocument];
--DROP TABLE [CourseVideo];
--DROP TABLE [RecommenedCourse];
--DROP TABLE [RelatedCourse];
--DROP TABLE [Admin];
--DROP TABLE [Author];
--DROP TABLE [Course_Author];
--DROP TABLE [VideoExam];
--DROP TABLE [ExamQuestion];
--DROP TABLE [Exam_Question];
--DROP TABLE [QuestionAnswer];

CREATE TABLE [dbo].[CourseDocument](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY ,
	[productID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Link] [varchar](200) NULL,
	[Title] [nvarchar](max) NULL,
	[DateUpdate] [datetime] NULL,
	)

CREATE TABLE [dbo].[CourseVideo](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[productID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Link]  [varchar](200) NULL,
	[Title] [nvarchar](max) NULL,
	[DateUpdate] [datetime] NULL,
	)

CREATE TABLE [dbo].[RecommenedCourse](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[productID] [int] NULL,
	[Decription] [nvarchar](max) NULL,
	[DateUpdate] [datetime] NULL,
	)

CREATE TABLE [dbo].[RelatedCourse](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[productID] [int] NULL,
	[Decription] [nvarchar](max) NULL,
	[DateUpdate] [datetime] NULL,
	)

CREATE TABLE [dbo].[Admin](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[userID] [int] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](32)NOT NULL,
	)

CREATE TABLE [dbo].[Course_Author](
	[ProductID] [int],
	[AuthorID] [int],
	[Description] [nvarchar](50),

	PRIMARY KEY(ProductID, AuthorID)
	)


CREATE TABLE [dbo].[Author](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](max) NULL,
	[Decription] [nvarchar](max) NULL,
	[DateUpdate] [datetime] NULL,
	)


CREATE TABLE [dbo].[VideoExam](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[VideoID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Time] [SMALLINT] NOT NULL,
	[TYPE] [varchar](20),
	)

CREATE TABLE [dbo].[Exam_Question](
	[QuestionID] [int],
	[ExamID] [int],
	[Description] [nvarchar](50),

	PRIMARY KEY(ExamID, QuestionID)
	)

CREATE TABLE [dbo].[ExamQuestion](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ProductID] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[TYPE] [varchar](20),
	[Score] [int],
	)

CREATE TABLE [dbo].[QuestionAnswer](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[QuestionID] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[IsTrueAnswer] [BIT] NOT NULL,
	)
