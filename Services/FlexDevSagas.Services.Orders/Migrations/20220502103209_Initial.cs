using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexDevSagas.Services.Orders.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reservations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledMovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    OrderState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
