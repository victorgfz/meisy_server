using System;
using Meisy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meisy.Infrastructure.Migrations
{
    [DbContext(typeof(MeisyDbContext))]
    [Migration("20260620180000_ConsolidatePushNotifications")]
    public partial class ConsolidatePushNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `PushSubscriptions` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Endpoint` varchar(500) NOT NULL,
                    `P256DH` varchar(255) NOT NULL,
                    `Auth` varchar(255) NOT NULL,
                    `UserId` int NOT NULL,
                    `ReceiveNotifications` tinyint(1) NOT NULL DEFAULT 1,
                    `LastUsedAt` datetime(6) NULL,
                    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
                    `UpdatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
                    CONSTRAINT `PK_PushSubscriptions` PRIMARY KEY (`Id`),
                    INDEX `IX_PushSubscriptions_UserId` (`UserId`)
                ) CHARACTER SET=utf8mb4;
                """);

            migrationBuilder.Sql("ALTER TABLE `PushSubscriptions` MODIFY COLUMN `Endpoint` varchar(500) NOT NULL;");
            migrationBuilder.Sql("ALTER TABLE `PushSubscriptions` MODIFY COLUMN `P256DH` varchar(255) NOT NULL;");
            migrationBuilder.Sql("ALTER TABLE `PushSubscriptions` MODIFY COLUMN `Auth` varchar(255) NOT NULL;");
            migrationBuilder.Sql(AddColumnIfMissing("PushSubscriptions", "ReceiveNotifications", "`ReceiveNotifications` tinyint(1) NOT NULL DEFAULT 1"));
            migrationBuilder.Sql(AddColumnIfMissing("PushSubscriptions", "LastUsedAt", "`LastUsedAt` datetime(6) NULL"));
            migrationBuilder.Sql(AddColumnIfMissing("PushSubscriptions", "CreatedAt", "`CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)"));
            migrationBuilder.Sql(AddColumnIfMissing("PushSubscriptions", "UpdatedAt", "`UpdatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)"));
            migrationBuilder.Sql(AddColumnIfMissing("Orders", "DeliveryReminderSentAt", "`DeliveryReminderSentAt` datetime(6) NULL"));

            migrationBuilder.Sql("""
                SET @index_exists := (
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.STATISTICS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'PushSubscriptions'
                      AND INDEX_NAME = 'IX_PushSubscriptions_Endpoint'
                );
                SET @create_index := IF(
                    @index_exists = 0,
                    'CREATE UNIQUE INDEX `IX_PushSubscriptions_Endpoint` ON `PushSubscriptions` (`Endpoint`)',
                    'SELECT 1'
                );
                PREPARE statement FROM @create_index;
                EXECUTE statement;
                DEALLOCATE PREPARE statement;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                SET @index_exists := (
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.STATISTICS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'PushSubscriptions'
                      AND INDEX_NAME = 'IX_PushSubscriptions_Endpoint'
                );
                SET @drop_index := IF(
                    @index_exists = 1,
                    'DROP INDEX `IX_PushSubscriptions_Endpoint` ON `PushSubscriptions`',
                    'SELECT 1'
                );
                PREPARE statement FROM @drop_index;
                EXECUTE statement;
                DEALLOCATE PREPARE statement;
                """);

            migrationBuilder.Sql(DropColumnIfExists("Orders", "DeliveryReminderSentAt"));
            migrationBuilder.Sql(DropColumnIfExists("PushSubscriptions", "ReceiveNotifications"));
            migrationBuilder.Sql(DropColumnIfExists("PushSubscriptions", "LastUsedAt"));
            migrationBuilder.Sql(DropColumnIfExists("PushSubscriptions", "CreatedAt"));
            migrationBuilder.Sql(DropColumnIfExists("PushSubscriptions", "UpdatedAt"));
        }

        private static string AddColumnIfMissing(string tableName, string columnName, string columnDefinition)
        {
            return $"""
                SET @column_exists := (
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = '{tableName}'
                      AND COLUMN_NAME = '{columnName}'
                );
                SET @add_column := IF(
                    @column_exists = 0,
                    'ALTER TABLE `{tableName}` ADD COLUMN {columnDefinition}',
                    'SELECT 1'
                );
                PREPARE statement FROM @add_column;
                EXECUTE statement;
                DEALLOCATE PREPARE statement;
                """;
        }

        private static string DropColumnIfExists(string tableName, string columnName)
        {
            return $"""
                SET @column_exists := (
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = '{tableName}'
                      AND COLUMN_NAME = '{columnName}'
                );
                SET @drop_column := IF(
                    @column_exists = 1,
                    'ALTER TABLE `{tableName}` DROP COLUMN `{columnName}`',
                    'SELECT 1'
                );
                PREPARE statement FROM @drop_column;
                EXECUTE statement;
                DEALLOCATE PREPARE statement;
                """;
        }
    }
}
