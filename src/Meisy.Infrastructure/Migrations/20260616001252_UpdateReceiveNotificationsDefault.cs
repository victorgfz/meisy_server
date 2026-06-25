using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meisy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiveNotificationsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Users SET ReceiveNotifications = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Users SET ReceiveNotifications = 0;");
        }
    }
}
