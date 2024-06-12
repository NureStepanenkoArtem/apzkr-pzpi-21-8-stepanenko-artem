using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAndObserve.Infrastructure.Migrations
{
    public partial class DeletedExstentionsIdinEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuardExstensionsId",
                table: "Equipment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuardExstensionsId",
                table: "Equipment",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
