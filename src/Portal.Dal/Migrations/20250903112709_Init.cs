using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Dal.Migrations;

/// <inheritdoc />
public partial class Init : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "Sale",
            columns: table => new {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DateTimeSale = table.Column<DateTime>(type: "datetime2", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_Sale", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Sale_DateTimeSale",
            table: "Sale",
            column: "DateTimeSale");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "Sale");
    }
}
