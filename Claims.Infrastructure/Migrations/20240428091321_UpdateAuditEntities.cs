using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Claims.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HttpRequestType",
                table: "CoverAudits",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "CoverAudits",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoverId",
                table: "CoverAudits",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "HttpRequestType",
                table: "ClaimAudits",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "ClaimAudits",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClaimId",
                table: "ClaimAudits",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HttpRequestType",
                table: "CoverAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "CoverAudits",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "CoverId",
                table: "CoverAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "HttpRequestType",
                table: "ClaimAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "ClaimAudits",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimId",
                table: "ClaimAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
