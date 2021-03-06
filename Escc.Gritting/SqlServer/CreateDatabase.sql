/****** Object:  Table [dbo].[Gritter]    Script Date: 08/02/2016 11:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Gritter](
	[GritterId] [varchar](50) NOT NULL,
	[GritterName] [varchar](100) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[Status] [int] NOT NULL,
	[DateUpdated] [datetime] NOT NULL CONSTRAINT [DF_Gritter_DateUpdated]  DEFAULT (getdate()),
 CONSTRAINT [PK_Gritter] PRIMARY KEY CLUSTERED 
(
	[GritterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_Gritter_Save]    Script Date: 08/02/2016 11:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Web Team
-- Create date: 31 October 2013
-- Description:	Add or update data for a gritter
-- =============================================
CREATE PROCEDURE [dbo].[usp_Gritter_Save]
	@GritterId varchar(50),
	@GritterName varchar(100),
	@Latitude decimal(9,6),
	@Longitude decimal(9,6),
	@Status int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Check if gritter has already been added to the database
	DECLARE @existingGritter varchar(50)
	SELECT @existingGritter = GritterId FROM Gritter WHERE GritterId = @GritterId

	-- Insert or update depending on whether an existing record was found
	IF @existingGritter IS NULL
		INSERT INTO Gritter (GritterId, GritterName, Latitude, Longitude, [Status])
		VALUES (@GritterId, @GritterName, @Latitude, @Longitude, @Status)
	ELSE
		UPDATE Gritter SET
		GritterName = @GritterName, 
		Latitude = @Latitude,
		Longitude = @Longitude,
		[Status] = @Status,
		DateUpdated = GETDATE()
		WHERE GritterId = @GritterId
END


GO
/****** Object:  StoredProcedure [dbo].[usp_Gritter_SelectAll]    Script Date: 08/02/2016 11:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Web Team
-- Create date: 31 Oct 2013
-- Description:	Read data on all gritters
-- =============================================
CREATE PROCEDURE [dbo].[usp_Gritter_SelectAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT GritterId, GritterName, Latitude, Longitude, [Status] 
	FROM Gritter
	WHERE DateUpdated >= DATEADD(DAY,-1,GETDATE())
END
GO
