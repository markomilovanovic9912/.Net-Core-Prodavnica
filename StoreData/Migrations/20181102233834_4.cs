using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StoreData.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemTypeId",
                table: "ItemTypeSub",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypeSub_ItemTypeId",
                table: "ItemTypeSub",
                column: "ItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTypeSub_ItemType_ItemTypeId",
                table: "ItemTypeSub",
                column: "ItemTypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTypeSub_ItemType_ItemTypeId",
                table: "ItemTypeSub");

            migrationBuilder.DropIndex(
                name: "IX_ItemTypeSub_ItemTypeId",
                table: "ItemTypeSub");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "ItemTypeSub");
        }
    }
}
