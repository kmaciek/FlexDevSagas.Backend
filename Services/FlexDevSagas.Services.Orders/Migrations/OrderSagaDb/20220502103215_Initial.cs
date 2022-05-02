using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexDevSagas.Services.Orders.Migrations.OrderSagaDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderSagaStates",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSagaStates", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderSagaStates");
        }
    }
}
