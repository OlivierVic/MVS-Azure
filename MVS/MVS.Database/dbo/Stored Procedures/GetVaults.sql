-- =============================================
-- Author:      <Olivier Vic>
-- Create Date: <27/07/2023>
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetVaults]
(
    @UserId NVARCHAR(450)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT DISTINCT [Vaults].[Id],
    [Title],
    [ContactId],
    [Sex],
    [FirstName],
    [LastName],
    [BirthDate],
    [BirthLocation],
    [Address],
    [ZipceCodeAndCity],
    [Country],
    [Email],
    [PhoneNumber],
    [Vaults].[UserId],
    [isArchived],
    [CreationDate],
    [IsDeleteAdmin],
    [DocumentDownload],
    [BirthName],
    [HaveBirthName],
    [Nationality], 
    [AcceptedCondition]
	FROM Vaults 
	INNER JOIN VaultUsers ON VaultUsers.UserId = @UserId
	WHERE (Vaults.UserId = @UserId OR Vaults.Id IN (SELECT DISTINCT VaultId FROM VaultUsers WHERE UserId = @UserId)) And (IsDeleteAdmin = 0 OR IsDeleteAdmin is NULL)
END
