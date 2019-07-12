using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StoreData.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "ItemTypeSubId",
                table: "Items",
                nullable: true);
           

            migrationBuilder.CreateTable(
                name: "ItemTypeSub",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypeSub", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeSubId",
                table: "Items",
                column: "ItemTypeSubId",
                unique: true,
                filter: "[ItemTypeSubId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypeSub_ItemTypeSubId",
                table: "Items",
                column: "ItemTypeSubId",
                principalTable: "ItemTypeSub",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypeSub_ItemTypeSubId",
                table: "Items");
            

            migrationBuilder.DropTable(
                name: "ItemTypeSub");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTypeSubId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTypeSubId",
                table: "Items");
 
 
        }
    }
}
