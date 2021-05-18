using Microsoft.EntityFrameworkCore.Migrations;

namespace GuideMVC_.Migrations
{
    public partial class newprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marriage_User_HusbandId",
                table: "Marriage");

            migrationBuilder.DropForeignKey(
                name: "FK_Marriage_User_WifeId",
                table: "Marriage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marriage",
                table: "Marriage");

            migrationBuilder.DropIndex(
                name: "IX_Marriage_HusbandId",
                table: "Marriage");

            migrationBuilder.DropIndex(
                name: "IX_Marriage_WifeId",
                table: "Marriage");

            migrationBuilder.DropColumn(
                name: "HusbandId",
                table: "Marriage");

            migrationBuilder.DropColumn(
                name: "WifeId",
                table: "Marriage");

            migrationBuilder.RenameTable(
                name: "Marriage",
                newName: "Marriages");

            migrationBuilder.AddColumn<int>(
                name: "MarriageId",
                table: "UserRelatives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HusbandMarriageId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WifeMarriageId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marriages",
                table: "Marriages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelatives_MarriageId",
                table: "UserRelatives",
                column: "MarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_HusbandMarriageId",
                table: "User",
                column: "HusbandMarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_WifeMarriageId",
                table: "User",
                column: "WifeMarriageId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Marriages_HusbandMarriageId",
                table: "User",
                column: "HusbandMarriageId",
                principalTable: "Marriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Marriages_WifeMarriageId",
                table: "User",
                column: "WifeMarriageId",
                principalTable: "Marriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelatives_Marriages_MarriageId",
                table: "UserRelatives",
                column: "MarriageId",
                principalTable: "Marriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Marriages_HusbandMarriageId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Marriages_WifeMarriageId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRelatives_Marriages_MarriageId",
                table: "UserRelatives");

            migrationBuilder.DropIndex(
                name: "IX_UserRelatives_MarriageId",
                table: "UserRelatives");

            migrationBuilder.DropIndex(
                name: "IX_User_HusbandMarriageId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_WifeMarriageId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marriages",
                table: "Marriages");

            migrationBuilder.DropColumn(
                name: "MarriageId",
                table: "UserRelatives");

            migrationBuilder.DropColumn(
                name: "HusbandMarriageId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WifeMarriageId",
                table: "User");

            migrationBuilder.RenameTable(
                name: "Marriages",
                newName: "Marriage");

            migrationBuilder.AddColumn<int>(
                name: "HusbandId",
                table: "Marriage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WifeId",
                table: "Marriage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marriage",
                table: "Marriage",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Marriage_User_HusbandId",
                table: "Marriage",
                column: "HusbandId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Marriage_User_WifeId",
                table: "Marriage",
                column: "WifeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
