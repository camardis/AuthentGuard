using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthentGuard.Migrations
{
    /// <inheritdoc />
    public partial class nr3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AgrredToTerms",
                table: "RegisterModel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LastLoginUnixTimestamp",
                table: "RegisterModel",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgrredToTerms",
                table: "RegisterModel");

            migrationBuilder.DropColumn(
                name: "LastLoginUnixTimestamp",
                table: "RegisterModel");
        }
    }
}
