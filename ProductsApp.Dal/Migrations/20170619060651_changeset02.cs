using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductsApp.Dal.Migrations
{
    public partial class changeset02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Images_ImageId1",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ImageId1",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ImageId1",
                table: "Likes");

            migrationBuilder.AlterColumn<long>(
                name: "ImageId",
                table: "Likes",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ImageId",
                table: "Likes",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ImageId",
                table: "Likes");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Likes",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "ImageId1",
                table: "Likes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ImageId1",
                table: "Likes",
                column: "ImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Images_ImageId1",
                table: "Likes",
                column: "ImageId1",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
