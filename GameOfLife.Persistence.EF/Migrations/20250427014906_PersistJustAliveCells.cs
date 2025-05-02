using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class PersistJustAliveCells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColsCount",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowsCount",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColsCount",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "RowsCount",
                table: "Boards");
        }
    }
}
