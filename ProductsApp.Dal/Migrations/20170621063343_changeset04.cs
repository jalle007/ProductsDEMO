using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsApp.Dal.Migrations
{
    public partial class changeset04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "EventId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "EventId",
                table: "Images",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
