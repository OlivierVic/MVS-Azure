-- =============================================
-- Author:      <Olivier Vic - Droits Quotidiens Legal Tech>
-- Create Date: <20 July 2023>
-- Description: <Stored Procedures for delete User>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteUser]
(
    -- Add the parameters for the stored procedure here
    @UserId NVARCHAR(450)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	DELETE [dbo].[AspNetUserClaims] WHERE [UserId] = @UserId
	DELETE [dbo].[AspNetUserLogins] WHERE [UserId] = @UserId
	
	DELETE [dbo].[AspNetUserTokens] WHERE [UserId] = @UserId
	DELETE [dbo].[AspNetUserRoles] WHERE [UserId] = @UserId

	DELETE FROM [dbo].[AspNetUsers] WHERE [Id] = @UserId
END