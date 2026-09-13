using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapelTrail.Migrations;

/// <inheritdoc />
public partial class RenameToPapelTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AuditLogs_Users_UserId",
            table: "AuditLogs");

        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Users_OwnerId",
            table: "Documents");

        migrationBuilder.DropForeignKey(
            name: "FK_DocumentShares_Documents_DocumentId",
            table: "DocumentShares");

        migrationBuilder.DropForeignKey(
            name: "FK_DocumentShares_Users_SharedWithUserId",
            table: "DocumentShares");

        migrationBuilder.DropForeignKey(
            name: "FK_DocumentVersions_Documents_DocumentId",
            table: "DocumentVersions");

        migrationBuilder.DropForeignKey(
            name: "FK_DocumentVersions_Users_UploadedById",
            table: "DocumentVersions");

        migrationBuilder.DropForeignKey(
            name: "FK_ProcessingJobs_DocumentVersions_DocumentVersionId",
            table: "ProcessingJobs");

        migrationBuilder.DropForeignKey(
            name: "FK_ProcessingJobs_Documents_DocumentId",
            table: "ProcessingJobs");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Documents",
            table: "Documents");

        migrationBuilder.DropPrimaryKey(
            name: "PK_DocumentVersions",
            table: "DocumentVersions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_DocumentShares",
            table: "DocumentShares");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ProcessingJobs",
            table: "ProcessingJobs");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AuditLogs",
            table: "AuditLogs");

        migrationBuilder.RenameTable(
            name: "Documents",
            newName: "Papels");

        migrationBuilder.RenameTable(
            name: "DocumentVersions",
            newName: "PapelVersions");

        migrationBuilder.RenameTable(
            name: "DocumentShares",
            newName: "PapelShares");

        migrationBuilder.RenameTable(
            name: "ProcessingJobs",
            newName: "PapelProcessingJobs");

        migrationBuilder.RenameTable(
            name: "AuditLogs",
            newName: "PapelAuditLogs");

        migrationBuilder.RenameColumn(
            name: "DocumentId",
            table: "PapelVersions",
            newName: "PapelId");

        migrationBuilder.RenameColumn(
            name: "DocumentId",
            table: "PapelShares",
            newName: "PapelId");

        migrationBuilder.RenameColumn(
            name: "DocumentId",
            table: "PapelProcessingJobs",
            newName: "PapelId");

        migrationBuilder.RenameColumn(
            name: "DocumentVersionId",
            table: "PapelProcessingJobs",
            newName: "PapelVersionId");

        migrationBuilder.RenameIndex(
            name: "IX_Documents_OwnerId_CreatedAt",
            table: "Papels",
            newName: "IX_Papels_OwnerId_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_DocumentVersions_DocumentId_VersionNumber",
            table: "PapelVersions",
            newName: "IX_PapelVersions_PapelId_VersionNumber");

        migrationBuilder.RenameIndex(
            name: "IX_DocumentVersions_UploadedById",
            table: "PapelVersions",
            newName: "IX_PapelVersions_UploadedById");

        migrationBuilder.RenameIndex(
            name: "IX_DocumentShares_DocumentId_SharedWithUserId",
            table: "PapelShares",
            newName: "IX_PapelShares_PapelId_SharedWithUserId");

        migrationBuilder.RenameIndex(
            name: "IX_DocumentShares_SharedWithUserId",
            table: "PapelShares",
            newName: "IX_PapelShares_SharedWithUserId");

        migrationBuilder.RenameIndex(
            name: "IX_ProcessingJobs_DocumentId_CreatedAt",
            table: "PapelProcessingJobs",
            newName: "IX_PapelProcessingJobs_PapelId_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_ProcessingJobs_DocumentVersionId_Status",
            table: "PapelProcessingJobs",
            newName: "IX_PapelProcessingJobs_PapelVersionId_Status");

        migrationBuilder.RenameIndex(
            name: "IX_AuditLogs_CreatedAt",
            table: "PapelAuditLogs",
            newName: "IX_PapelAuditLogs_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_AuditLogs_EntityType_EntityId",
            table: "PapelAuditLogs",
            newName: "IX_PapelAuditLogs_EntityType_EntityId");

        migrationBuilder.RenameIndex(
            name: "IX_AuditLogs_UserId",
            table: "PapelAuditLogs",
            newName: "IX_PapelAuditLogs_UserId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Papels",
            table: "Papels",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PapelVersions",
            table: "PapelVersions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PapelShares",
            table: "PapelShares",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PapelProcessingJobs",
            table: "PapelProcessingJobs",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PapelAuditLogs",
            table: "PapelAuditLogs",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Papels_Users_OwnerId",
            table: "Papels",
            column: "OwnerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelVersions_Papels_PapelId",
            table: "PapelVersions",
            column: "PapelId",
            principalTable: "Papels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelVersions_Users_UploadedById",
            table: "PapelVersions",
            column: "UploadedById",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelShares_Papels_PapelId",
            table: "PapelShares",
            column: "PapelId",
            principalTable: "Papels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelShares_Users_SharedWithUserId",
            table: "PapelShares",
            column: "SharedWithUserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelProcessingJobs_Papels_PapelId",
            table: "PapelProcessingJobs",
            column: "PapelId",
            principalTable: "Papels",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelProcessingJobs_PapelVersions_PapelVersionId",
            table: "PapelProcessingJobs",
            column: "PapelVersionId",
            principalTable: "PapelVersions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PapelAuditLogs_Users_UserId",
            table: "PapelAuditLogs",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PapelAuditLogs_Users_UserId",
            table: "PapelAuditLogs");

        migrationBuilder.DropForeignKey(
            name: "FK_Papels_Users_OwnerId",
            table: "Papels");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelShares_Papels_PapelId",
            table: "PapelShares");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelShares_Users_SharedWithUserId",
            table: "PapelShares");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelVersions_Papels_PapelId",
            table: "PapelVersions");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelVersions_Users_UploadedById",
            table: "PapelVersions");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelProcessingJobs_PapelVersions_PapelVersionId",
            table: "PapelProcessingJobs");

        migrationBuilder.DropForeignKey(
            name: "FK_PapelProcessingJobs_Papels_PapelId",
            table: "PapelProcessingJobs");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Papels",
            table: "Papels");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PapelVersions",
            table: "PapelVersions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PapelShares",
            table: "PapelShares");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PapelProcessingJobs",
            table: "PapelProcessingJobs");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PapelAuditLogs",
            table: "PapelAuditLogs");

        migrationBuilder.RenameTable(
            name: "Papels",
            newName: "Documents");

        migrationBuilder.RenameTable(
            name: "PapelVersions",
            newName: "DocumentVersions");

        migrationBuilder.RenameTable(
            name: "PapelShares",
            newName: "DocumentShares");

        migrationBuilder.RenameTable(
            name: "PapelProcessingJobs",
            newName: "ProcessingJobs");

        migrationBuilder.RenameTable(
            name: "PapelAuditLogs",
            newName: "AuditLogs");

        migrationBuilder.RenameColumn(
            name: "PapelId",
            table: "DocumentVersions",
            newName: "DocumentId");

        migrationBuilder.RenameColumn(
            name: "PapelId",
            table: "DocumentShares",
            newName: "DocumentId");

        migrationBuilder.RenameColumn(
            name: "PapelId",
            table: "ProcessingJobs",
            newName: "DocumentId");

        migrationBuilder.RenameColumn(
            name: "PapelVersionId",
            table: "ProcessingJobs",
            newName: "DocumentVersionId");

        migrationBuilder.RenameIndex(
            name: "IX_Papels_OwnerId_CreatedAt",
            table: "Documents",
            newName: "IX_Documents_OwnerId_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_PapelVersions_PapelId_VersionNumber",
            table: "DocumentVersions",
            newName: "IX_DocumentVersions_DocumentId_VersionNumber");

        migrationBuilder.RenameIndex(
            name: "IX_PapelVersions_UploadedById",
            table: "DocumentVersions",
            newName: "IX_DocumentVersions_UploadedById");

        migrationBuilder.RenameIndex(
            name: "IX_PapelShares_PapelId_SharedWithUserId",
            table: "DocumentShares",
            newName: "IX_DocumentShares_DocumentId_SharedWithUserId");

        migrationBuilder.RenameIndex(
            name: "IX_PapelShares_SharedWithUserId",
            table: "DocumentShares",
            newName: "IX_DocumentShares_SharedWithUserId");

        migrationBuilder.RenameIndex(
            name: "IX_PapelProcessingJobs_PapelId_CreatedAt",
            table: "ProcessingJobs",
            newName: "IX_ProcessingJobs_DocumentId_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_PapelProcessingJobs_PapelVersionId_Status",
            table: "ProcessingJobs",
            newName: "IX_ProcessingJobs_DocumentVersionId_Status");

        migrationBuilder.RenameIndex(
            name: "IX_PapelAuditLogs_CreatedAt",
            table: "AuditLogs",
            newName: "IX_AuditLogs_CreatedAt");

        migrationBuilder.RenameIndex(
            name: "IX_PapelAuditLogs_EntityType_EntityId",
            table: "AuditLogs",
            newName: "IX_AuditLogs_EntityType_EntityId");

        migrationBuilder.RenameIndex(
            name: "IX_PapelAuditLogs_UserId",
            table: "AuditLogs",
            newName: "IX_AuditLogs_UserId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Documents",
            table: "Documents",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_DocumentVersions",
            table: "DocumentVersions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_DocumentShares",
            table: "DocumentShares",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ProcessingJobs",
            table: "ProcessingJobs",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AuditLogs",
            table: "AuditLogs",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Users_OwnerId",
            table: "Documents",
            column: "OwnerId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_DocumentVersions_Documents_DocumentId",
            table: "DocumentVersions",
            column: "DocumentId",
            principalTable: "Documents",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_DocumentVersions_Users_UploadedById",
            table: "DocumentVersions",
            column: "UploadedById",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_DocumentShares_Documents_DocumentId",
            table: "DocumentShares",
            column: "DocumentId",
            principalTable: "Documents",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_DocumentShares_Users_SharedWithUserId",
            table: "DocumentShares",
            column: "SharedWithUserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_ProcessingJobs_Documents_DocumentId",
            table: "ProcessingJobs",
            column: "DocumentId",
            principalTable: "Documents",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_ProcessingJobs_DocumentVersions_DocumentVersionId",
            table: "ProcessingJobs",
            column: "DocumentVersionId",
            principalTable: "DocumentVersions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_AuditLogs_Users_UserId",
            table: "AuditLogs",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }
}
