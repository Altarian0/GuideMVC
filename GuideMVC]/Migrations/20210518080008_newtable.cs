using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GuideMVC_.Migrations
{
    public partial class newtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marriage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HusbandId = table.Column<int>(type: "int", nullable: false),
                    WifeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DivorceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDivorced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marriage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marriage_User_HusbandId",
                        column: x => x.HusbandId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marriage_User_WifeId",
                        column: x => x.WifeId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Marriage_HusbandId",
                table: "Marriage",
                column: "HusbandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marriage_WifeId",
                table: "Marriage",
                column: "WifeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marriage");
        }
    }
}
