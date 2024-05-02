using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Claims.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditEntitiesFromEntityConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.RenameTable(
                name: "CoverAudits",
                newName: "CoverAudits",
                newSchema: "audit");

            migrationBuilder.RenameTable(
                name: "ClaimAudits",
                newName: "ClaimAudits",
                newSchema: "audit");

            migrationBuilder.AlterColumn<string>(
                name: "HttpRequestType",
                schema: "audit",
                table: "CoverAudits",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "HttpRequestType",
                schema: "audit",
                table: "ClaimAudits",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CoverAudits",
                schema: "audit",
                newName: "CoverAudits");

            migrationBuilder.RenameTable(
                name: "ClaimAudits",
                schema: "audit",
                newName: "ClaimAudits");

            migrationBuilder.AlterColumn<int>(
                name: "HttpRequestType",
                table: "CoverAudits",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<int>(
                name: "HttpRequestType",
                table: "ClaimAudits",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);
        }
    }
}
