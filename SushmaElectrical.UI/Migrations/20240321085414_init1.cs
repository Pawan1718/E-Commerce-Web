using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SushmaElectrical.UI.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "PaymentModes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "PaymentModes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
