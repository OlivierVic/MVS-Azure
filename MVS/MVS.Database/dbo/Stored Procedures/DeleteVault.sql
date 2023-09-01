-- =============================================
-- Author:      <Olivier Vic - Droits Quotidiens Legal Tech>
-- Create Date: <20 July 2023>
-- Description: <Stored Procedures for delete Vault>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteVault]
(
    -- Add the parameters for the stored procedure here
    @VaultId NVARCHAR(450)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	DELETE [dbo].[Answers] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultAnswersAnticipationMeasures] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultAnswersPersonal] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultAnswersAnticipationMeasures] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultAnswersHeritage] WHERE [VaultId] = @VaultId

	DELETE [dbo].[VaultContacts] WHERE [VaultId] = @VaultId
	
	DELETE [dbo].[VaultHeritage] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultAnticipationMeasuresInfos] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultContracts] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultDocuments] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultFamilyInfos] WHERE [VaultId] = @VaultId
	DELETE [dbo].[VaultPersonalInfos] WHERE [VaultId] = @VaultId

	DELETE [dbo].[VaultUsers] WHERE [VaultId] = @VaultId

	DELETE FROM Vaults WHERE Id = @VaultId
END
