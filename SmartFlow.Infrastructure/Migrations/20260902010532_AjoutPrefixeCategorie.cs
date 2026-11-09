using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutPrefixeCategorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefixe",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefixe",
                table: "Categories");
        }
    }
}
