using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class StateAndStepColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Step",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "Boards");
        }
    }
}
