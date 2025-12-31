using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meisy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInputs_Inputs_InputId",
                table: "ProductInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInputs_Products_ProductId",
                table: "ProductInputs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInputs",
                table: "ProductInputs");

            migrationBuilder.RenameTable(
                name: "ProductInputs",
                newName: "Product_Inputs");

            migrationBuilder.RenameColumn(
                name: "AmountUsed",
                table: "Product_Inputs",
                newName: "ProductionMeasurementUnit");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInputs_InputId",
                table: "Product_Inputs",
                newName: "IX_Product_Inputs_InputId");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Products",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Inputs",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Product_Inputs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ProductionAmount",
                table: "Product_Inputs",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_Inputs",
                table: "Product_Inputs",
                columns: new[] { "ProductId", "InputId" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Inputs_CompanyId",
                table: "Product_Inputs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Inputs_Companies_CompanyId",
                table: "Product_Inputs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Inputs_Inputs_InputId",
                table: "Product_Inputs",
                column: "InputId",
                principalTable: "Inputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Inputs_Products_ProductId",
                table: "Product_Inputs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Inputs_Companies_CompanyId",
                table: "Product_Inputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Inputs_Inputs_InputId",
                table: "Product_Inputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Inputs_Products_ProductId",
                table: "Product_Inputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_Inputs",
                table: "Product_Inputs");

            migrationBuilder.DropIndex(
                name: "IX_Product_Inputs_CompanyId",
                table: "Product_Inputs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Product_Inputs");

            migrationBuilder.DropColumn(
                name: "ProductionAmount",
                table: "Product_Inputs");

            migrationBuilder.RenameTable(
                name: "Product_Inputs",
                newName: "ProductInputs");

            migrationBuilder.RenameColumn(
                name: "ProductionMeasurementUnit",
                table: "ProductInputs",
                newName: "AmountUsed");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Inputs_InputId",
                table: "ProductInputs",
                newName: "IX_ProductInputs_InputId");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Inputs",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInputs",
                table: "ProductInputs",
                columns: new[] { "ProductId", "InputId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInputs_Inputs_InputId",
                table: "ProductInputs",
                column: "InputId",
                principalTable: "Inputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInputs_Products_ProductId",
                table: "ProductInputs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
