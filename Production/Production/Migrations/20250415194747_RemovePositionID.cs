using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.Migrations
{
    /// <inheritdoc />
    public partial class RemovePositionID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
