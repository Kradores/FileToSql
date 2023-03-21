using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToSql.Migrations
{
    /// <inheritdoc />
    public partial class AddFusedPriceTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FusedPrices",
                columns: table => new
                {
                    PartNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FusedPrices", x => x.PartNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FusedPrices");
        }
    }
}
