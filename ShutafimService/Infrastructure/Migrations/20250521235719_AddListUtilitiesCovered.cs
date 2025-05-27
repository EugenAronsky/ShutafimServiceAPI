using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShutafimService.Migrations
{
    /// <inheritdoc />
    public partial class AddListUtilitiesCovered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "UtilitiesCovered",
                table: "Listings",
                type: "integer[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtilitiesCovered",
                table: "Listings");
        }
    }
}
