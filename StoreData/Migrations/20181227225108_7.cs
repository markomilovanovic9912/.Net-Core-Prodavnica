using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StoreData.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeptHeaderImageUrl",
                table: "ItemType",
                newName: "ItemTypeHeaderImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemTypeHeaderImageUrl",
                table: "ItemType",
                newName: "DeptHeaderImageUrl");
        }
    }
}
